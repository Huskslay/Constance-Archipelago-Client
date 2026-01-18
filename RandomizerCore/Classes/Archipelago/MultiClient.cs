using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Models;
using RandomizerCore.Classes.Data.Types.Locations;
using RandomizerCore.Classes.Handlers;
using RandomizerCore.Classes.Handlers.Files;
using RandomizerCore.Classes.Handlers.Messages;
using RandomizerCore.Classes.Handlers.State;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RandomizerCore.Classes.Archipelago;

public static class MultiClient
{
    public static string GameName => "Constance";

    private static bool IsConnected => session != null && session.Socket != null && session.Socket.Connected;
    private static bool isSubscribed = false;

    public static string Url { get; private set; }
    public static int Port { get; private set; }
    public static string SlotName { get; private set; }


    private static ArchipelagoSession session;
    private static int playerSlot;


    private static string playedDisconnectMessage = null;
    public static readonly Queue<string> recievedLocations = new();


    public static LoginResult Connect(string server, int port, string user, string password, out string errorMessage, out SlotData slotData)
    {
        Plugin.Logger.LogMessage($"Attempting to connect to '{server}:{port}' as {user}");
        // Establish session
        session = ArchipelagoSessionFactory.CreateSession(server, port);

        // Store information for later storage into save file
        Url = server;
        Port = port;
        SlotName = user;

        // Subscribe
        session.Socket.SocketClosed += OnSocketClosed;
        session.Socket.ErrorReceived += ErrorRecieved;
        session.Locations.CheckedLocationsUpdated += RecievedLocaion;
        RandomActionHandler.onLocationCheck.AddListener(OnLocationChecked);
        isSubscribed = true;

        // Attempt to login
        LoginResult result;
        try
        {
            result = session.TryConnectAndLogin(GameName, user, ItemsHandlingFlags.AllItems, password: password, requestSlotData: false);
            session.SetClientState(ArchipelagoClientState.ClientPlaying);
        }
        catch (Exception e)
        {
            Plugin.Logger.LogError($"Problem: {e.GetBaseException().Message}");
            result = new LoginFailure(e.GetBaseException().Message);
        }

        // Login failed, disconnect everything and send back an error
        if (!result.Successful)
        {
            Disconnect();

            LoginFailure failure = (LoginFailure)result;
            errorMessage = string.Join(" - ", failure.Errors);
            slotData = null;

            Plugin.Logger.LogError(errorMessage);
            return result;
        }

        // Login succeeded, intialize the rest and return success
        LoginSuccessful success = (LoginSuccessful)result;
        playerSlot = success.Slot;
        Plugin.Logger.LogMessage($"Successfully connected to '{server}:{port}' as {user} and slot {playerSlot}");

        errorMessage = null;
        slotData = new(session.DataStorage.GetSlotData());
        DataStorage.Initialize(session);

        return result;
    }



    // Location got, tell server
    private static void OnLocationChecked(ALocation location)
    {
        if (IsConnected)
        {
            long id = session.Locations.GetLocationIdFromName(GameName, location.GetName());
            session.Locations.CompleteLocationChecks(id);
        }
        else RandomStateHandler.DisconnectLocations.Add(location.GetName());
    }
    // Recieved Location from ap
    private static void RecievedLocaion(ReadOnlyCollection<long> newCheckedLocations)
    {
        foreach (long id in newCheckedLocations)
        {
            string locationName = session.Locations.GetLocationNameFromId(id);
            recievedLocations.Enqueue(locationName);
        }
    }



    // Try get a location from a disconnected save
    private static bool TryPopDisconnectLocation()
    {
        if (RandomStateHandler.DisconnectLocations.Count == 0) return false;

        string locationName = RandomStateHandler.DisconnectLocations[0];
        Plugin.Logger.LogMessage($"Getting '{locationName}' that was gotten offline");

        ALocation location = LocationHandler.I.GetFromName(locationName);
        if (location == null) return false;

        RandomStateHandler.UnObtainLocation(location);
        OnLocationChecked(location);
        RandomStateHandler.DisconnectLocations.RemoveAt(0);
        return true;
    }
    // Try get an location each frame
    private static bool TryPopRecievedLocation()
    {
        if (recievedLocations.Count == 0) return false;
        RandomStateHandler.GetLocation(recievedLocations.Dequeue());
        return true;
    }
    // Try get an item each frame
    private static bool TryRecieveItem()
    {
        if (!session.Items.Any()) return false;
        ItemInfo newItemInfo = session.Items.DequeueItem();
        RandomStateHandler.GetItem(
            newItemInfo.ItemName,
            newItemInfo.Player == null ? "null" : newItemInfo.Player.Name,
            newItemInfo.Player != null && newItemInfo.Player.Slot == playerSlot
        );
        return true;
    }
    public static void Update()
    {
        if (!IsConnected)
        {
            // Disconnect message must be established on main thread
            if (playedDisconnectMessage != null)
            {
                MessageHandler.CreateText(playedDisconnectMessage, permanent: true, UnityEngine.Color.red);
                playedDisconnectMessage = null;
            }
            return;
        }
        if (!RandomStateHandler.readyForItems) return;

        // Recieving items / locations (one per frame)
        if (TryRecieveItem()) return;
        if (TryPopRecievedLocation()) return;
        if (TryPopDisconnectLocation()) return;
    }



    // Search for what item a location has
    public static string ScoutItemName(string locationName, out string playerName, out bool isCurrentPlayer)
    {
        if (!IsConnected)
        {
            playerName = "error";
            isCurrentPlayer = false;
            return "reconnect";
        }

        long id = session.Locations.GetLocationIdFromName(GameName, locationName);
        ScoutedItemInfo info = session.Locations.ScoutLocationsAsync([id]).Result[id];

        playerName = info.Player.Name;
        isCurrentPlayer = info.Player.Slot == playerSlot;

        if (isCurrentPlayer) return info.ItemName;
        return $"{info.Player.Name}-{info.ItemName}";
    }



    // Called it socket disconnects without use of Disconnect()
    private static void OnSocketClosed(string reason)
    {
        session = null;
        string message = $"Socket Closed! Please quit and rejoin to reconnect!";
        playedDisconnectMessage = message;
        Plugin.Logger.LogError(message + $" ({reason})");
    }

    // Called it socket disconnects without use of Disconnect()
    private static void ErrorRecieved(Exception e, string message)
    {
        Plugin.Logger.LogError(e.ToString());
        OnSocketClosed(message);
    }

    // Disconnect client if connected
    public static void Disconnect()
    {
        // Remove listener and confirm if connected
        if (isSubscribed) RandomActionHandler.onLocationCheck.RemoveListener(OnLocationChecked);
        if (!IsConnected)
        {
            session = null;
            return;
        }

        // Unsubscrib
        session.Socket.SocketClosed -= OnSocketClosed;
        session.Socket.ErrorReceived -= ErrorRecieved;

        // Disconnect
        session.Socket.DisconnectAsync();
        session = null;
    }
}

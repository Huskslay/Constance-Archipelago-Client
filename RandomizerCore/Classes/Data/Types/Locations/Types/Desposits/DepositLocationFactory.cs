using Constance;
using RandomizerCore.Classes.Data.Types.Regions;
using RandomizerCore.Classes.Enums;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RandomizerCore.Classes.Data.Types.Locations.Types.Desposits;

public static class DepositLocationFactory
{
    public static FindObjectsInactive FindInactive => FindObjectsInactive.Exclude;

    public static Randomizable EntityToDepositType(CConCurrencyDepositEntity deposit)
    {
        string lowerName = deposit.name.ToLower();
        if (lowerName.Contains("lightstone")) return Randomizable.LightStone;
        else if (lowerName.Contains("currencyflower")) return Randomizable.CurrencyFlower;

        throw new NotSupportedException($"No ALocation for deposit of name {deposit.name}");
    }

    public static ALocation CreateDepositLocation(CConCurrencyDepositEntity deposit, Region region)
    {
#pragma warning disable IDE0066
        switch (EntityToDepositType(deposit))
        {
            case Randomizable.LightStone:
                return new LightStoneLocation(deposit, region);
            case Randomizable.CurrencyFlower:
                return new CurrencyFlowerLocation(deposit, region);
            default:
                throw new NotSupportedException($"No ALocation for deposit of name {deposit.name}");
        }
#pragma warning restore IDE0066
    }

    public static void PatchLoadedLevel(List<CConCurrencyDepositEntity> deposits, Region region)
    {
        List<CConCurrencyDepositEntity> lightStoneDeposits = [];
        List<CConCurrencyDepositEntity> currencyFlowerDeposits = [];

        foreach (CConCurrencyDepositEntity deposit in deposits)
        {
            switch (EntityToDepositType(deposit))
            {
                case Randomizable.LightStone:
                    lightStoneDeposits.Add(deposit);
                    break;
                case Randomizable.CurrencyFlower:
                    currencyFlowerDeposits.Add(deposit);
                    break;
                default:
                    throw new NotSupportedException($"No ALocation for deposit of name {deposit.name}");
            }
        }

        ALocation.BasicPatch<CConCurrencyDepositEntity, LightStoneLocation>(lightStoneDeposits, region);
        ALocation.BasicPatch<CConCurrencyDepositEntity, CurrencyFlowerLocation>(currencyFlowerDeposits, region);
    }
}
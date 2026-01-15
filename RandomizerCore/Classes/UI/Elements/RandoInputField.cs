using RandomizerCore.Patches.Files;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RandomizerCore.Classes.UI.Elements;

public class RandoInputField
{
    private static readonly int maxLength = 30;
    private static readonly float blinkTime = 0.5f;

    public RandoButton button;
    public bool active = false;

    private readonly string text;
    private string input;
    public bool numbersOnly;
    private readonly Action<RandoInputField, string> onUpdate;

    private float nextTime = 0;
    private bool showing = false;
    private readonly Action onClick;

    public RandoInputField(Transform transform, string text, string input, bool numbersOnly, Action<RandoInputField, string> onUpdate, Action onClick = null)
    {
        button = CConStartMenu_Patch.CreateButton(text, transform, OnClick, OnUpdate);

        this.text = text;
        this.input = input;
        this.numbersOnly = numbersOnly;
        this.onUpdate = onUpdate;
        this.onClick = onClick;

        this.onUpdate?.Invoke(this, input);
    }

    public void OnClick(RandoButton _) { onClick?.Invoke(); active = true; }

    public void SetInput(string newInput)
    {
        if (newInput.Length >= maxLength) return;

        input = newInput;
    }

    private void OnUpdate()
    {
        if (!active)
        {
            button.text = text + input.ToString();
            return;
        }

        if (Time.time > nextTime)
        {
            nextTime = Time.time + blinkTime;
            showing = !showing;
        }
        button.text = text + input.ToString() + (showing ? "<color=#FFFFFFFF>" : "<color=#FFFFFF00>") + "|</color>";

        if (Keyboard.current.backspaceKey.wasPressedThisFrame && input.Length > 0)
        { input = input[..^1]; onUpdate?.Invoke(this, input); }

        if (input.Length > maxLength) input = input[..maxLength];
        if (input.Length >= maxLength) return;

        if (Keyboard.current.digit0Key.wasPressedThisFrame || Keyboard.current.numpad0Key.wasPressedThisFrame)
        { input += "0"; onUpdate?.Invoke(this, input); }

        if (Keyboard.current.digit1Key.wasPressedThisFrame || Keyboard.current.numpad1Key.wasPressedThisFrame)
        { input += "1"; onUpdate?.Invoke(this, input); }

        if (Keyboard.current.digit2Key.wasPressedThisFrame || Keyboard.current.numpad2Key.wasPressedThisFrame)
        { input += "2"; onUpdate?.Invoke(this, input); }

        if (Keyboard.current.digit3Key.wasPressedThisFrame || Keyboard.current.numpad3Key.wasPressedThisFrame)
        { input += "3"; onUpdate?.Invoke(this, input); }

        if (Keyboard.current.digit4Key.wasPressedThisFrame || Keyboard.current.numpad4Key.wasPressedThisFrame)
        { input += "4"; onUpdate?.Invoke(this, input); }

        if (Keyboard.current.digit5Key.wasPressedThisFrame || Keyboard.current.numpad5Key.wasPressedThisFrame)
        { input += "5"; onUpdate?.Invoke(this, input); }

        if (Keyboard.current.digit6Key.wasPressedThisFrame || Keyboard.current.numpad6Key.wasPressedThisFrame)
        { input += "6"; onUpdate?.Invoke(this, input); }

        if (Keyboard.current.digit7Key.wasPressedThisFrame || Keyboard.current.numpad7Key.wasPressedThisFrame)
        { input += "7"; onUpdate?.Invoke(this, input); }

        if (Keyboard.current.digit8Key.wasPressedThisFrame || Keyboard.current.numpad8Key.wasPressedThisFrame)
        { input += "8"; onUpdate?.Invoke(this, input); }

        if (Keyboard.current.digit9Key.wasPressedThisFrame || Keyboard.current.numpad9Key.wasPressedThisFrame)
        { input += "9"; onUpdate?.Invoke(this, input); }

        if (numbersOnly) return;

        if (Keyboard.current.periodKey.wasPressedThisFrame)
        {
            input += ".";
            onUpdate?.Invoke(this, input);
        }

        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            input += "a";
            onUpdate?.Invoke(this, input);
        }

        if (Keyboard.current.bKey.wasPressedThisFrame)
        {
            input += "b";
            onUpdate?.Invoke(this, input);
        }

        if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            input += "c";
            onUpdate?.Invoke(this, input);
        }

        if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            input += "d";
            onUpdate?.Invoke(this, input);
        }

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            input += "e";
            onUpdate?.Invoke(this, input);
        }

        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            input += "f";
            onUpdate?.Invoke(this, input);
        }

        if (Keyboard.current.gKey.wasPressedThisFrame)
        {
            input += "g";
            onUpdate?.Invoke(this, input);
        }

        if (Keyboard.current.hKey.wasPressedThisFrame)
        {
            input += "h";
            onUpdate?.Invoke(this, input);
        }

        if (Keyboard.current.iKey.wasPressedThisFrame)
        {
            input += "i";
            onUpdate?.Invoke(this, input);
        }

        if (Keyboard.current.jKey.wasPressedThisFrame)
        {
            input += "j";
            onUpdate?.Invoke(this, input);
        }

        if (Keyboard.current.kKey.wasPressedThisFrame)
        {
            input += "k";
            onUpdate?.Invoke(this, input);
        }

        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            input += "l";
            onUpdate?.Invoke(this, input);
        }

        if (Keyboard.current.mKey.wasPressedThisFrame)
        {
            input += "m";
            onUpdate?.Invoke(this, input);
        }

        if (Keyboard.current.nKey.wasPressedThisFrame)
        {
            input += "n";
            onUpdate?.Invoke(this, input);
        }

        if (Keyboard.current.oKey.wasPressedThisFrame)
        {
            input += "o";
            onUpdate?.Invoke(this, input);
        }

        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            input += "p";
            onUpdate?.Invoke(this, input);
        }

        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            input += "q";
            onUpdate?.Invoke(this, input);
        }

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            input += "r";
            onUpdate?.Invoke(this, input);
        }

        if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            input += "s";
            onUpdate?.Invoke(this, input);
        }

        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            input += "t";
            onUpdate?.Invoke(this, input);
        }

        if (Keyboard.current.uKey.wasPressedThisFrame)
        {
            input += "u";
            onUpdate?.Invoke(this, input);
        }

        if (Keyboard.current.vKey.wasPressedThisFrame)
        {
            input += "v";
            onUpdate?.Invoke(this, input);
        }

        if (Keyboard.current.wKey.wasPressedThisFrame)
        {
            input += "w";
            onUpdate?.Invoke(this, input);
        }

        if (Keyboard.current.xKey.wasPressedThisFrame)
        {
            input += "x";
            onUpdate?.Invoke(this, input);
        }

        if (Keyboard.current.yKey.wasPressedThisFrame)
        {
            input += "y";
            onUpdate?.Invoke(this, input);
        }

        if (Keyboard.current.zKey.wasPressedThisFrame)
        {
            input += "z";
            onUpdate?.Invoke(this, input);
        }

    }
}
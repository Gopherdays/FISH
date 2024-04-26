using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

// We receive data as raw HID input reports. This struct
// describes the raw binary format of such a report.
[StructLayout(LayoutKind.Explicit, Size = 32)]
struct StupidControllerInputReport : IInputStateTypeInfo
{
    public FourCC format => new FourCC('H', 'I', 'D');

    [InputControl(name = "leftStick", layout = "Stick", format = "VC2B")]
    [InputControl(name = "leftStick/x", offset = 0, format = "BYTE",
        parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5")]
    [InputControl(name = "leftStick/left", offset = 0, format = "BYTE",
        parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp,clampMin=0,clampMax=0.5,invert")]
    [InputControl(name = "leftStick/right", offset = 0, format = "BYTE",
        parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp,clampMin=0.5,clampMax=1")]
    [InputControl(name = "leftStick/y", offset = 1, format = "BYTE",
        parameters = "invert,normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5")]
    [InputControl(name = "leftStick/up", offset = 1, format = "BYTE",
        parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp,clampMin=0,clampMax=0.5,invert")]
    [InputControl(name = "leftStick/down", offset = 1, format = "BYTE",
        parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp,clampMin=0.5,clampMax=1,invert=false")]
    [FieldOffset(1)] public byte leftStickX;
    [FieldOffset(2)] public byte leftStickY;

    [InputControl(name = "buttonWest", displayName = "X", bit = 4)]
    [InputControl(name = "buttonSouth", displayName = "A", bit = 5)]
    [InputControl(name = "buttonEast", displayName = "B", bit = 6)]
    [InputControl(name = "buttonNorth", displayName = "Y", bit = 7)]
    [FieldOffset(3)] public byte buttons;
}

[InputControlLayout(stateType = typeof(StupidControllerInputReport))]
#if UNITY_EDITOR
[InitializeOnLoad]
#endif
public class JoystickJoystickItIsAllJoystick : Joystick
{
    static JoystickJoystickItIsAllJoystick()
    {
        InputSystem.RegisterLayout<JoystickJoystickItIsAllJoystick>(
            matches: new InputDeviceMatcher()
            .WithInterface("HID")
            .WithManufacturer("Microntek              ")
            );
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init() {}
}
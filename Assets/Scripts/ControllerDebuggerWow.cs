using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerDebuggerWow : MonoBehaviour
{
    InputDevice device;

    private void Update()
    {
        device = InputSystem.GetDevice("Microntek             USB Joystick          ");
        print(device.displayName);
    }
}

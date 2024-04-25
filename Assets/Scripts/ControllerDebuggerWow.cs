using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerDebuggerWow : MonoBehaviour
{
    InputDevice device;

    private void Update()
    {
        device = InputSystem.GetDevice("Joystick");
        print(device.displayName);
    }
}

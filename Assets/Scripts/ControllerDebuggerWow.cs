using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerDebuggerWow : MonoBehaviour
{
    InputDevice device;

    private InputDevice FindJoystick()
    {
        for (int i = 0; i < 100; i++)
        {
            if (InputSystem.devices.Count <= i)
            {
                print("HELP HELP HELP");
                return null;
            }
            InputDevice test = InputSystem.GetDeviceById(i);
            if (test.name.Contains("Microntek"))
            {
                return InputSystem.GetDevice(test.name);
            }
        }
        return null;
    }

    private void Update()
    {
        print(device);
        if (!device.name.Contains("Microntek"))
            device = FindJoystick();
        print(device["stick"].ReadValueAsObject());
        print(device["trigger"].ReadValueAsObject());
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class InputHandler : MonoBehaviour
{
    public InputActionReference leftHandPress;
    public InputActionReference rightHandPress;

    void Awake()
    {
        leftHandPress.action.Enable();
        leftHandPress.action.performed += LeftHandPress;
        rightHandPress.action.Enable();
        rightHandPress.action.performed += RightHandPress;
    }

    private void LeftHandPress(InputAction.CallbackContext obj)
    {
        Debug.Log("Left Hand" + obj.ReadValue<float>());
    }

    private void RightHandPress(InputAction.CallbackContext obj)
    {
        Debug.Log("Right Hand" + obj.ReadValue<float>());
    }
}

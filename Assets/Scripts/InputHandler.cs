using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [SerializeField] InputActionReference leftHandPress;
    [SerializeField] InputActionReference rightHandPress;
    [SerializeField] InputActionReference leftHandHold;
    [SerializeField] InputActionReference rightHandHold;
    [SerializeField] WebHandler leftWebHandler;
    [SerializeField] WebHandler rightWebHandler;

    void Awake()
    {
        leftHandPress.action.Enable();
        leftHandPress.action.performed += LeftHandPress;
        rightHandPress.action.Enable();
        rightHandPress.action.performed += RightHandPress;
        leftHandHold.action.Enable();
        leftHandHold.action.performed += LeftHandHold;
        rightHandHold.action.Enable();
        rightHandHold.action.performed += RightHandHold;
    }

    private void LeftHandPress(InputAction.CallbackContext obj)
    {
        leftWebHandler.OnWebInput(obj.ReadValue<float>() > .5f);
    }

    private void RightHandPress(InputAction.CallbackContext obj)
    {
        rightWebHandler.OnWebInput(obj.ReadValue<float>() > .5f);
    }

    private void LeftHandHold(InputAction.CallbackContext obj)
    {
        leftWebHandler.OnHoldInput(obj.ReadValue<float>() > .5f);
    }

    private void RightHandHold(InputAction.CallbackContext obj)
    {
        rightWebHandler.OnHoldInput(obj.ReadValue<float>() > .5f);
    }
}

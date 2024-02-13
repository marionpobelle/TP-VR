using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [SerializeField] InputActionReference leftHandPress;
    [SerializeField] InputActionReference rightHandPress;
    [SerializeField] WebHandler leftWebHandler;
    [SerializeField] WebHandler rightWebHandler;

    void Awake()
    {
        leftHandPress.action.Enable();
        leftHandPress.action.performed += LeftHandPress;
        rightHandPress.action.Enable();
        rightHandPress.action.performed += RightHandPress;
    }

    private void LeftHandPress(InputAction.CallbackContext obj)
    {
        leftWebHandler.OnWebInput(obj.ReadValue<float>() > .5f);
    }

    private void RightHandPress(InputAction.CallbackContext obj)
    {
        rightWebHandler.OnWebInput(obj.ReadValue<float>() > .5f);
    }
}

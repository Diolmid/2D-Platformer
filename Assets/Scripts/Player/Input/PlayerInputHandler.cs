using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private Vector2 _movementInput;

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        _movementInput = context.ReadValue<Vector2>();
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {

    }
}
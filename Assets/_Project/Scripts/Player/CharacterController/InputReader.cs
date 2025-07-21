using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static PlayerControls;

public interface IInputReader
{
    Vector2 Direction { get; }
    void EnablePlayerActions();
}

[CreateAssetMenu(fileName = "InputReader", menuName = "ScriptableObjects/InputReader")]
public class InputReader : ScriptableObject, IPlayerActions, IInputReader
{
    public event UnityAction<Vector2> Move = delegate { };
    public event UnityAction<Vector2> Look = delegate { };
    public event UnityAction Jump = delegate { };
    public event UnityAction Interact = delegate { };
    public event UnityAction Escape = delegate { };
    public event UnityAction<InputAction.CallbackContext, int> Weapon = delegate { };
    protected PlayerControls inputActions;
    public PlayerControls InputActions
    {
        get
        {
            if (inputActions == null)
            {
                inputActions = new PlayerControls();
                inputActions.Player.SetCallbacks(this);
                inputActions.Enable();
            }
            return inputActions;
        }
    }
    public Vector2 Direction => InputActions.Player.Move.ReadValue<Vector2>();
    public Vector2 LookDirection => InputActions.Player.Look.ReadValue<Vector2>();
    public string InteractKey => InputActions.Player.Interact.GetBindingDisplayString();
    public void EnablePlayerActions()
    {
        inputActions?.Enable();
    }
    public void DisablePlayerActions()
    {
        if (inputActions != null)
        {
            inputActions.Player.SetCallbacks(null);
            inputActions.Disable();
        }
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        Move.Invoke(context.ReadValue<Vector2>());
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        Look.Invoke(context.ReadValue<Vector2>());
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Jump.Invoke();
        }
    }
    public void OnWeapon0(InputAction.CallbackContext context)
    {
        Weapon.Invoke(context, 0);
    }
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Interact.Invoke();
        }
    }
    public void OnEscape(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Escape.Invoke();
        }
    }
}
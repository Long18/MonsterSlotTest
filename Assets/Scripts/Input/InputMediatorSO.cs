using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputMediatorSO : ScriptableObject,
InputActions.IMainActions
{
    private InputActions _inputActions;
    public InputActions InputActions
    {
        get
        {
            CreateInstance();
            return _inputActions;
        }
    }

    public UnityAction OnSpaceClicked;

    private void OnEnable() => CreateInstance();
    private void OnDisable() => DisableAllInput();

    public void OnSpace(InputAction.CallbackContext context)
    {
        if (context.performed) OnSpaceClicked?.Invoke();
    }

    private void CreateInstance()
    {
        if (_inputActions != null) return;

        _inputActions = new InputActions();
        _inputActions.Enable();
        _inputActions.Main.SetCallbacks(this);
    }

    public void DisableAllInput()
    {
        _inputActions.Disable();
        _inputActions = null;
    }
}

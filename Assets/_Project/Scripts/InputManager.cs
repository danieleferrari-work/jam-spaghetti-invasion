using BaseTemplate;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    protected override bool isDontDestroyOnLoad => false;

    PlayerInputActions inputActions;

    public Vector2 Move => inputActions.Player.Move.ReadValue<Vector2>();
    public float Fire => inputActions.Player.Fire.ReadValue<float>();

    protected override void InitializeInstance()
    {
        base.InitializeInstance();
        inputActions = new PlayerInputActions();
    }

    void OnEnable()
    {
        inputActions.Player.Move.Enable();
        inputActions.Player.Fire.Enable();
    }

    void OnDisable()
    {
        inputActions.Player.Move.Disable();
        inputActions.Player.Fire.Disable();
    }
}

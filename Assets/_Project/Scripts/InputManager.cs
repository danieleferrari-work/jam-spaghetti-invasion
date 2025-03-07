using BaseTemplate;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    protected override bool isDontDestroyOnLoad => false;


    PlayerInputActions inputActions;

    public Vector2 GetMoveInput => inputActions.Player.Move.ReadValue<Vector2>();


    void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    void OnEnable()
    {
        inputActions.Player.Move.Enable();
    }

    void OnDisable()
    {
        inputActions.Player.Move.Disable();
    }
}

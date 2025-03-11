using BaseTemplate;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    protected override bool isDontDestroyOnLoad => false;

    PlayerInputActions inputActions;

    public Vector2 Move => inputActions.Player.Move.ReadValue<Vector2>();
    public float Fire => inputActions.Player.Fire.ReadValue<float>();

    private InputDevice currentDevice;

    public UnityAction<InputDevice> OnDeviceChanged;
    public UnityAction OnStartUsingGamepad;
    public UnityAction OnStartUsingMouseAndKeyboard;


    protected override void InitializeInstance()
    {
        base.InitializeInstance();
        inputActions = new PlayerInputActions();

        foreach (var action in inputActions)
        {
            action.performed += UpdateCurrentDevice;
        }
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

    void UpdateCurrentDevice(InputAction.CallbackContext context)
    {
        var newDevice = context.action.activeControl.device;

        if (newDevice != currentDevice)
        {
            currentDevice = newDevice;
            OnDeviceChanged?.Invoke(newDevice);

            if (currentDevice is Gamepad)
                OnStartUsingGamepad?.Invoke();

            if (currentDevice is Mouse || currentDevice is Keyboard)
                OnStartUsingMouseAndKeyboard?.Invoke();
        }
    }
}

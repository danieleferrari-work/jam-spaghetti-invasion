using BaseTemplate;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    [Header("Sensibility")]
    [SerializeField] private float gamepadSensMultiplier = 2f;
    [SerializeField] private float mouseSensMultiplier = 2f;

    public Vector2 Move => inputActions.Player.Move.ReadValue<Vector2>();
    public float Fire => inputActions.Player.Fire.ReadValue<float>();

    float GetHorizontalSensibility => PlayerPrefs.GetFloat("horizontalSensibility", 2);
    float GetVerticalSensibility => PlayerPrefs.GetFloat("verticalSensibility", 2);

    InputDevice currentDevice;
    PlayerInputActions inputActions;

    public UnityAction<InputDevice> OnDeviceChanged;
    public UnityAction OnStartUsingGamepad;
    public UnityAction OnStartUsingMouseAndKeyboard;

    protected override bool isDontDestroyOnLoad => false;

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
            {
                PlayerCameraManager.instance.SetSensibility(GetHorizontalSensibility * gamepadSensMultiplier * 100, GetVerticalSensibility * gamepadSensMultiplier * 100);
                OnStartUsingGamepad?.Invoke();
            }

            if (currentDevice is Mouse || currentDevice is Keyboard)
            {
                PlayerCameraManager.instance.SetSensibility(GetHorizontalSensibility * mouseSensMultiplier * 100, GetVerticalSensibility * mouseSensMultiplier * 100);
                OnStartUsingMouseAndKeyboard?.Invoke();
            }
        }
    }
}

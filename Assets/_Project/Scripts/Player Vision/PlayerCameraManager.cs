using BaseTemplate;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCameraManager : Singleton<PlayerCameraManager>
{
    [Header("References")]
    [SerializeField] CinemachineVirtualCamera povVirtualCamera;
    [SerializeField] CinemachineVirtualCamera lookAtVirtualCamera;

    CinemachineInputProvider inputProvider;
    CinemachinePOV povComponent;

    float defaultFov;
    bool zooming;
    private CinemachineVirtualCamera currentVirtualCamera;

    public bool Zooming => zooming;
    public CinemachineVirtualCamera PlayerPovVirtualCamera => povVirtualCamera;

    protected override bool isDontDestroyOnLoad => true;

    //input type
    private enum InputMethod { Mouse, Gamepad }
    private InputMethod currentInput = InputMethod.Mouse;
    private float mouseDeadZone = 2f;
    [Header("Sensibility")]
    [SerializeField] private float gamepadSensMultiplier = 2f;
    [SerializeField] private float mouseSensMultiplier = 2f;
    [SerializeField] private MainMenuManager settingsSens;

    [Header("Shake Settings")]
    [SerializeField] private float initialAmplitude = 0.5f;
    [SerializeField] private float maxAmplitude = 2.0f;
    [SerializeField] private float initialFrequency = 1.0f;
    [SerializeField] private float maxFrequency = 3.0f;
    [SerializeField] private float intensificationDuration = 5.0f;
    [SerializeField] private NoiseSettings noiseProfile; // Reference to a Cinemachine Noise Profile asset

    private CinemachineBasicMultiChannelPerlin currentCameraNoise;
    private float shakeTimer = 0f;
    private bool isShaking = false;
    private float currentAmplitude = 0f;
    private float currentFrequency = 0f;

    public void ChangeCamera(CinemachineVirtualCamera camera)
    {
        currentVirtualCamera.gameObject.SetActive(false);
        camera.gameObject.SetActive(true);

        currentVirtualCamera = camera;
    }

    public void SetPovRecentering(bool value)
    {
        povComponent.m_VerticalRecentering.m_enabled = value;
        povComponent.m_HorizontalRecentering.m_enabled = value;
    }

    protected override void InitializeInstance() // == Awake
    {
        base.InitializeInstance();

        inputProvider = povVirtualCamera.GetComponent<CinemachineInputProvider>();
        povComponent = povVirtualCamera.GetCinemachineComponent<CinemachinePOV>();

        GondolaAutoPilotArea.OnEnableAutoPilot += OnEnableAutoPilot;
        GondolaAutoPilotArea.OnDisableAutoPilot += OnDisableAutoPilot;

        GondolaMovementManager.OnStartMoving += OnGondolaStartMoving;
        GondolaMovementManager.OnStopMoving += OnGondolaStopMoving;

        defaultFov = povVirtualCamera.m_Lens.FieldOfView;
        currentVirtualCamera = povVirtualCamera;

        // Adds noise component to current camera
        SetShakingComponent(currentVirtualCamera);
    }

    void Update()
    {
        var zoomValue = InputManager.instance.Fire;

        if (zoomValue > 0)
            ZoomIn(zoomValue);
        else
            ZoomOut();

        if (isShaking)
        {
            UpdateShakeIntensity();
        }


        // gamepad input, CHANGE TO GAMEPAD
        if (Gamepad.current != null && Gamepad.current.wasUpdatedThisFrame)
        {
            if (currentInput != InputMethod.Gamepad)
            {
                currentInput = InputMethod.Gamepad;
                Debug.Log("GAMEPAD");

                povComponent.m_HorizontalAxis.m_MaxSpeed = settingsSens.hor_sensibility + gamepadSensMultiplier * 100;
                povComponent.m_VerticalAxis.m_MaxSpeed = settingsSens.ver_sensibility + gamepadSensMultiplier * 100;
            }
        }
        // mouse/keyboard input, CHANGE TO MOUSE/KEYBOARD
        else if (Mouse.current != null || Keyboard.current != null)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();

            bool isMouseMoved = mouseDelta.magnitude > mouseDeadZone;
            bool isKeyPressed = Keyboard.current.anyKey.wasPressedThisFrame;

            if (isMouseMoved || isKeyPressed)
            {
                if (currentInput != InputMethod.Mouse)
                {
                    currentInput = InputMethod.Mouse;
                    Debug.Log("MOUSE/TASTIERA");

                    povComponent.m_HorizontalAxis.m_MaxSpeed = settingsSens.hor_sensibility + mouseSensMultiplier * 100;
                    povComponent.m_VerticalAxis.m_MaxSpeed = settingsSens.ver_sensibility + mouseSensMultiplier * 100;
                }
            }
        }
    }

    void OnEnableAutoPilot(GondolaAutoPilotArea area)
    {
        LockCameraLooking(area.LookTarget);
    }

    void OnDisableAutoPilot()
    {
        UnlockCamera();
    }

    void OnGondolaStartMoving()
    {
        SetPovRecentering(true);
    }

    void OnGondolaStopMoving()
    {
        SetPovRecentering(false);
    }

    void ZoomIn(float value)
    {
        zooming = true;
        if (povVirtualCamera.m_Lens.FieldOfView > Params.instance.minFov)
            povVirtualCamera.m_Lens.FieldOfView -= value * Time.deltaTime * Params.instance.zoomInSpeed;
    }

    void ZoomOut()
    {
        zooming = false;
        if (povVirtualCamera.m_Lens.FieldOfView < defaultFov)
            povVirtualCamera.m_Lens.FieldOfView += Time.deltaTime * Params.instance.zoomOutSpeed;
    }

    void LockCameraLooking(Transform targetTrasform)
    {
        if (targetTrasform == null)
            return;

        inputProvider.enabled = false;

        lookAtVirtualCamera.LookAt = targetTrasform.transform;

        ChangeCamera(lookAtVirtualCamera);
    }

    void UnlockCamera()
    {
        ChangeCamera(povVirtualCamera);
        inputProvider.enabled = true;
    }

    private void SetShakingComponent(CinemachineVirtualCamera virtualCamera)
    {
        Debug.Log("SetShakingComponent");
        Debug.Log(virtualCamera);

        var newCameraNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        Debug.Log(newCameraNoise);
        if (newCameraNoise == null)
        {
            newCameraNoise = virtualCamera.AddCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }
        if (noiseProfile != null)
        {
            newCameraNoise.m_NoiseProfile = noiseProfile;
        }
        else
        {
            Debug.LogWarning("No noise profile assigned to ShakingEffect. Camera shake may not work properly.", this);
        }

        newCameraNoise.m_AmplitudeGain = 0f;
        newCameraNoise.m_FrequencyGain = 0f;

        currentCameraNoise = newCameraNoise;
    }

    private void UpdateShakeIntensity()
    {
        // Increase timer
        shakeTimer += Time.deltaTime;

        // Calculate intensity based on elapsed time (0 to 1 range)
        float intensityFactor = Mathf.Clamp01(shakeTimer / intensificationDuration);

        // Apply shake with gradually increasing intensity
        currentAmplitude = Mathf.Lerp(initialAmplitude, maxAmplitude, intensityFactor);
        currentFrequency = Mathf.Lerp(initialFrequency, maxFrequency, intensityFactor);

        currentCameraNoise.m_AmplitudeGain = currentAmplitude;
        currentCameraNoise.m_FrequencyGain = currentFrequency;
    }

    public void StartShaking()
    {
        SetShakingComponent(currentVirtualCamera);

        isShaking = true;
        shakeTimer = 0f;
    }

    public void StopShaking()
    {
        isShaking = false;
        currentCameraNoise.m_AmplitudeGain = 0f;
        currentCameraNoise.m_FrequencyGain = 0f;
        shakeTimer = 0f;
        currentAmplitude = 0f;
        currentFrequency = 0f;
    }
}

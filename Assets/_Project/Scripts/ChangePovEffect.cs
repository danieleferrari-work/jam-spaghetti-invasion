using UnityEngine;
using System.Collections;
using Cinemachine;

public class ChangePovEffect : MonoBehaviour
{
    [Header("Camera References")]
    [SerializeField] private CinemachineVirtualCamera destinationCamera;
    [SerializeField] private Animator anim;
    [SerializeField] private float cameraSwitchTransitionDuration = 1f;

    [Header("Shake Settings")]
    [SerializeField] private float initialAmplitude = 0.5f;
    [SerializeField] private float maxAmplitude = 2.0f;
    [SerializeField] private float initialFrequency = 1.0f;
    [SerializeField] private float maxFrequency = 3.0f;
    [SerializeField] private float intensificationDuration = 5.0f;
    [SerializeField] private NoiseSettings noiseProfile; // Reference to a Cinemachine Noise Profile asset

    private CinemachineVirtualCamera playerCamera;
    private CinemachineBasicMultiChannelPerlin cameraNoise;
    private float shakeTimer = 0f;
    private bool isShaking = false;
    private float currentAmplitude = 0f;
    private float currentFrequency = 0f;

    void Awake()
    {
        playerCamera = PlayerCameraManager.instance.currentVirtualCamera;

        // Get or add noise component
        cameraNoise = playerCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        if (cameraNoise == null)
        {
            cameraNoise = playerCamera.AddCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        // Assign noise profile if one is specified
        if (noiseProfile != null)
        {
            cameraNoise.m_NoiseProfile = noiseProfile;
        }
        else
        {
            Debug.LogWarning("No noise profile assigned to ShakingEffect. Camera shake may not work properly.", this);
        }
    }

    private void Start()
    {
        if (destinationCamera != null)
            destinationCamera.gameObject.SetActive(false);

        // Initialize with no shake
        cameraNoise.m_AmplitudeGain = 0f;
        cameraNoise.m_FrequencyGain = 0f;
    }

    private void Update()
    {
        if (isShaking)
        {
            UpdateShakeIntensity();
        }
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

        cameraNoise.m_AmplitudeGain = currentAmplitude;
        cameraNoise.m_FrequencyGain = currentFrequency;
    }

    /// <summary>
    /// Start applying the camera shake effect
    /// </summary>
    public void StartShaking()
    {
        isShaking = true;
        shakeTimer = 0f;
    }

    /// <summary>
    /// Smoothly stop the camera shake effect
    /// </summary>
    public void StopShaking()
    {
        isShaking = false;

        // Reset noise values
        cameraNoise.m_AmplitudeGain = 0f;
        cameraNoise.m_FrequencyGain = 0f;

    }

    /// <summary>
    /// Immediately switch to the destination camera
    /// </summary>
    public void ChangeCamera()
    {
        StartCoroutine(SwitchCameraCoroutine(destinationCamera));
    }

    /// <summary>
    /// Reset back to the player's camera
    /// </summary>
    public void ResetCamera()
    {
        StartCoroutine(SwitchCameraCoroutine(playerCamera));
    }

    private IEnumerator SwitchCameraCoroutine(CinemachineVirtualCamera targetCamera)
    {
        if (anim != null)
            anim.SetTrigger("ChangeCam");

        yield return new WaitForSeconds(cameraSwitchTransitionDuration);

        PlayerCameraManager.instance.ChangeCamera(targetCamera);

        // Stop any active shaking when switching cameras
        StopShaking();
    }

    private void OnDisable()
    {
        // Make sure to reset noise when disabled
        if (cameraNoise != null)
        {
            cameraNoise.m_AmplitudeGain = 0f;
            cameraNoise.m_FrequencyGain = 0f;
        }
    }
}
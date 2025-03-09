using BaseTemplate;
using Cinemachine;
using UnityEngine;

public class PlayerCameraManager : Singleton<PlayerCameraManager>
{
    [SerializeField] float sensibility;
    [SerializeField] CinemachineVirtualCamera povVirtualCamera;
    [SerializeField] CinemachineVirtualCamera lookAtVirtualCamera;

    CinemachineInputProvider inputProvider;
    CinemachinePOV povComponent;

    float defaultFov;
    bool zooming;

    public CinemachineVirtualCamera currentVirtualCamera;
    public bool Zooming => zooming;
    public CinemachineVirtualCamera PlayerPovVirtualCamera => povVirtualCamera;

    protected override bool isDontDestroyOnLoad => true;

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
    }

    void Update()
    {
        var zoomValue = InputManager.instance.Fire;

        if (zoomValue > 0)
            ZoomIn(zoomValue);
        else
            ZoomOut();
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
}

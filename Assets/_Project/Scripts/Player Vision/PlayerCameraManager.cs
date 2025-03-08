using BaseTemplate;
using Cinemachine;
using UnityEngine;

public class PlayerCameraManager : Singleton<PlayerCameraManager>
{
    [SerializeField] CinemachineVirtualCamera povVirtualCamera;
    [SerializeField] CinemachineVirtualCamera lookAtVirtualCamera;

    CinemachineInputProvider inputProvider;

    float defaultFov;
    bool zooming;

    public bool Zooming => zooming;
    public CinemachineVirtualCamera PlayerPovVirtualCamera => povVirtualCamera;

    protected override bool isDontDestroyOnLoad => true;

    protected override void InitializeInstance()
    {
        base.InitializeInstance();

        inputProvider = povVirtualCamera.GetComponent<CinemachineInputProvider>();

        GondolaAutoPilotArea.OnEnableAutoPilot += OnEnableAutoPilot;
        GondolaAutoPilotArea.OnDisableAutoPilot += OnDisableAutoPilot;

        defaultFov = povVirtualCamera.m_Lens.FieldOfView;
    }

    void OnEnableAutoPilot(GondolaAutoPilotArea area)
    {
        LockCameraLooking(area.LookTarget);
    }

    void OnDisableAutoPilot()
    {
        UnlockCamera();
    }

    void Update()
    {
        var zoomValue = InputManager.instance.Fire;

        if (zoomValue > 0)
            ZoomIn(zoomValue);
        else
            ZoomOut();
    }

    void ZoomIn(float value)
    {
        zooming = true;
        if (povVirtualCamera.m_Lens.FieldOfView > Params.instance.minFov)
            povVirtualCamera.m_Lens.FieldOfView -= value * Time.deltaTime *  Params.instance.zoomInSpeed;
    }

    void ZoomOut()
    {
        zooming = false;
        if (povVirtualCamera.m_Lens.FieldOfView < defaultFov)
            povVirtualCamera.m_Lens.FieldOfView += Time.deltaTime *  Params.instance.zoomOutSpeed;
    }

    void LockCameraLooking(Transform targetTrasform)
    {
        if (targetTrasform == null)
            return;

        inputProvider.enabled = false;

        lookAtVirtualCamera.LookAt = targetTrasform.transform;

        lookAtVirtualCamera.enabled = true;
        povVirtualCamera.enabled = false;
    }

    void UnlockCamera()
    {
        lookAtVirtualCamera.enabled = false;
        povVirtualCamera.enabled = true;

        inputProvider.enabled = true;
    }
}

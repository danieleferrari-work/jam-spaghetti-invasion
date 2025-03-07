using Cinemachine;
using UnityEngine;

public class PlayerCameraManager : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera povVirtualCamera;
    [SerializeField] CinemachineVirtualCamera lookAtVirtualCamera;
    [SerializeField] float minFov;
    [SerializeField] float zoomSpeed;

    CinemachineInputProvider inputProvider;

    float defaultFov;
    float maxZoom;

    void Awake()
    {
        inputProvider = povVirtualCamera.GetComponent<CinemachineInputProvider>();

        GondolaAutoPilotArea.OnEnableAutoPilot += OnEnableAutoPilot;
        GondolaAutoPilotArea.OnDisableAutoPilot += OnDisableAutoPilot;

        defaultFov = povVirtualCamera.m_Lens.FieldOfView;
        maxZoom = defaultFov - minFov;
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
        if (zoomValue > 0 && povVirtualCamera.m_Lens.FieldOfView > minFov)
        {
            povVirtualCamera.m_Lens.FieldOfView -= zoomValue * Time.deltaTime * zoomSpeed;
        }
        else if (zoomValue == 0 && povVirtualCamera.m_Lens.FieldOfView < defaultFov)
        {
            povVirtualCamera.m_Lens.FieldOfView += Time.deltaTime * zoomSpeed;
        }
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

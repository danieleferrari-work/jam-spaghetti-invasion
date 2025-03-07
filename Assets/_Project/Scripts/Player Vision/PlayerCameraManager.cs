using Cinemachine;
using UnityEngine;

public class PlayerCameraManager : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera povVirtualCamera;
    [SerializeField] CinemachineVirtualCamera lookAtVirtualCamera;


    CinemachinePOV pov;
    CinemachineInputProvider inputProvider;


    void Awake()
    {
        pov = povVirtualCamera.GetCinemachineComponent<CinemachinePOV>();
        inputProvider = povVirtualCamera.GetComponent<CinemachineInputProvider>();

        GondolaAutoPilotArea.OnEnableAutoPilot += OnEnableAutoPilot;
        GondolaAutoPilotArea.OnDisableAutoPilot += OnDisableAutoPilot;
    }

    private void OnEnableAutoPilot(GondolaAutoPilotArea area)
    {
        LockCameraLooking(area.LookTarget);
    }

    private void OnDisableAutoPilot()
    {
        UnlockCamera();
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

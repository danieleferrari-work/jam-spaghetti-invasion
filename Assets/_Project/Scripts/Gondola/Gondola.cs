using Cinemachine;
using UnityEngine;

public class Gondola : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera playerVirtualCamera;
    Rigidbody rb;
    public GameObject catOnBoat;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnLoopReset()
    {
        ResetTransform();
        ResetRigidbody();
        ResetCamera();
    }

    private void ResetTransform()
    {
        gameObject.transform.position = Vector3.zero;
    }

    private void ResetRigidbody()
    {
        rb.transform.position = Vector3.zero;
        rb.transform.rotation = Quaternion.identity;
        rb.velocity = Vector3.zero;
    }

    private void ResetCamera()
    {
        CinemachinePOV pov = playerVirtualCamera.GetCinemachineComponent<CinemachinePOV>();
        pov.m_HorizontalAxis.Value = 0.0f;
        pov.m_VerticalAxis.Value = 0.0f;
        playerVirtualCamera.PreviousStateIsValid = false;
    }
}

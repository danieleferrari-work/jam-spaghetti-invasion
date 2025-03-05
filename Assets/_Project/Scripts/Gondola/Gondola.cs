using Cinemachine;
using UnityEngine;

public class Gondola : MonoBehaviour
{
    public static float BaseHeight = 1;

    [SerializeField] CinemachineVirtualCamera playerVirtualCamera;
    Rigidbody rb;
    public GameObject catOnBoat;

    Vector3 BasePosition => new Vector3(0, BaseHeight, 0);


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        ResetTransform();
        ResetRigidbody();

        LoopsManager.OnStartLoop += OnStartLoop;
    }

    private void OnStartLoop()
    {
        ResetTransform();
        ResetRigidbody();
        ResetCamera();
    }

    private void ResetTransform()
    {
        gameObject.transform.position = BasePosition;
    }

    private void ResetRigidbody()
    {
        rb.transform.position = BasePosition;
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

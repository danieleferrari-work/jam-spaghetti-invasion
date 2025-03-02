using UnityEngine;

public class AlwaysLookAtCamera : MonoBehaviour
{

    [SerializeField] bool rotateXAxis;
    [SerializeField] bool rotateYAxis;
    [SerializeField] bool rotateZAxis;


    Camera mainCamera;
    Quaternion startRotation;

    void Awake()
    {
        mainCamera = Camera.main;
        startRotation = transform.rotation;
    }

    void FixedUpdate()
    {
        Vector3 dir = mainCamera.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(dir, Vector3.up) * startRotation;
        transform.rotation = rotation;
    }
}

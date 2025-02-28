using UnityEngine;

public class RigidbodyFollowCameraRotation : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float rotationSpeed = 1;

    Rigidbody rb;

    void Awake()
    {
        rb = target.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
            RotateRigidbody();
    }

    private void RotateRigidbody()
    {
        var currentRotation = rb.rotation;
        var targetRotation = Quaternion.LookRotation(Camera.main.transform.forward);
        var newRotation = Quaternion.Slerp(
            currentRotation,
            targetRotation,
            rotationSpeed * Time.fixedDeltaTime);
        rb.MoveRotation(newRotation);
    }
}

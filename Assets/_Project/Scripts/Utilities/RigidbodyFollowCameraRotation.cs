using UnityEngine;

public class RigidbodyFollowCameraRotation : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float rotationSpeed = 1;
    [Tooltip("Array that allow to lock axis rotation. 0 means lock, 1 means free.")]
    [SerializeField] Vector3 freeRotations;

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
        var targetRotation = Quaternion.LookRotation(Camera.main.transform.forward).eulerAngles;
        var lockedTargetRotation = Vector3.Scale(targetRotation, freeRotations);
        var targetRotationQuat = Quaternion.Euler(lockedTargetRotation);

        var newRotation = Quaternion.Slerp(
            currentRotation,
            targetRotationQuat,
            rotationSpeed * Time.fixedDeltaTime);
        rb.MoveRotation(newRotation);
    }
}

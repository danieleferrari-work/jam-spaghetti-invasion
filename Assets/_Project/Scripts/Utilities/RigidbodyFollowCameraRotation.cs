using UnityEngine;

public class RigidbodyFollowCameraRotation : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float rotationSpeed = 1;
    [Tooltip("Array that allow to lock axis rotation. 0 means lock, 1 means free.")]
    [SerializeField] Vector3 freeRotations;
    [SerializeField] float maxRotationAngle = 360;

    Rigidbody rb;

    Vector3 minAngle;
    Vector3 maxAngle;

    void Awake()
    {
        rb = target.GetComponent<Rigidbody>();

        minAngle = transform.rotation.eulerAngles + Vector3.one * -maxRotationAngle;
        maxAngle = transform.rotation.eulerAngles + Vector3.one * maxRotationAngle;
    }

    void FixedUpdate()
    {
        if (maxRotationAngle < 360)
            RotateRigidbodyClamped();
        else
            RotateRigidbody();
    }

    private void RotateRigidbodyClamped()
    {
        var currentRotation = rb.rotation;
        var lookRotation = Quaternion.LookRotation(Camera.main.transform.forward).eulerAngles;
        var lookRotationLocked = Vector3.Scale(lookRotation, freeRotations).ToAngles180();
        var lookRotationLockedClamped = lookRotationLocked.Clamp(minAngle, maxAngle).ToAngle360();

        var targetRotation = Quaternion.Euler(lookRotationLockedClamped);

        var newRotation = Quaternion.Slerp(
            currentRotation,
            targetRotation,
            rotationSpeed * Time.fixedDeltaTime);

        rb.MoveRotation(newRotation);
    }

    private void RotateRigidbody()
    {
        var currentRotation = rb.rotation;
        var lookRotation = Quaternion.LookRotation(Camera.main.transform.forward).eulerAngles;
        var lookRotationLocked = Vector3.Scale(lookRotation, freeRotations);

        var targetRotation = Quaternion.Euler(lookRotationLocked);

        var newRotation = Quaternion.Slerp(
            currentRotation,
            targetRotation,
            rotationSpeed * Time.fixedDeltaTime);

        rb.MoveRotation(newRotation);
    }
}

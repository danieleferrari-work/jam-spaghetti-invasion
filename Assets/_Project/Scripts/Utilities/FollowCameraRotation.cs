using UnityEngine;

public class FollowCameraRotation : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float rotationSpeed = 1;
    [Tooltip("Array that allow to lock axis rotation. 0 means lock, 1 means free.")]
    [SerializeField] Vector3 freeRotations;

    void Awake()
    {
        if (target == null)
            target = transform;
    }

    void FixedUpdate()
    {
        RotateTransform();
    }

    private void RotateTransform()
    {
        var cameraRotation = Camera.main.transform.rotation;
        var targetRotation = Vector3.Scale(cameraRotation.eulerAngles, freeRotations);
        target.transform.rotation = Quaternion.Slerp(target.transform.rotation, Quaternion.Euler(targetRotation), rotationSpeed * Time.fixedDeltaTime);
    }
}

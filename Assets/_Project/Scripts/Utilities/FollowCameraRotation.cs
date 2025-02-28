using UnityEngine;

public class FollowCameraRotation : MonoBehaviour
{
    [SerializeField] bool lockXRotation;
    [SerializeField] bool lockYRotation;
    [SerializeField] bool lookZRotation;
    [SerializeField] Transform target;
    [SerializeField] float rotationSpeed = 1;

    Vector3 lockVector = Vector3.zero;

    void Awake()
    {
        if (target == null)
            target = transform;

        lockVector = CalculateLockVector();
    }

    void FixedUpdate()
    {
        RotateTransform();
    }

    private void RotateTransform()
    {
        var cameraRotation = Camera.main.transform.rotation;
        var targetRotation = Vector3.Scale(cameraRotation.eulerAngles, lockVector);
        target.transform.rotation = Quaternion.Euler(targetRotation);
    }

    private Vector3 CalculateLockVector()
    {
        var result = Vector3.zero;
        if (!lockXRotation)
            result += new Vector3(1, 0, 0);

        if (!lockYRotation)
            result += new Vector3(0, 1, 0);

        if (!lookZRotation)
            result += new Vector3(0, 0, 1);

        return result;
    }
}

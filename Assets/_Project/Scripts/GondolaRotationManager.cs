using UnityEngine;

public class GondolaRotationManager : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 100;
    [SerializeField] float movementRange = 30;
    [SerializeField] GameObject camera; // TODO deve diventare il player?!

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponentInParent<Rigidbody>();
    }

    void FixedUpdate()
    {
        var currentRotation = rb.rotation;
        var targetRotation = Quaternion.LookRotation(camera.transform.forward);
        var newRotation = Quaternion.Slerp(
            currentRotation,
            targetRotation,
            rotationSpeed * Time.fixedDeltaTime);
        rb.MoveRotation(newRotation);
    }
}

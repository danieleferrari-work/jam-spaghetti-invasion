using UnityEngine;

public class GondolaMovementManagerV2 : MonoBehaviour
{
    [SerializeField] float maxSpeed = 100;
    [SerializeField] float rotationSpeed;
    [SerializeField] float defaultAcceleration = 10;
    [SerializeField] float pushForce = 10;
    [SerializeField] float pushDelay = 1.5f;


    Rigidbody rb;
    PlayerInputActions inputActions;

    float lastPushTime;


    void Awake()
    {
        rb = GetComponentInParent<Rigidbody>();
        inputActions = new PlayerInputActions();

        lastPushTime = Time.time + pushDelay * 2;
    }

    void OnEnable()
    {
        inputActions.Player.Move.Enable();
    }

    void OnDisable()
    {
        inputActions.Player.Move.Disable();
    }

    void FixedUpdate()
    {
        var inputValue = inputActions.Player.Move.ReadValue<Vector2>();

        if (inputValue.magnitude <= 0)
            return;

        if (rb.velocity.magnitude < maxSpeed)
        {
            AddDefaultAcceleration(inputValue.y);

            if (Time.time - lastPushTime > pushDelay)
            {
                Push(inputValue.y);
            }
        }

        ApplyRotation(inputValue.x);
    }

    private void ApplyRotation(float rotation)
    {
        var targetRotation = rb.rotation.eulerAngles + Vector3.up * rotation * rotationSpeed;
        rb.MoveRotation(Quaternion.Euler(targetRotation));
    }

    private void AddDefaultAcceleration(float value)
    {
        rb.AddForce(rb.transform.forward * value * defaultAcceleration, ForceMode.Acceleration);
    }

    private void Push(float value)
    {
        rb.AddForce(rb.transform.forward * value * pushForce, ForceMode.Impulse);
        lastPushTime = Time.time;
    }
}

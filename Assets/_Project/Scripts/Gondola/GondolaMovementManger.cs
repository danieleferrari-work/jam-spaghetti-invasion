using UnityEngine;

public class GondolaMovementManager : MonoBehaviour
{
    [SerializeField] float maxSpeed = 100;
    [SerializeField] float rotationSpeed;
    [SerializeField] float defaultAcceleration = 10;
    [SerializeField] float pushForce = 10;
    [SerializeField] float pushDelay = 6f;
    [SerializeField] float pushDuration = 3f;
    [SerializeField] GameObject model;


    Rigidbody rb;

    float lastPushTime;
    bool autoPilotEnabled;

    bool IsTimerElapsed => Time.time - lastPushTime > pushDelay;
    bool IsPushComplete => Time.time - lastPushTime > pushDelay + pushDuration;

    public Vector3 Velocity => rb.velocity;


    void Awake()
    {
        rb = GetComponentInParent<Rigidbody>();

        lastPushTime = Time.time + pushDelay * 2;
    }

    void Start()
    {
        GondolaAutoPilotArea.OnEnableAutoPilot += OnEnableAutoPilot;
        GondolaAutoPilotArea.OnDisableAutoPilot += OnDisableAutoPilot;
    }

    void FixedUpdate()
    {
        if (autoPilotEnabled)
            return;

        var inputValue = InputManager.instance.Move;

        if (inputValue.magnitude > 0)
        {
            if (rb.velocity.magnitude < maxSpeed)
            {
                AddDefaultAcceleration(inputValue.y);

                if (IsTimerElapsed)
                {
                    Push(inputValue.y);

                    if (IsPushComplete)
                        lastPushTime = Time.time;
                }
            }
            ApplyRotation(inputValue.x);
        }
    }

    void OnEnableAutoPilot(GondolaAutoPilotArea trigger)
    {
        rb.isKinematic = true;
        autoPilotEnabled = true;
    }

    void OnDisableAutoPilot()
    {
        rb.isKinematic = false;
        rb.MovePosition(transform.position);
        rb.MoveRotation(transform.rotation);
        rb.velocity = Vector3.zero;

        autoPilotEnabled = false;
    }

    void ApplyRotation(float rotation)
    {
        var targetRotation = rb.rotation.eulerAngles + Vector3.up * rotation * rotationSpeed;
        rb.MoveRotation(Quaternion.Euler(targetRotation));
    }

    void AddDefaultAcceleration(float value)
    {
        rb.AddForce(rb.transform.forward * value * defaultAcceleration, ForceMode.Acceleration);
    }

    void Push(float value)
    {
        rb.AddForce(rb.transform.forward * value * pushForce, ForceMode.Force);
    }
}

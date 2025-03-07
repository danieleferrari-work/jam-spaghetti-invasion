using UnityEngine;

public class GondolaMovementManager : MonoBehaviour
{
    [SerializeField] GameObject model;


    Rigidbody rb;

    float lastPushTime;
    bool autoPilotEnabled;

    bool IsTimerElapsed => Time.time - lastPushTime > Params.instance.rowPushDelay;
    bool IsPushComplete => Time.time - lastPushTime > Params.instance.rowPushDelay + Params.instance.rowPushDuration;

    public Vector3 Velocity => rb.velocity;


    void Awake()
    {
        rb = GetComponentInParent<Rigidbody>();

        lastPushTime = Time.time + Params.instance.rowPushDelay * 2;
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
            if (rb.velocity.magnitude < Params.instance.gondolaMaxSpeed)
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
        var targetRotation = rb.rotation.eulerAngles + Vector3.up * rotation * Params.instance.gondolaRotationSpeed;
        rb.MoveRotation(Quaternion.Euler(targetRotation));
    }

    void AddDefaultAcceleration(float value)
    {
        rb.AddForce(rb.transform.forward * value * Params.instance.gondolaDefaultAcceleration, ForceMode.Acceleration);
    }

    void Push(float value)
    {
        rb.AddForce(rb.transform.forward * value * Params.instance.rowPushForce, ForceMode.Force);
    }
}

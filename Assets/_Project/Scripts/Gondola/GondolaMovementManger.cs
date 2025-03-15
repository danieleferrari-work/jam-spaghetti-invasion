using UnityEngine;
using UnityEngine.Events;

public class GondolaMovementManager : MonoBehaviour
{
    [SerializeField] GameObject model;

    Rigidbody rb;

    float lastPushTime;
    bool autoPilotEnabled;

    bool isFlipped;
    bool isMoving;
    bool pauseMovement = false;
    bool IsTimerElapsed => Time.time - lastPushTime > Params.instance.rowPushDelay;
    bool IsPushComplete => Time.time - lastPushTime > Params.instance.rowPushDelay + Params.instance.rowPushDuration;
    float minVelocity = 0.5f;

    public bool IsFlipped { get => isFlipped; set => isFlipped = value; }
    public bool PauseMovement { get => pauseMovement; set => pauseMovement = value; }

    public static UnityAction OnDoRowing;
    public static UnityAction OnStartMoving;
    public static UnityAction OnStopMoving;



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
        if (autoPilotEnabled || PauseMovement)
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

        if (rb.velocity.magnitude <= minVelocity)
            StopMoving();
        else if (rb.velocity.magnitude >= minVelocity)
            StartMoving();
    }

    private void StartMoving()
    {
        if (!isMoving)
        {
            isMoving = true;
            OnStartMoving?.Invoke();
        }
    }

    private void StopMoving()
    {
        if (isMoving)
        {
            isMoving = false;
            OnStopMoving?.Invoke();
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
        if (isFlipped)
            rotation = -rotation;

        var targetRotation = rb.rotation.eulerAngles + Vector3.up * rotation * Params.instance.gondolaRotationSpeed;
        rb.MoveRotation(Quaternion.Euler(targetRotation));
    }

    void AddDefaultAcceleration(float value)
    {
        rb.AddForce(rb.transform.forward * value * Params.instance.gondolaDefaultAcceleration, ForceMode.Acceleration);
    }

    void Push(float value)
    {
        OnDoRowing?.Invoke();
        rb.AddForce(rb.transform.forward * value * Params.instance.rowPushForce, ForceMode.Force);
    }
}

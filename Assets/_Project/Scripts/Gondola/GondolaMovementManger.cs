using UnityEngine;
using UnityEngine.Events;

public class GondolaMovementManager : MonoBehaviour
{
    [SerializeField] GameObject model;

    Rigidbody rb;

    float lastPushTime;
    bool autoPilotEnabled;
    bool isMoving;

    bool isFlipped;
    bool pauseMovement = false;
    bool IsTimerElapsed => Time.time - lastPushTime > Params.instance.rowPushDelay;
    bool IsPushComplete => Time.time - lastPushTime > Params.instance.rowPushDelay + Params.instance.rowPushDuration;

    public bool IsFlipped { get => isFlipped; set => isFlipped = value; }
    public bool PauseMovement { get => pauseMovement; set => pauseMovement = value; }

    public static UnityAction OnStartRowing;
    public static UnityAction OnStopRowing;
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
            if (!isMoving)
            {
                StartMoving();
            }

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
            if (!isFlipped)
            {
                ApplyRotation(inputValue.x);
            }
            else
            {
                ApplyRotation(- inputValue.x);
            }
        }
        else if (isMoving)
        {
            StopMoving();
        }
    }

    private void StartMoving()
    {
        isMoving = true;
        OnStartMoving?.Invoke();
    }

    private void StopMoving()
    {
        isMoving = false;
        OnStopMoving?.Invoke();
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

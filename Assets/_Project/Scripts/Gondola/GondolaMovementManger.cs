using UnityEngine;
using UnityEngine.Video;

public class GondolaMovementManager : MonoBehaviour
{
    [SerializeField] float maxSpeed = 100;
    [SerializeField] float rotationSpeed;
    [SerializeField] float defaultAcceleration = 10;
    [SerializeField] float pushForce = 10;
    [SerializeField] float pushDelay = 6f;
    [SerializeField] float pushDuration = 3f;


    Rigidbody rb;
    PlayerInputActions inputActions;
    GondolaFloatingManager gondolaFloatingManager;
    GondolaAutoPilot autoPilot;

    float lastPushTime;
    bool autoPilotEnabled;

    bool IsTimerElapsed => Time.time - lastPushTime > pushDelay;
    bool IsPushComplete => Time.time - lastPushTime > pushDelay + pushDuration;


    void Awake()
    {
        rb = GetComponentInParent<Rigidbody>();
        gondolaFloatingManager = rb.GetComponentInChildren<GondolaFloatingManager>();
        autoPilot = rb.GetComponentInChildren<GondolaAutoPilot>();

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
        if (autoPilotEnabled)
            return;
            
        var inputValue = inputActions.Player.Move.ReadValue<Vector2>();

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

        ApplyOscillationBobbing();
        ApplyOscillationRoll();
    }

    public void EnableAutoPilot(GondolaAutoPilotTrigger trigger)
    {
        Debug.Log("Enable autopiloting");
        autoPilotEnabled = true;
        autoPilot.GoTo(trigger);
    }

    public void DisableAutoPilot()
    {
        Debug.Log("Disable autopiloting");

        rb.MovePosition(transform.position);
        rb.MoveRotation(transform.rotation);
        rb.velocity = Vector3.zero;

        autoPilotEnabled = false;
    }

    private void ApplyOscillationRoll()
    {
        var roll = gondolaFloatingManager.CalculateOscillationRoll();
        rb.MoveRotation(Quaternion.Euler(new Vector3(rb.rotation.eulerAngles.x, rb.rotation.eulerAngles.y, roll)));
    }

    private void ApplyOscillationBobbing()
    {
        var bobbing = gondolaFloatingManager.CalculateOscillationBobbing();
        rb.MovePosition(new Vector3(rb.transform.position.x, bobbing, rb.transform.position.z));
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
        rb.AddForce(rb.transform.forward * value * pushForce, ForceMode.Force);
    }
}

using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class GondolaMovementManager : MonoBehaviour
{
    [SerializeField] float maxSpeed = 100;
    [SerializeField] float lateralDegradation = .5f;
    [SerializeField] float defaultAcceleration = 10;
    [SerializeField] float pushForce = 10;
    [SerializeField] float pushDelay = 1.5f;


    Rigidbody rb;
    PlayerInput playerInput;

    float lastPushTime;
    Vector3 inputMovement;
    Vector3 movement;


    void Awake()
    {
        rb = GetComponentInParent<Rigidbody>();
        playerInput = FindObjectOfType<PlayerInput>();

        lastPushTime = Time.time + pushDelay * 2;
        playerInput.actions["Move"].started += OnMoveStarted;
        playerInput.actions["Move"].canceled += OnMoveCanceled;
    }

    void FixedUpdate()
    {
        if (inputMovement.magnitude <= 0)
            return;

        CalculateMovement();

        if (rb.velocity.magnitude < maxSpeed)
        {
            AddDefaultAcceleration();
        }

        if (Time.time - lastPushTime > pushDelay)
        {
            Push();
        }
    }

    public void OnMoveStarted(CallbackContext context)
    {
        inputMovement = context.ReadValue<Vector2>();
    }

    public void OnMoveCanceled(CallbackContext context)
    {
        inputMovement = Vector2.zero;
    }

    private void CalculateMovement()
    {
        var forwardMovement = rb.transform.forward * inputMovement.y;
        var lateralMovement = rb.transform.right * inputMovement.x * lateralDegradation;

        movement = forwardMovement + lateralMovement;
    }

    private void AddDefaultAcceleration()
    {
        rb.AddForce(movement * defaultAcceleration, ForceMode.Acceleration);
    }

    private void Push()
    {
        rb.AddForce(movement * pushForce, ForceMode.Impulse);
        lastPushTime = Time.time;
    }
}

using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class GondolaMovementManager : MonoBehaviour
{
    [SerializeField] float maxSpeed = 100;
    [SerializeField] float defaultAcceleration = 10;
    [SerializeField] float pushForce = 10;
    [SerializeField] float pushDelay = 1.5f;


    Rigidbody rb;
    PlayerInput playerInput;

    float lastPushTime;
    bool isMoving = false;
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
        if (isMoving)
        {
            if (movement.magnitude <= 0)
                return;

            if (rb.velocity.magnitude < maxSpeed)
            {
                AddDefaultAcceleration(movement);
            }

            if (Time.time - lastPushTime > pushDelay)
            {
                Push(movement);
            }
        }
    }

    public void OnMoveStarted(CallbackContext context)
    {
        isMoving = true;
        var inputValue = context.ReadValue<Vector2>();
        movement = new Vector3(inputValue.x, 0, inputValue.y);
    }

    public void OnMoveCanceled(CallbackContext context)
    {
        isMoving = false;
    }

    private void AddDefaultAcceleration(Vector3 movement)
    {
        rb.AddForce(movement * defaultAcceleration, ForceMode.Acceleration);
    }

    private void Push(Vector3 movement)
    {
        rb.AddForce(movement * pushForce, ForceMode.Impulse);
        lastPushTime = Time.time;
    }
}

using UnityEngine;

public class GondolaMovementManager : MonoBehaviour
{
    [SerializeField] float maxSpeed = 100;
    [SerializeField] float lateralDegradation = .5f;
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

        var movement = CalculateMovement(inputValue);

        if (rb.velocity.magnitude < maxSpeed)
        {
            AddDefaultAcceleration(movement);
        }

        if (Time.time - lastPushTime > pushDelay)
        {
            Push(movement);
        }
    }

    private Vector3 CalculateMovement(Vector2 inputValue)
    {
        var forwardMovement = rb.transform.forward * inputValue.y;
        var lateralMovement = rb.transform.right * inputValue.x * lateralDegradation;

        return forwardMovement + lateralMovement;
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

using System;
using System.Collections;
using UnityEngine;

public class GondolaMovementManager : MonoBehaviour
{


    [SerializeField] bool enableConstantAcceleration = true;
    [SerializeField] bool enablePush = true;
    [SerializeField] float maxSpeed = 100;
    [SerializeField] float defaultAcceleration = 10;
    [SerializeField] float pushForce = 10;
    [SerializeField] float pushDelay = 1.5f;
    [SerializeField] ForceMode forceMode;

    Rigidbody rb;
    float lastPushTime;

    void Awake()
    {
        rb = GetComponentInParent<Rigidbody>();
        lastPushTime = Time.time + pushDelay * 2;
    }

    void FixedUpdate()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        var movement = new Vector3(horizontal, 0, vertical);

        if (movement.magnitude <= 0)
            return;

        if (enableConstantAcceleration && rb.velocity.magnitude < maxSpeed)
        {
            AddDefaultAcceleration(movement);
        }

        if (enablePush && Time.time - lastPushTime > pushDelay)
        {
            Push(movement);
        }
    }

    private void AddDefaultAcceleration(Vector3 movement)
    {
        rb.AddForce(movement * defaultAcceleration, ForceMode.Acceleration);
    }

    private void Push(Vector3 movement)
    {
        rb.AddForce(movement * 10, forceMode);
        lastPushTime = Time.time;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GondolaRotationManager : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 100;
    [SerializeField] float movementRange = 30;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponentInParent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // var horizontal = Input.GetAxis("Horizontal");

        // if (horizontal == 0)
        //     return;

        // var rotation = new Vector3(0, horizontal, 0);

        // rb.AddTorque(rotation * rotationSpeed, ForceMode.Acceleration);
    }
}

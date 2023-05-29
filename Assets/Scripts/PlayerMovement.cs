using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float forwardAcceleration;
    [SerializeField] float backwardAcceleration;
    [SerializeField] float turnAcceleration;

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(forwardAcceleration * transform.up);

        }
        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(backwardAcceleration * -transform.up);

        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddTorque(-turnAcceleration);

        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddTorque(turnAcceleration);

        }
    }
}

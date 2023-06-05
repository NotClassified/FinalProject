using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public event System.Action<Vector2> Position;
    public static event System.Action<Vector2> PlayerVelocity;

    [SerializeField] float defaultAcceleration;
    float currentAcceleration;
    float movementVerticle;
    float movementHorizontal;
    Vector2 movement;

    [SerializeField] float turnSpeed;
    float targetAngle;

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ResetAcceleration();
    }

    private void Update()
    {
        ResetAcceleration();
        //verticle movement (y-axis)
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S))
        {
            movementVerticle = 0;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            movementVerticle = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            movementVerticle = -1;
        }
        else
        {
            movementVerticle = 0;
        }

        //horizontal movement (x-axis)
        if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A))
        {
            movementHorizontal = 0;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            movementHorizontal = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            movementHorizontal = -1;
        }
        else
        {
            movementHorizontal = 0;
        }

        if (Position != null)
            Position(transform.position);

        if (movement.magnitude > 0)
        {
            targetAngle = Mathf.Atan2(-movement.x, movement.y) * Mathf.Rad2Deg;

        }
        transform.eulerAngles = new Vector3(0, 0, Mathf.LerpAngle
            (
                transform.eulerAngles.z, targetAngle, turnSpeed * Time.deltaTime
            ));
    }

    private void FixedUpdate()
    {
        movement = new Vector2(movementHorizontal, movementVerticle);
        rb.AddForce(currentAcceleration * movement.normalized);
        if (PlayerVelocity != null)
        {
            PlayerVelocity(rb.velocity);
        }
    }

    public void AddAcceleration(float add) => currentAcceleration += add;
    public void ResetAcceleration() => currentAcceleration = defaultAcceleration;
}

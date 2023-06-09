using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour, IContainsInput
{
    public static event System.Action<Vector2> PlayerVelocity;

    [SerializeField] float defaultAcceleration;
    float currentAcceleration;
    Vector2 movement;
    Vector3 rotation;

    [SerializeField] float turnSpeed;
    float targetAngle;

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ResetAcceleration();
    }

    public void Move(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        rb.AddForce(currentAcceleration * movement.normalized);
        if (PlayerVelocity != null)
        {
            PlayerVelocity(rb.velocity);
        }

        //rotate towards where the player is moving
        if (movement.magnitude > 0)
        {
            targetAngle = Mathf.Atan2(-movement.x, movement.y) * Mathf.Rad2Deg;
        }
        //always finish rotation
        rotation.z = Mathf.LerpAngle(rotation.z, targetAngle, turnSpeed * Time.fixedDeltaTime);
        transform.eulerAngles = rotation;
    }

    public void AddAcceleration(float add) => currentAcceleration += add;
    public void ResetAcceleration() => currentAcceleration = defaultAcceleration;
}

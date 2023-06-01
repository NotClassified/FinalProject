using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public static event System.Action<GameObject> AsteroidHit;

    Rigidbody2D rb;
    Asteroid collidedAsteroid;

    [SerializeField] float slowDownDuration;
    [SerializeField] float dragIncrease;
    float dragTimer;
    Coroutine slowerSpeedRoutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Asteroid>(out collidedAsteroid))
        {
            if (slowerSpeedRoutine == null)
            {
                //don't have multiple SlowDownSpeed routines
                slowerSpeedRoutine = StartCoroutine(SlowDownSpeed());
            }
            dragTimer = slowDownDuration;
            if (AsteroidHit != null)
            {
                AsteroidHit(collision.gameObject);
            }
        }
    }

    IEnumerator SlowDownSpeed()
    {
        float initialDrag = rb.drag;
        rb.drag += dragIncrease;
        while (dragTimer > 0)
        {
            dragTimer -= Time.deltaTime;
            yield return null;
        }
        rb.drag = initialDrag;
        slowerSpeedRoutine = null;
    }
}

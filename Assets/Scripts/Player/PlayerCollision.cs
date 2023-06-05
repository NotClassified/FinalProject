using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public static event System.Action<GameObject> AsteroidHit;
    public static event System.Action<GameObject> ItemPickedup;

    Rigidbody2D rb;
    [SerializeField] float dragIncrease;
    [SerializeField] float dragReleaseDuration;
    Coroutine slowerSpeedRoutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Asteroid>() != null)
        {
            if (slowerSpeedRoutine != null)
                //don't have multiple SlowDownSpeed routines
                StopCoroutine(slowerSpeedRoutine);
            slowerSpeedRoutine = StartCoroutine(SlowDownSpeed());

            if (AsteroidHit != null)
                AsteroidHit(collision.gameObject);
        }
        else if(collision.GetComponent<Item>() != null)
        {
            if (ItemPickedup != null)
                ItemPickedup(collision.transform.gameObject);
        }
    }

    IEnumerator SlowDownSpeed()
    {
        float initialDrag = rb.drag;
        float newDrag = initialDrag + dragIncrease;

        float timer = 0;
        while (timer < dragReleaseDuration)
        {
            rb.drag = Mathf.Lerp(newDrag, initialDrag, timer / dragReleaseDuration);

            timer += Time.deltaTime;
            yield return null;
        }
        rb.drag = initialDrag;
        slowerSpeedRoutine = null;
    }
}

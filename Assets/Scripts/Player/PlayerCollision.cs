using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] float asteroidIgnore_CoolDown;
    float asteroidIgnore_Timer;
    public static event System.Action<GameObject> AsteroidHit;
    public static event System.Action<GameObject> ItemPickedup;

    Rigidbody2D rb;
    float initialDrag;
    [SerializeField] float dragIncrease;
    [SerializeField] float dragReleaseDuration;
    Coroutine slowerSpeedRoutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        initialDrag = rb.drag;
        PlayerItems.ItemUsed += ItemEffectsOnCollision;
    }
    private void OnDestroy()
    {
        PlayerItems.ItemUsed -= ItemEffectsOnCollision;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Asteroid>() != null)
        {
            if (slowerSpeedRoutine != null)
                //don't have multiple SlowDownSpeed routines
                StopCoroutine(slowerSpeedRoutine);
            if (asteroidIgnore_Timer <= 0)
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

    private void Update()
    {
        if (asteroidIgnore_Timer > 0)
        {
            asteroidIgnore_Timer -= Time.deltaTime;
        }
    }

    IEnumerator SlowDownSpeed()
    {
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

    void ItemEffectsOnCollision(Item item)
    {
        //ignore asteroids for the speed of light boost
        if (item is SpeedOfLightBoost)
        {
            asteroidIgnore_Timer = asteroidIgnore_CoolDown;
        }
    }
}

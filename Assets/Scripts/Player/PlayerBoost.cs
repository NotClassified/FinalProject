using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBoost : MonoBehaviour, IContainsInput
{
    public event System.Action<float> BoostInitiated;
    public event System.Action BoostFinished;

    [Header("Boost Parameters")]
    [SerializeField] float minBoostForce0Percent;
    [SerializeField] float maxBoostForce100Precent;
    [SerializeField] float maxDistanceFor0Percent;
    [SerializeField] float minDistanceFor100Percent;
    [SerializeField] float boostDuration;
    [SerializeField] float dragIncrease;
    float initialDrag;
    float distanceToBooster;
    float percentBoost;

    [SerializeField] Vector2 castSize;
    [SerializeField] ContactFilter2D contactFilter;
    RaycastHit2D lastHit;

    [Header("Spam Prevention")]
    [SerializeField] float attemptCoolDown;
    float attemptTimer;

    //[Header("Trail Effect")]
    //[SerializeField] float trailAlpha;
    //[SerializeField] float trailAlphaLerpSpeed;
    //[SerializeField] float minTraillength;
    //[SerializeField] float maxTraillength;
    //float targetTrailAlpha;
    //TrailRenderer boostTrail;

    Rigidbody2D rb;
    Coroutine forwardBoostRoutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        initialDrag = rb.drag;

        //boostTrail = GetComponent<TrailRenderer>();
        //targetTrailAlpha = trailAlpha;
        //SetTrailAlpha(targetTrailAlpha);
        //boostTrail.enabled = false;

        PlayerCollision.AsteroidHit += StopBoostWhenAsteroidHit;
    }

    private void OnDestroy()
    {
        PlayerCollision.AsteroidHit -= StopBoostWhenAsteroidHit;
    }

    private void Update()
    {
        if (attemptTimer > 0)
        {
            attemptTimer -= Time.deltaTime;
        }
        //if (boostTrail.material.color.a < targetTrailAlpha - .05f || boostTrail.material.color.a > targetTrailAlpha + .05f)
        //{
        //    SetTrailAlpha(Mathf.Lerp(boostTrail.material.color.a, targetTrailAlpha, Time.deltaTime * trailAlphaLerpSpeed));
        //}
        //else if (boostTrail.material.color.a != targetTrailAlpha)
        //    SetTrailAlpha(targetTrailAlpha);

        //if (boostTrail.enabled && boostTrail.material.color.a == 0)
        //{
        //    boostTrail.enabled = false;
        //}
    }

    public void AttemptBoost(InputAction.CallbackContext context)
    {
        if (context.performed && attemptTimer <= 0)
        {
            attemptTimer = attemptCoolDown;

            RaycastHit2D[] boosterHit = new RaycastHit2D[1];
            if (Physics2D.BoxCast(transform.position, castSize, transform.eulerAngles.z,
                 Vector3.zero, contactFilter, boosterHit) > 0)
            {
                //distance between player and booster to determine extra speed amount
                distanceToBooster = Vector2.Distance(transform.position,
                                                      boosterHit[0].collider.ClosestPoint(transform.position));
                distanceToBooster = Mathf.Clamp(distanceToBooster, minDistanceFor100Percent, maxDistanceFor0Percent);

                //shorter distance to booster = more force
                percentBoost = Mathf.Abs(FindPercentageOfAValueBetweenTwoNumbers(distanceToBooster,
                                                                                  maxDistanceFor0Percent,
                                                                                    minDistanceFor100Percent));
                float force = Mathf.Lerp(minBoostForce0Percent, maxBoostForce100Precent, percentBoost);
                ////more force = longer trail effect
                //boostTrail.time = Mathf.Lerp(minTraillength, maxTraillength, percentBoost);
                if (BoostInitiated != null)
                    BoostInitiated(percentBoost);

                ForceStopForwardBoostRoutine(); //prevent multiple routines
                forwardBoostRoutine = StartCoroutine(ForwardBoost(force));
            }
        }
    }

    IEnumerator ForwardBoost(float force)
    {
        rb.drag = initialDrag + dragIncrease;

        //if (!boostTrail.enabled)
        //{
        //    boostTrail.enabled = true;
        //    targetTrailAlpha = trailAlpha;
        //    SetTrailAlpha(targetTrailAlpha);
        //}
        //else
        //    targetTrailAlpha = trailAlpha;

        float timer = 0;
        while (timer < boostDuration)
        {
            rb.AddForce(transform.up * force);
            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        rb.drag = initialDrag;

        //targetTrailAlpha = 0f;
        if (BoostFinished != null)
            BoostFinished();
    }

    void ForceStopForwardBoostRoutine()
    {
        if (forwardBoostRoutine != null)
        {
            StopCoroutine(forwardBoostRoutine);
        }
    }
    void StopBoostWhenAsteroidHit(GameObject asteroid)
    {
        ForceStopForwardBoostRoutine();
        //targetTrailAlpha = 0f;
    }

    //void SetTrailAlpha(float alpha)
    //{
    //    var newAlpha = boostTrail.material.color;
    //    newAlpha.a = alpha;
    //    boostTrail.material.color = newAlpha;
    //}

    public static float FindPercentageOfAValueBetweenTwoNumbers(float value, float a, float b)
    {
        return (value - a) / (b - a);
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawCube(transform.position, castSize);

        Gizmos.color = Color.red;
        if (lastHit.collider != null)
        {
            Gizmos.DrawLine(transform.position, lastHit.collider.ClosestPoint(transform.position));
        }
    }
}

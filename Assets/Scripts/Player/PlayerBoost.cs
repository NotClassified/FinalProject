using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoost : MonoBehaviour
{
    [Header("Boost Parameters")]
    [SerializeField] float minBoostForce0Percent;
    [SerializeField] float maxBoostForce100Precent;
    [SerializeField] float maxDistanceFor0Percent;
    [SerializeField] float minDistanceFor100Percent;
    float distanceToBooster;
    float percentBoost;

    [SerializeField] Vector2 castSize;
    [SerializeField] ContactFilter2D contactFilter;
    RaycastHit2D lastHit;

    [Header("Spam Prevention")]
    [SerializeField] float attemptCoolDown;
    float attemptTimer;

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && attemptTimer <= 0)
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

                rb.AddForce(transform.up * force, ForceMode2D.Impulse);
            }
        }

        if (attemptTimer > 0)
        {
            attemptTimer -= Time.deltaTime;
        }
    }

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

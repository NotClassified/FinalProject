using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoost : MonoBehaviour
{
    [Header("Boost Parameters")]
    [SerializeField] float minBoostForce;
    [SerializeField] float maxBoostForce;
    [SerializeField] float minDistanceFor0Percent;
    [SerializeField] float maxDistanceFor100Percent;
    float distanceToBooster;
    float percentBoost;

    [SerializeField] Vector2 castSize;
    [SerializeField] ContactFilter2D contactFilter;
    RaycastHit2D[] boosterHit = new RaycastHit2D[1];

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

            if (Physics2D.BoxCast(transform.position, castSize, 0, transform.up, contactFilter, boosterHit) > 0)
            {
                distanceToBooster = Vector2.Distance(transform.position, 
                                                      boosterHit[0].collider.ClosestPoint(transform.position));
                print(distanceToBooster);
                distanceToBooster = Mathf.Clamp(distanceToBooster, minDistanceFor0Percent, maxDistanceFor100Percent);

                percentBoost = Mathf.Abs(FindPercentageOfAValueBetweenTwoNumbers(distanceToBooster,
                                                                                  maxDistanceFor100Percent,
                                                                                    minDistanceFor0Percent));
                //shorter distance to booster = more force
                float force = Mathf.Lerp(minBoostForce, maxBoostForce, percentBoost);

                rb.AddForce(transform.up * force, ForceMode2D.Impulse);
                print("%" + percentBoost * 100);
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidGun : Item
{
    [SerializeField] float radius;
    [SerializeField] ContactFilter2D filter;

    AsteroidManager asteroidManager;

    private void Awake()
    {
        asteroidManager = FindObjectOfType<AsteroidManager>();
    }

    public override void Use(GameObject player)
    {
        base.Use(player);

        Collider2D[] results = new Collider2D[5];
        Physics2D.OverlapCircle(player.transform.position, radius, filter, results);
        foreach (Collider2D col in results)
        {
            if (col == null)
                break; //no more results left

            if (col.GetComponent<Asteroid>() != null)
            {
                asteroidManager.ReleaseAsteroid(col.gameObject);
            }
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidManager : MonoBehaviour
{
    private AsteroidPooler _pooler;
    private AsteroidPooler Pooler
    {
        get
        {
            if (_pooler == null)
                _pooler = GetComponent<AsteroidPooler>();
            return _pooler;
        }
    }

    [Header("Managing Instances")]
    private List<GameObject> currentAsteroids = new List<GameObject>();
    [SerializeField] int startAmount;
    [SerializeField] int maxAmount;
    [SerializeField] int spawnBunchAmount;
    [SerializeField] float spawnFrequency;
    [SerializeField] float checkOutOfBoundsFrequency;

    [Header("Instance Parameters")]
    [SerializeField] float speed;
    [SerializeField] Vector2 direction;
    [SerializeField] float minSize;
    [SerializeField] float maxSize;
    [SerializeField] ScreenSides[] spawnSides;

    private void Awake()
    {
        PlayerCollision.AsteroidHit += ReleaseAsteroid;
    }
    private void OnDestroy()
    {
        PlayerCollision.AsteroidHit -= ReleaseAsteroid;
    }

    private IEnumerator Start()
    {
        SpawnAsteroids(startAmount);

        StartCoroutine(CheckOutOfBounds());
        while (true)
        {
            while (currentAsteroids.Count < maxAmount)
            {
                yield return new WaitForSeconds(spawnFrequency);
                SpawnAsteroids(spawnBunchAmount);
            }
            yield return null;
        }
    }

    private void SpawnAsteroids(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject asteroid = Pooler.GetInstance();
            Vector2 spawn = LevelAssetData.instance.GetRandomBoundaryPosition(spawnSides);
            asteroid.transform.position = spawn;
            asteroid.transform.localScale = Vector3.one * Random.Range(minSize, maxSize);

            Asteroid script = asteroid.GetComponent<Asteroid>();
            script.speed = this.speed;
            script.direction = this.direction;

            currentAsteroids.Add(asteroid);
            if (currentAsteroids.Count >= maxAmount)
                break;
        }
    }

    IEnumerator CheckOutOfBounds()
    {
        while (true)
        {
            yield return new WaitForSeconds(checkOutOfBoundsFrequency);
            print(true);
            foreach (GameObject asteroid in currentAsteroids)
            {
                //release asteroids that are out of bounds
                if (!LevelAssetData.instance.IsInLevelBounds(asteroid.transform.position))
                    ReleaseAsteroid(asteroid);
            }
        }
    }

    void ReleaseAsteroid(GameObject asteroid)
    {
        currentAsteroids.Remove(asteroid);
        Pooler.ReleaseInstance(asteroid);
    }
}

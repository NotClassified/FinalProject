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
    [SerializeField] float spaceBetweenAsteroids;
    [SerializeField] float checkOutOfBoundsFrequency;

    [Header("Instance Parameters")]
    [SerializeField] float speed;
    [SerializeField] Vector2 direction;
    [SerializeField] float minSize;
    [SerializeField] float maxSize;
    [SerializeField] ScreenSides[] spawnSides;

    LevelManager.PositionOnScreenSide posMethod;

    private void Awake()
    {
        posMethod =LevelManager.instance.GetRandomBoundaryPosition;

        PlayerCollision.AsteroidHit += ReleaseAsteroid;
    }
    private void OnDestroy()
    {
        PlayerCollision.AsteroidHit -= ReleaseAsteroid;
    }

    private IEnumerator Start()
    {
        for (int i = 0; i < startAmount; i++)
        {
            SpawnAsteroids();
            yield return new WaitForFixedUpdate();
        }

        StartCoroutine(CheckOutOfBounds());
        while (true)
        {
            while (currentAsteroids.Count < maxAmount)
            {
                yield return new WaitForSeconds(spawnFrequency);
                for (int i = 0; i < spawnBunchAmount && currentAsteroids.Count < maxAmount; i++)
                {
                    SpawnAsteroids();
                    yield return new WaitForFixedUpdate();
                }
            }
            yield return null;
        }
    }

    private void SpawnAsteroids()
    {
        GameObject asteroid = Pooler.GetInstance();
        Vector2 spawn = LevelManager.instance.FindAreaWithoutC<Asteroid>(posMethod, spawnSides, spaceBetweenAsteroids, true);

        asteroid.transform.position = spawn;
        asteroid.transform.localScale = Vector3.one * Random.Range(minSize, maxSize);

        Asteroid script = asteroid.GetComponent<Asteroid>();
        script.speed = this.speed;
        script.direction = this.direction;

        currentAsteroids.Add(asteroid);
    }

    IEnumerator CheckOutOfBounds()
    {
        while (true)
        {
            yield return new WaitForSeconds(checkOutOfBoundsFrequency);
            for (int i = 0; i < currentAsteroids.Count; i++)
            {
                //release asteroids that are out of bounds
                if (!LevelManager.instance.IsInLevelBounds(currentAsteroids[i].transform.position))
                    ReleaseAsteroid(currentAsteroids[i]);
            }
        }
    }

    public void ReleaseAsteroid(GameObject asteroid)
    {
        currentAsteroids.Remove(asteroid);
        Pooler.ReleaseInstance(asteroid);
    }
}

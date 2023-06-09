using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostManager : MonoBehaviour
{
    private BoostPooler _pooler;
    private BoostPooler Pooler
    {
        get
        {
            if (_pooler == null)
                _pooler = GetComponent<BoostPooler>();
            return _pooler;
        }
    }

    private int boosterCount;
    [SerializeField] int startAmount;
    [SerializeField] int maxAmount;
    [SerializeField] float boostLifetime;
    [SerializeField] float spawnFrequency;
    [SerializeField] float minSpaceBetweenBoosters;
    Coroutine boosterSpawnRoutine;

    LevelManager.PositionMethod posMethod;

    private void Awake()
    {
        GameManager.GameOver += delegate { StopCoroutine(boosterSpawnRoutine); };
    }
    private void OnDestroy()
    {
        GameManager.GameOver -= delegate { StopCoroutine(boosterSpawnRoutine); };
    }

    private void Start()
    {
        posMethod = LevelManager.instance.GetRandomAssetPosition;

        boosterSpawnRoutine = StartCoroutine(SpawnBoostersRoutine());
    }

    IEnumerator SpawnBoostersRoutine()
    {

        for (int i = 0; i < startAmount; i++)
        {
            SpawnBoosterInstance();
            yield return new WaitForEndOfFrame();
        }

        while (true)
        {
            while (boosterCount < maxAmount)
            {
                yield return new WaitForSeconds(spawnFrequency);
                SpawnBoosterInstance();
            }
            yield return null;
        }
    }

    private void SpawnBoosterInstance()
    {
        GameObject booster = Pooler.GetInstance();
        Vector2 spawn = LevelManager.instance.FindAreaWithoutC<Booster>(posMethod, minSpaceBetweenBoosters, true);
        booster.transform.position = spawn;

        Booster script = booster.GetComponent<Booster>();
        script.Lifetime = boostLifetime;

        script.TimerFinished += InstanceReleased;
        boosterCount++;
    }

    private void InstanceReleased(GameObject booster)
    {
        booster.GetComponent<Booster>().TimerFinished -= InstanceReleased;
        boosterCount--;
    }
}

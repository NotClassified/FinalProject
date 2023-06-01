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

    private IEnumerator Start()
    {
        for (int i = 0; i < startAmount; i++)
        {
            SpawnBooster();
        }

        while (true)
        {
            while (boosterCount < maxAmount)
            {
                yield return new WaitForSeconds(spawnFrequency);
                SpawnBooster();
            }
            yield return null;
        }
    }

    private void SpawnBooster()
    {
        GameObject booster = Pooler.GetInstance();
        Vector2 spawn = LevelAssetData.instance.GetRandomAssetPosition(true);
        booster.transform.position = spawn;

        Booster script = booster.GetComponent<Booster>();
        script.Lifetime = boostLifetime;
        script.timerOn = boostLifetime != 0;

        script.TimerFinished += InstanceReleased;
        boosterCount++;
    }

    private void InstanceReleased(GameObject booster)
    {
        booster.GetComponent<Booster>().TimerFinished -= InstanceReleased;
        boosterCount--;
    }
}

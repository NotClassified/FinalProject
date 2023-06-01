using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidPooler : MonoBehaviour
{
    [SerializeField] GameObject asteroidPrefab;
    private List<GameObject> asteroidInstances = new List<GameObject>();
    [SerializeField] int startSize;

    private void Start()
    {
        for (int i = 0; i < startSize; i++)
        {
            CreateNewInstance().SetActive(false);
        }
    }

    private GameObject CreateNewInstance()
    {
        GameObject newInstance = Instantiate(asteroidPrefab, transform);

        asteroidInstances.Add(newInstance);
        return newInstance;
    }

    public GameObject GetInstance()
    {
        GameObject newInstance = null;
        foreach (GameObject instance in asteroidInstances)
        {
            if (!instance.activeSelf)
            {
                newInstance = instance;
                newInstance.SetActive(true);
                break;
            }
        }
        if (newInstance == null) //all instances are in use
            newInstance = CreateNewInstance();

        return newInstance;
    }

    public void ReleaseInstance(GameObject usedInstance)
    {
        usedInstance.SetActive(false);
        usedInstance.transform.position = Vector3.zero;
        usedInstance.transform.localScale = Vector3.zero;
    }
}

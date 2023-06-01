using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostPooler : MonoBehaviour
{
    //public static BoostPooler instance;
    [SerializeField] GameObject boostPrefab;
    private List<GameObject> boostInstances = new List<GameObject>();
    [SerializeField] int startSize;

    //private void Awake()
    //{
    //    instance = this;
    //}

    private void Start()
    {
        for (int i = 0; i < startSize; i++)
        {
            CreateNewInstance().SetActive(false);
        }
    }

    private GameObject CreateNewInstance()
    {
        GameObject newInstance = Instantiate(boostPrefab, transform);
        newInstance.GetComponent<Booster>().TimerFinished += ReleaseInstance;

        boostInstances.Add(newInstance);
        return newInstance;
    }

    public GameObject GetInstance()
    {
        GameObject newInstance = null;
        foreach (GameObject instance in boostInstances)
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
        //usedInstance.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        usedInstance.transform.position = Vector3.zero;
        usedInstance.GetComponent<Booster>().TimerFinished -= ReleaseInstance;
    }
}

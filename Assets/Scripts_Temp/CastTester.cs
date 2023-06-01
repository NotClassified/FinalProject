using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastTester : MonoBehaviour
{
    public GameObject prefab;
    public Bounds bounds;
    [Min(.1f)] public float spawnIncrement;

    List<GameObject> objects = new List<GameObject>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Clear();
            SpawnGrid();
        }
    }

    void Clear()
    {
        foreach (GameObject obj in objects)
        {
            Destroy(obj);
        }
        objects.Clear();
    }

    void SpawnGrid()
    {
        float x = bounds.min.x;
        while (x <= bounds.max.x)
        {
            float y = bounds.min.y;
            while (y <= bounds.max.y)
            {
                Instantiate(prefab, new Vector2(x, y), Quaternion.identity);
                y += spawnIncrement;
            }
            x += spawnIncrement;

        }
    }
}

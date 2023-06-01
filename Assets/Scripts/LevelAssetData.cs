using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelAssetData : MonoBehaviour
{
    public static LevelAssetData instance;

    public List<Bounds> assetBounds = new List<Bounds>();
    public List<int> filledAssets = new List<int>();
    [SerializeField] Vector2 assetSize;

    private void Awake()
    {
        instance = this;

        foreach (Transform child in transform)
        {
            if (child.name.Contains("Straight"))
            {
                Vector3 boundSize = assetSize;
                if (child.localEulerAngles.z != 0)
                {
                    var temp = boundSize.x;
                    boundSize.x = boundSize.y;
                    boundSize.y = temp;
                }

                assetBounds.Add(new Bounds(child.position, boundSize));
            }
        }
    }

    public Vector3 GetRandomPosition(bool emptyTile)
    {
        int rand;
        if (emptyTile)
        {
            do
            {
                if (filledAssets.Count == assetBounds.Count)
                {
                    Debug.LogWarning("no empty tiles");
                    return Vector3.zero;
                }

                rand = Random.Range(0, assetBounds.Count);
            } while (filledAssets.Contains(rand));

            filledAssets.Add(rand);
            return GetRandomBoundPosition(assetBounds[rand]);
        }
        else
        {
            rand = Random.Range(0, assetBounds.Count);
            filledAssets.Add(rand);
            return GetRandomBoundPosition(assetBounds[rand]);
        }
    }

    private Vector3 GetRandomBoundPosition(Bounds bounds)
    {
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);
        return new Vector3(x, y, 0f);
    }

    //private void OnDrawGizmos()
    //{
    //    foreach (Bounds asset in assetBounds)
    //    {
    //        Gizmos.DrawCube(asset.center, asset.size);
    //    }
    //}
}

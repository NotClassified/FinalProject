using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum ScreenSides
{
    Left, Right, Top, Bottom,
}

public class LevelAssetData : MonoBehaviour
{
    public static LevelAssetData instance;

    public List<Bounds> assetBounds = new List<Bounds>();
    public List<int> filledAssets = new List<int>();
    [SerializeField] Vector2 assetSize;
    [SerializeField] Bounds levelBounds;

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

    public Vector2 GetRandomBoundaryPosition(ScreenSides[] screenSides)
    {
        //choose random side of the sides given
        ScreenSides side = screenSides[Random.Range(0, screenSides.Length)];

        float x = 0;
        float y = 0;
        switch (side)
        {
            case ScreenSides.Left:
                x = levelBounds.min.x;
                y = Random.Range(levelBounds.min.y, levelBounds.max.y);
                break;
            case ScreenSides.Right:
                x = levelBounds.max.x;
                y = Random.Range(levelBounds.min.y, levelBounds.max.y);
                break;
            case ScreenSides.Top:
                x = Random.Range(levelBounds.min.x, levelBounds.max.x);
                y = levelBounds.max.y;
                break;
            case ScreenSides.Bottom:
                x = Random.Range(levelBounds.min.x, levelBounds.max.x);
                y = levelBounds.min.y;
                break;
            default:
                Debug.LogError("couldn't find side");
                break;
        }
        return new Vector2(x, y);
    }

    public Vector2 GetRandomAssetPosition(bool emptyTile, bool fillTile)
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

            if (fillTile)
                filledAssets.Add(rand);
            return GetRandomBoundPosition(assetBounds[rand]);
        }
        else
        {
            rand = Random.Range(0, assetBounds.Count);
            if (fillTile)
                filledAssets.Add(rand);
            return GetRandomBoundPosition(assetBounds[rand]);
        }
    }

    private Vector2 GetRandomBoundPosition(Bounds bounds)
    {
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);
        return new Vector2(x, y);
    }

    public bool IsInLevelBounds(Vector2 position)
    {
        return levelBounds.Contains(position);
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawCube(levelBounds.center, levelBounds.size);
    }
}

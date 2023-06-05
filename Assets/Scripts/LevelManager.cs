using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public enum ScreenSides
{
    Left, Right, Top, Bottom,
}

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public delegate Vector2 PositionMethod();

    public List<Bounds> assetBounds = new List<Bounds>();
    public List<int> filledAssets = new List<int>();
    [SerializeField] Vector2 assetSize;
    [SerializeField] Bounds levelBounds;
    [SerializeField] ContactFilter2D antiLevelFilter;

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

    public Vector2 GetRandomAssetPosition()
    {
        int rand = Random.Range(0, assetBounds.Count);
        return GetRandomBoundPosition(assetBounds[rand]);
    }

    bool DoesAreaHaveComponent<Component>(Vector2 center, float radius, ContactFilter2D filter)
    {
        Collider2D[] results = new Collider2D[20];
        Physics2D.OverlapCircle(center, radius, filter, results);

        foreach (Collider2D col in results)
        {
            if (col == null)
                return false;
            else
                print(col.gameObject);
            if (col.GetComponent<Component>() != null) //an object with "Component" is within distance
                return true;
        }
        return false;
    }
    public Vector2 FindAreaWithoutC<C>(PositionMethod method, float radius, bool useTriggers)
    {
        //prevent too many loops
        int maxLoops = 100;
        int loopAmount = 0;

        antiLevelFilter.useTriggers = useTriggers;

        Vector2 newPos;
        do
        {
            newPos = method();
        } while (DoesAreaHaveComponent<C>(newPos, radius, antiLevelFilter) && ++loopAmount < maxLoops);
        print(loopAmount);
        return newPos; //no objects with "C" component is within distance
    }

    //public Vector2 GetRandomAssetPosition<AvoidComponent>(float avoidDistance, bool useTriggers)
    //{
    //    //prevent too many loops
    //    int maxLoops = 100;
    //    int loopAmount = 0;

    //    ContactFilter2D filter = new ContactFilter2D();
    //    filter.useTriggers = useTriggers;

    //    Vector2 randPos;
    //    do
    //    {
    //        randPos = GetRandomAssetPosition();
    //    } while (DoesAreaHaveComponent<AvoidComponent>(randPos, avoidDistance, filter) && ++loopAmount < maxLoops);

    //    return randPos; //no objects with "AvoidComponent" is within distance
    //}

    private Vector2 GetRandomBoundPosition(Bounds bounds)
    {
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);
        return new Vector2(x, y);
    }
    //private Vector2 GetRandomBoundPosition<AvoidComponent>(Bounds bounds, float avoidDistance, bool useTriggers)
    //{
    //    //prevent too many loops
    //    int maxLoops = 100;
    //    int loopAmount = 0;

    //    ContactFilter2D filter = new ContactFilter2D();
    //    filter.useTriggers = useTriggers;

    //    Vector2 randPos;
    //    do
    //    {
    //        randPos = GetRandomAssetPosition();
    //    } while (DoesAreaHaveComponent<AvoidComponent>(randPos, avoidDistance, filter) && ++loopAmount < maxLoops);

    //    return randPos; //no objects with "AvoidComponent" is within distance
    //}

    public bool IsInLevelBounds(Vector2 position)
    {
        return levelBounds.Contains(position);
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawCube(levelBounds.center, levelBounds.size);
    }
}
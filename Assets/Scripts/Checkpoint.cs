using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public static event System.Action<int> checkpointHit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerCollision>() != null && checkpointHit != null)
        {
            checkpointHit(transform.GetSiblingIndex());
        }
    }
}

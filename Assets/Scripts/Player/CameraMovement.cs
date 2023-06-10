using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    Transform player;

    [SerializeField] float lerpSpeed;
    Vector3 targetPos;

    private void Start()
    {
        targetPos.z = transform.position.z;
    }

    private void FixedUpdate()
    {
        if (player == null)
        {
            var playerScript = FindObjectOfType<PlayerMovement>();
            if (playerScript != null)
            {
                player = playerScript.transform;
            }
            return;
        }

        targetPos.x = player.position.x;
        targetPos.y = player.position.y;
        transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed);
    }
}

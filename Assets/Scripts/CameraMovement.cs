using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] Transform player;

    [SerializeField] float lerpSpeed;
    Vector3 targetPos;

    private void Start()
    {
        targetPos.z = transform.position.z;
    }

    private void FixedUpdate()
    {
        targetPos.x = player.position.x;
        targetPos.y = player.position.y;
        transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed);
    }
}

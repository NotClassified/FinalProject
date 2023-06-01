using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float speed;
    public Vector3 direction;

    private void Update()
    {
        transform.position += direction.normalized * speed * Time.deltaTime;
    }
}

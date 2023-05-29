using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : MonoBehaviour
{
    public Vector3 center;

    private void Awake()
    {
        center = transform.position; //remove if the center is set a different way
    }
}

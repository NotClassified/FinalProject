using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : MonoBehaviour
{
    public Vector3 center;

    public event System.Action<GameObject> TimerFinished;
    private float timer;
    public bool timerOn;
    private float _lifetime;
    public float Lifetime
    {
        set
        {
            _lifetime = value;
            timer = value;
        }
    }

    private void Awake()
    {
        center = transform.position; //remove if the center is set a different way
    }

    private void Update()
    {
        if (timerOn)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                timerOn = false;
                timer = _lifetime;

                TimerFinished(gameObject);
            }
        }
    }
}

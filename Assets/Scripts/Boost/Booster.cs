using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : MonoBehaviour
{
    public Vector3 center;

    public event System.Action<GameObject> TimerFinished;

    private float timer;
    private bool timerOn;
    private float _lifetime;
    public float Lifetime
    {
        set
        {
            if (value > 0)
            {
                _lifetime = value;
                timer = value;
                timerOn = true;
            }
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
                timer = _lifetime;
                timerOn = false;

                TimerFinished(gameObject);
            }
        }
    }
}

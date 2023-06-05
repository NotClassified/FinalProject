using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] Slider speedBar;

    private void OnEnable()
    {
        PlayerMovement.PlayerVelocity += SpeedUpdate;
    }
    private void OnDisable()
    {
        PlayerMovement.PlayerVelocity -= SpeedUpdate;

    }

    void SpeedUpdate(Vector2 velocity)
    {
        var speed = velocity.magnitude;
        if (speed < speedBar.minValue)
        {
            speedBar.minValue = speed;
        }
        else if (speed > speedBar.maxValue)
        {
            speedBar.maxValue = speed;
        }
        speedBar.value = speed;
    }
}

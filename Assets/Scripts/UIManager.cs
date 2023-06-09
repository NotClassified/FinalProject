using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] Slider speedBar;
    [SerializeField] TextMeshProUGUI timerMilisecond_Text;
    [SerializeField] TextMeshProUGUI timerMinuteAndSecond_Text;

    private void OnEnable()
    {
        PlayerMovement.PlayerVelocity += SpeedUpdate;
        FindObjectOfType<GameManager>().Timelapse += TimeLapseText;
    }

    private void OnDisable()
    {
        PlayerMovement.PlayerVelocity -= SpeedUpdate;
        FindObjectOfType<GameManager>().Timelapse -= TimeLapseText;
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

    private void TimeLapseText(float timer)
    {
        TimeObject timeObj = new TimeObject(timer);
        timerMilisecond_Text.text = timeObj.GetFormat(TimeObject.Format.TwoDigitMilisecond, ':');
        timerMinuteAndSecond_Text.text = timeObj.GetFormat(TimeObject.Format.TwoDigitMinuteAndSecond, ':') + ':';
    }
}

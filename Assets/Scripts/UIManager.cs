using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] Slider speedBar;
    [SerializeField] float speedBarLerp;
    [SerializeField] TextMeshProUGUI itemDisplay_Text;

    [SerializeField] TextMeshProUGUI timerMilisecond_Text;
    [SerializeField] TextMeshProUGUI timerMinuteAndSecond_Text;
    TimeObject gameTimer;

    [SerializeField] TextMeshProUGUI gameOverTimer_Text;
    [SerializeField] GameObject gameOverScreenObj;

    private void OnEnable()
    {
        PlayerMovement.PlayerVelocity += SpeedUpdate;
        PlayerItems.ItemChange += DisplayItem;
        GameManager.Timelapse += TimeLapseText;
        GameManager.GameOver += GameOverScreen;

        gameOverScreenObj.SetActive(false);
    }

    private void OnDisable()
    {
        PlayerMovement.PlayerVelocity -= SpeedUpdate;
        PlayerItems.ItemChange -= DisplayItem;
        GameManager.Timelapse -= TimeLapseText;
        GameManager.GameOver -= GameOverScreen;
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
        speedBar.value = Mathf.Lerp(speedBar.value, speed, Time.deltaTime * speedBarLerp);
    }

    void DisplayItem(Item item)
    {
        if (item == null)
        {
            itemDisplay_Text.text = "";
            return;
        }

        itemDisplay_Text.text = item.GetType().FullName;
    }

    private void TimeLapseText(float timer)
    {
        gameTimer = new TimeObject(timer);
        timerMilisecond_Text.text = gameTimer.GetFormat(TimeObject.Format.TwoDigitMilisecond, ':');
        timerMinuteAndSecond_Text.text = gameTimer.GetFormat(TimeObject.Format.TwoDigitMinSec, ':') + ':';
    }

    private void GameOverScreen()
    {
        gameOverTimer_Text.text = gameTimer.GetFormat(TimeObject.Format.TwoDigitMinSecMili, ':');
        gameOverScreenObj.SetActive(true);
    }
}

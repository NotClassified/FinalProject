using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeObject
{
    float exactTimer;

    int minutes;
    int seconds;
    int miliseconds;

    string minutesAndSecondsFormat;

    public enum Format
    {
        TwoDigitMinSec, TwoDigitMilisecond, TwoDigitMinSecMili
    }

    public TimeObject(float uncovertedTimer)
    {
        exactTimer = uncovertedTimer;

        minutes = (int)exactTimer / 60;
        seconds = (int)exactTimer % 60;
        miliseconds = (int)(exactTimer * 100 % 100);
    }

    public string GetFormat(Format format, char divider)
    {
        string newFormat = "";
        switch (format)
        {
            case Format.TwoDigitMinSec:
                if (minutes < 10)
                    newFormat += "0";
                newFormat += minutes;
                newFormat += divider;

                if (seconds < 10)
                    newFormat += "0";
                newFormat += seconds;
                break;

            case Format.TwoDigitMilisecond:
                if (miliseconds < 10)
                    newFormat += "0";
                newFormat += miliseconds;
                break;

            case Format.TwoDigitMinSecMili:
                newFormat += GetFormat(Format.TwoDigitMinSec, divider);
                newFormat += divider;
                newFormat += GetFormat(Format.TwoDigitMilisecond, divider);
                break;

            default:
                Debug.LogError(format + " hasn't been setup");
                break;
        }
        return newFormat;
    }
}

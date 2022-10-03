using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;
    public enum TimeState {DayTime, NightTime};
    private TimeState currTime = TimeState.DayTime;

    public int day = 1;
    private bool isLastDay = false;

    public float dayHour = 0; // set to 5 for debugging
    private float dayMinute = 0.0f;

    public float nightHour = 0; // set to 5 for debugging
    private float nightMinute = 0.0f;

    private const int maxDay = 6; // one full week cycle
    public float maxHours = 2.0f; // hours it takes to be considered as a Day (2 hours = 1 minute irl)
    public float maxMins = 60.0f; // mins it takes to be considered as an hour

    private float TIME_MULTIPLIER = 60.0f; // --- 60.0f for debugging --- 2.0f normal ---

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        currTime = TimeState.DayTime;
    }

    void Update()
    {
        UpdateTimeState();

        if (currTime == TimeState.DayTime)
        {
            PlayDayTicks();
            UpdateDayTime();
        }

        if (currTime == TimeState.NightTime)
        {
            PlayNightTicks();
            UpdateNightTime();
        }

        if (day >= 6 && !isLastDay)
        {
            GameManager.instance.gameState = GameManager.GameState.Win;
            isLastDay = true;
        }
    }

    private void UpdateTimeState()
    {
        InGameUIManager.instance.UpdateDayLabel("Day " + day);
        if (day >= 5) { InGameUIManager.instance.UpdateDayLabel("Day " + 5); }

        if (dayHour == maxHours && currTime == TimeState.DayTime) // set to night when the hours needed is met
        {
            
            currTime = TimeState.NightTime;
            nightHour = 0;
            nightMinute = 0;
        }

        if (nightHour == maxHours && currTime == TimeState.NightTime) // if total enemy killed == total enemy for a day
        {
            Debug.Log("DAY");
            currTime = TimeState.DayTime;
            dayHour = 0;
            dayMinute = 0;
            day++;
        }
    }

    private void PlayDayTicks()
    {
        dayMinute += Time.deltaTime * TIME_MULTIPLIER; //2f; Note: Use 30f for debugging 
    }

    private void UpdateDayTime()
    {
        if (dayMinute >= maxMins)// increment the hour if minute is greater than max min
        {
            if (!(day == maxDay - 1 && dayHour == maxHours))
            {
                dayHour++; // increment hour
                dayMinute = 0.0f; // reset minutes
            }
            else
            {
                dayMinute = 59.0f;
            }
        }
    }

    private void PlayNightTicks()
    {
        nightMinute += Time.deltaTime * TIME_MULTIPLIER; //2f; Note: Use 30f for debugging 
    }

    private void UpdateNightTime()
    {
        if (nightMinute >= maxMins)// increment the hour if minute is greater than max min
        {
            if (!(day == maxDay - 1 && nightHour == maxHours))
            {
                nightHour++; // increment hour
                nightMinute = 0.0f; // reset minutes
            }
            else
            {
                nightMinute = 59.0f;
            }
        }
    }

    public bool IsNightTime()
    {
        return (currTime == TimeState.NightTime) ? true : false;
    }

    public float MaxHoursTimesMaxMins()
    {
        return maxHours * maxMins;
    }
}

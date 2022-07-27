using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;
    public enum TimeState {DayTime, NightTime};
    private TimeState currTime = TimeState.DayTime;

    public static int day = 1;
    public static float hour = 0; // set to 5 for debugging
    private float minute = 0.0f;

    private const int maxDay = 8; // one full week cycle
    private float maxHours = 6.0f; // hours it takes to be considered as a Day
    private float maxMins = 60.0f; // mins it takes to be considered as an hour

    private float TIME_MULTIPLIER = 60.0f; // 3f for debugging // 2.0f normal

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

    void Update()
    {
        PlayTicks();
        UpdateTime();
        UpdateTimeState();
    }

    private void UpdateTimeState()
    {
        if (hour == maxHours && currTime == TimeState.DayTime) // set to night when the hours needed is met
        {
            Debug.Log("NIGHT");
            currTime = TimeState.NightTime;
        }

        if (EnemySpawningManager.instance.IsEnemyCleared() && currTime == TimeState.NightTime) // if total enemy killed == total enemy for a day
        {
            Debug.Log("DAY");
            currTime = TimeState.DayTime;
            hour = 0;
            minute = 0;
            day++;
        }
    }

    private void UpdateTime()
    {
        if (minute >= maxMins)// increment the hour if minute is greater than max min
        {
            if (!(day == maxDay - 1 && hour == maxHours))
            {
                hour++; // increment hour
                minute = 0.0f; // reset minutes
            }
            else
            {
                minute = 59.0f;
            }
        }

        //Debug.Log("Day " + day + "Time " + hour + " : " + minute);
    }

    private void PlayTicks()
    {
        if (currTime == TimeState.DayTime) 
        {
            minute += Time.deltaTime * TIME_MULTIPLIER; //2f; Note: Use 30f for debugging 
        }
    }

    public bool IsNightTime()
    {
        return (currTime == TimeState.NightTime) ? true : false;
    }
}

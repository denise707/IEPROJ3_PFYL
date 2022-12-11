using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;
    public enum TimeState {DayTime, NightTime};
    private TimeState currTime = TimeState.DayTime;

    public int day = 1;
    private bool isLastDay = false;

    private float dayHour = 0.0f; // set to 5 for debugging
    [SerializeField] float dayMinute = 0.0f; // changed

    public float nightHour = 0.0f; // set to 5 for debugging
    [SerializeField] float nightMinute = 0.0f;

    private const int maxDay = 6; // one full week cycle
    public float maxHours = 2.0f; // hours it takes to be considered as a Day (2 hours = 1 minute irl)
    public float maxMins = 60.0f; // mins it takes to be considered as an hour

    private float TIME_MULTIPLIER = 2.0f; // --- 60.0f for debugging --- 2.0f normal ---

    [Header("Light")]
    [SerializeField] private Light directionalLight;
    [SerializeField] private Color morningColor;
    [SerializeField] private Color nightColor;
    [SerializeField] private GameObject nightPPE;

    [Header("Sound Files")] 
    [SerializeField] private AudioClip dayBGM;
    [SerializeField] private AudioClip nightBGM;
    [SerializeField] private AudioClip dayTransition;
    [SerializeField] private AudioClip nightTransition;

    [Header("Shop")]
    [SerializeField] private GameObject shopEnv;
    [SerializeField] private GameObject store;

    public bool start = true;

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
        morningColor = new Color(0.8588f, 0.8392f, 1);
        nightColor = new Color(0.101f, 0.0682f, 0.24528f);
        AudioManager.instance.ChangeBGMClip(dayBGM);
        AudioManager.instance.PlayBGM();
        //shopEnv.SetActive(true);
        if(store!= null)
        store.SetActive(false);
    }

    void Update()
    {
        if (start)
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
    }

    private void UpdateTimeState()
    {
        InGameUIManager.instance.UpdateDayLabel("Day " + day);

        if (day >= 5) 
        { 
            InGameUIManager.instance.UpdateDayLabel("Day " + 5);
        }

        if (dayHour == maxHours && currTime == TimeState.DayTime) // set to night when the hours needed is met
        {
            currTime = TimeState.NightTime;
            nightHour = 0;
            nightMinute = 0;
            AudioManager.instance.ChangeBGMClip(nightBGM);
            AudioManager.instance.PlaySFX(nightTransition);
            StartCoroutine("StartBGM");
            nightPPE.SetActive(true);
        }

        if (nightHour == maxHours && currTime == TimeState.NightTime) // if total enemy killed == total enemy for a day
        {
            Debug.Log("DAY");
            PlayerData.instance.Heal();
            currTime = TimeState.DayTime;
            dayHour = 0;
            dayMinute = 0;
            day++;
            AudioManager.instance.ChangeBGMClip(dayBGM);
            AudioManager.instance.PlaySFX(dayTransition);
            StartCoroutine("StartBGM");
            GameManager.instance.currentDay = day;
            nightPPE.SetActive(false);
        }
    }

    IEnumerator StartBGM()
    {
        yield return new WaitForSeconds(10.0f);
        AudioManager.instance.PlayBGM();
    }

    private void PlayDayTicks()
    {
        dayMinute += Time.deltaTime * TIME_MULTIPLIER; //2f; Note: Use 30f for debugging 
        LightManager.instance.elapsedTime = dayMinute;
        LightManager.instance.lerpDuration = maxMins;
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
        LightManager.instance.elapsedTime = nightMinute;
        LightManager.instance.lerpDuration = maxMins;
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

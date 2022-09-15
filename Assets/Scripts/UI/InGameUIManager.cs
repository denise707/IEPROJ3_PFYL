using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
    public static InGameUIManager instance;

    private float accumMins = 0.0f;
    private float TIME_MULTIPLIER = 60.0f; // 3f for debugging // 2.0f normal

    [SerializeField] Transform clock;
    [SerializeField] Text dayLabel;

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
        UpdateClock();
    }

    public void Inventory(GameObject popUp)
    {
        if (popUp.activeInHierarchy == false)
        {
            popUp.SetActive(true);
        }

        else
        {
            popUp.SetActive(false);
        }
    }

    public void UpdateClock()
    {
        if (!TimeManager.instance.IsNightTime())
        {
            accumMins += Time.deltaTime * TIME_MULTIPLIER;
            float angle = Mathf.Lerp(0.0f, -180, accumMins / (6 * 60));
            Quaternion target = Quaternion.Euler(0, 0, angle);
            clock.transform.rotation = Quaternion.Slerp(clock.transform.rotation, target, Time.deltaTime * 5.0f);
        }

        else
        {
            if (EnemySpawningManager.instance.totalEnemyKilledInLevel > 0)
            {
                accumMins = 0;
                float angle = Mathf.Lerp(-180, -360, EnemySpawningManager.instance.GetEnemyKilledRatio());
                Quaternion target = Quaternion.Euler(0, 0, angle);
                clock.transform.rotation = Quaternion.Slerp(clock.transform.rotation, target, Time.deltaTime);
            }
        }
    }

    public void UpdateDayLabel(String day)
    {
        this.dayLabel.text = day;
    }
}

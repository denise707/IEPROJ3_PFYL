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

    [Header("HUD Objects")]
    [SerializeField] Transform clock;
    [SerializeField] Text dayLabel;

    [Header("Blocker Panels")]
    [SerializeField] private GameObject blocker1;
    [SerializeField] private GameObject blocker2;

    [Header("Pop Up Panels")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject mainMenuConfirmation;

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

    public void PauseMenu()
    {
        HandlePopUp(pauseMenu, blocker1);
    }

    public void OptionsMenu()
    {
        HandlePopUp(optionsMenu, blocker2);
    }

    public void MainMenuConfirmation()
    {
        HandlePopUp(mainMenuConfirmation, blocker2);
    }

    public void HandlePopUp(GameObject popUp, GameObject blocker)
    {
        if (popUp.activeInHierarchy == false)
        {
            popUp.SetActive(true);
            popUp.GetComponent<PanelOpener>().OpenPanel();
        }

        else
        {
            popUp.GetComponent<PanelOpener>().OpenPanel();
        }

        if (blocker.activeInHierarchy == false)
        {
            BlockerfadeIn(blocker);
        }

        else
        {
            BlockerfadeOut(blocker);
        }
    }


    public void BlockerfadeIn(GameObject obj)
    {
        obj.SetActive(true);
        obj.GetComponent<FadeVFX>().panelState = FadeVFX.PanelState.FadeIn;
    }

    public void BlockerfadeOut(GameObject obj)
    {
        obj.GetComponent<FadeVFX>().panelState = FadeVFX.PanelState.FadeOut;
    }
}

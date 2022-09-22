using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
    public static InGameUIManager instance;

    private float accumMins = 0.0f;

    private float TIME_MULTIPLIER = 2.0f; // --- 60.0f for debugging --- 2.0f normal ---

    [Header("HUD Objects")]
    [SerializeField] Transform clock;
    [SerializeField] Text dayLabel;

    [Header("Player HUD Objects")]
    [SerializeField] Slider hpBar;
    [SerializeField] Text goldLabel;
    [SerializeField] Text bulletCountText;


    [Header("Blocker Panels")]
    [SerializeField] private GameObject blocker1;
    [SerializeField] private GameObject blocker2;

    [Header("Pop Up Panels")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject mainMenuConfirmation;

    private bool resetDay = false;
    private bool resetNight = false;

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
    public void UpdateHP()
    {
        PlayerData.instance.currHP = Mathf.Clamp(PlayerData.instance.currHP, 0.0f, PlayerData.instance.maxHP);

        hpBar.maxValue = PlayerData.instance.maxHP;
        hpBar.value = PlayerData.instance.currHP;
    }

    public void UpdateGold()
    {
        if (PlayerData.instance.GOLD <= 0)
        {
            PlayerData.instance.GOLD = 0;
        }

        goldLabel.text = PlayerData.instance.GOLD.ToString();
    }

    public void UpdateBulletUI ()
    {
        bulletCountText.text = PlayerData.instance.bulletCount.ToString();
    }

    public void UpdateClock()
    {
        if (!TimeManager.instance.IsNightTime())
        {
            if (!resetDay)
            {
                accumMins = 0;
                resetDay = true;
                resetNight = false;
            }

            accumMins += Time.deltaTime * TIME_MULTIPLIER;
            float angle = Mathf.Lerp(0.0f, -180, accumMins / TimeManager.instance.MaxHoursTimesMaxMins());
            Quaternion target = Quaternion.Euler(0, 0, angle);
            clock.transform.rotation = Quaternion.Slerp(clock.transform.rotation, target, Time.deltaTime * TIME_MULTIPLIER);
        }

        else
        {
            if (!resetNight)
            {
                accumMins = 0;
                resetDay = false;
                resetNight = true;
            }

            accumMins += Time.deltaTime * TIME_MULTIPLIER;
            float angle = Mathf.Lerp(-180, -360, accumMins / TimeManager.instance.MaxHoursTimesMaxMins());
            Quaternion target = Quaternion.Euler(0, 0, angle);
            clock.transform.rotation = Quaternion.Slerp(clock.transform.rotation, target, Time.deltaTime * TIME_MULTIPLIER);
        }
    }

    public void UpdateDayLabel(String day)
    {
        this.dayLabel.text = day;
    }

    public void PauseMenu()
    {
        HandlePopUp(pauseMenu, blocker1);
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        HandlePopUp(pauseMenu, blocker1);
        Time.timeScale = 1;
    }

    public void OptionsMenu()
    {
        HandlePopUp(optionsMenu, blocker2);
    }

    public void MainMenuConfirmation()
    {
        HandlePopUp(mainMenuConfirmation, blocker2);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main Menu");
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

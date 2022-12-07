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

    [SerializeField] Slider bulletSlider;
    [SerializeField] Text bulletLabel;


    [Header("Blocker Panels")]
    [SerializeField] private GameObject blocker1;
    [SerializeField] private GameObject blocker2;

    [Header("Pop Up Panels")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject mainMenuConfirmation;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject gameWinScreen;
    [SerializeField] private GameObject restartDayConfirmation;

    [Header("Sound Files")]
    [SerializeField] private AudioClip buttonSFX;
    [SerializeField] private AudioClip mainMenuBGM;

    [Header("Loading Screen")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider loading;

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
        bulletSlider.maxValue = WeaponManager.instance.reload_time;
        bulletSlider.value = WeaponManager.instance.reload_ticks;
    }
    void Update()
    {
        if (TimeManager.instance.start)
        {
            UpdateClock();            
        }
        UpdateBulletSlider();
    }

    private void UpdateBulletSlider()
    {
        if (WeaponManager.instance.IsAtMaxAmmo())
        {
            // display filled slider when ammo at max
            bulletSlider.value = WeaponManager.instance.reload_time;
        }
        else
        {
            bulletSlider.value = WeaponManager.instance.reload_ticks;
        }
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
        bulletLabel.text = WeaponManager.instance.bulletCount.ToString();
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
        if (restartDayConfirmation.activeSelf)
        {
            restartDayConfirmation.SetActive(false);
        }
        HandlePopUp(mainMenuConfirmation, blocker2);
    }

    public void GameOverScreen()
    {
        HandlePopUp(gameOverScreen, blocker1);
        Time.timeScale = 0;
    }

    public void GameWinScreen()
    {
        HandlePopUp(gameWinScreen, blocker1);
        Time.timeScale = 0;
    }

    public void RestartDayConfirmation()
    {
        HandlePopUp(restartDayConfirmation, blocker2);
    }

    public void RestartDay()
    {
        HandlePopUp(restartDayConfirmation, blocker2);

        if (gameOverScreen.activeInHierarchy == true)
        {
           gameOverScreen.SetActive(false);
           BlockerfadeOut(blocker1);
           BlockerfadeOut(blocker2);
        }

        if (gameWinScreen.activeInHierarchy == true)
        {
            gameWinScreen.SetActive(false);
            BlockerfadeOut(blocker1);
            BlockerfadeOut(blocker2);
        }
        loadingScreen.SetActive(true);
        StartCoroutine(LoadSceneAsync(1));
        AudioManager.instance.StopBGM();
        /*SceneManager.LoadScene("Main Level");*/

        Time.timeScale = 1;
    }
    IEnumerator LoadSceneAsync(int index)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(index);
        while (!operation.isDone)
        {
            loading.value = operation.progress;
            Debug.Log(operation.progress);
            yield return null;
        }
    }
    public void ReturnToPause()
    {
        BlockerfadeOut(blocker2);
        if(mainMenuConfirmation.activeSelf)
            mainMenuConfirmation.SetActive(false);
        else
        {
            restartDayConfirmation.SetActive(false);
        }
    }
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1;
        AudioManager.instance.StopBGM();
        AudioManager.instance.ChangeBGMClip(mainMenuBGM);
        SceneManager.LoadScene("Main Menu");
    }

    public void HandlePopUp(GameObject popUp, GameObject blocker)
    {
        //Play SFX
        AudioManager.instance.PlaySFX(buttonSFX);

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

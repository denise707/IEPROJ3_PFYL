using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUIManager : MonoBehaviour
{
    public static MainMenuUIManager instance;

    [Header("Blocker Panels")]
    [SerializeField] private GameObject blocker1;

    [Header("Pop Up Panels")]
    [SerializeField] private GameObject playGameConfirmation;
    [SerializeField] private GameObject playTutorialConfirmation;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject exitGameConfirmation;

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

    public void PlayGameConfimation()
    {
        HandlePopUp(playGameConfirmation, blocker1);
    }
    public void PlayTutorialConfimation()
    {
        HandlePopUp(playTutorialConfirmation, blocker1);
    }

    public void OptionsMenu()
    {
        HandlePopUp(optionsMenu, blocker1);
    }

    public void ExitGameConfirmation()
    {
        HandlePopUp(exitGameConfirmation, blocker1);
    }

    public void OpenMainLevel()
    {
        SceneManager.LoadScene("Level-Test");
    }

    public void OpenTutorialLevel()
    {
        SceneManager.LoadScene("Tutorial Level");
    }

    public void ExitGame()
    {
        Debug.Log("Exit Game");
        Application.Quit();
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

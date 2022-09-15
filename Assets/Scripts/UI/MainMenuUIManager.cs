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
    [SerializeField] private GameObject blocker2;
    [SerializeField] private GameObject blocker3;
    [SerializeField] private GameObject blocker4;

    [Header("Pop Up Panels")]
    [SerializeField] private GameObject newGameConfirmation;
    [SerializeField] private GameObject loadGameConfirmation;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject loadFile;
    [SerializeField] private GameObject exitGameConfirmation;
    [SerializeField] private GameObject createFile;
    [SerializeField] private GameObject overwriteConfirmation;
    [SerializeField] private GameObject deleteConfirmation;
    [SerializeField] private GameObject tutorialConfirmation;
    [SerializeField] private GameObject loadSavedConfirmation;

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

    public void NewGameConfimation()
    {
        HandlePopUp(newGameConfirmation, blocker1);
    }
    public void LoadGameConfimation()
    {
        HandlePopUp(loadGameConfirmation, blocker1);
    }

    public void OptionsMenu()
    {
        HandlePopUp(optionsMenu, blocker1);
    }

    public void ExitGameConfirmation()
    {
        HandlePopUp(exitGameConfirmation, blocker1);
    }

    public void ExitGame()
    {
        Debug.Log("Exit Game");
        Application.Quit();
    }

    public void LoadFile()
    {
        HandlePopUp(loadFile, blocker2);
    }

    public void CreateFile()
    {
        HandlePopUp(createFile, blocker2);
    }

    public void OverwriteConfirmation()
    {
        HandlePopUp(overwriteConfirmation, blocker3);
    }

    public void DeleteConfirmation()
    {
        HandlePopUp(deleteConfirmation, blocker3);
    }

    public void LoadSavedConfirmation()
    {
        HandlePopUp(loadSavedConfirmation, blocker3);
    }

    public void TutorialConfirmation()
    {
        HandlePopUp(tutorialConfirmation, blocker4);
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

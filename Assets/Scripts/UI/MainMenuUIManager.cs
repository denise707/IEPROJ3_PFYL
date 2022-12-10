using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
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

    [Header("Sound Files")] [SerializeField]
    private AudioClip buttonSFX;

    [Header("Loading Screen")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider loading;

    private PlayableDirector narrative;
    [SerializeField] private GameObject narrativeImage;
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
        AudioManager.instance.PlayBGM();
        loadingScreen.SetActive(false);
        narrative = GameObject.FindObjectOfType<PlayableDirector>();
    }
    public void PlayDirector()
    {
        if(narrative!= null)
        {
            narrative.Play();
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

    public void OpenMainLevel(int levelIndex)
    {
        PlayDirector();
        StartCoroutine(WaitForNarrative(levelIndex));

        AudioManager.instance.StopBGM();
    }

    public void OpenTutorialLevel()
    {
        SceneManager.LoadScene("Tutorial Level");
        AudioManager.instance.StopBGM();
    }

    public void ExitGame()
    {
        Debug.Log("Exit Game");
        Application.Quit();
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
    IEnumerator WaitForNarrative(int levelIndex)
    {
        yield return new WaitForSeconds(7.5f);
        narrativeImage.SetActive(false);
        loadingScreen.SetActive(true);
        StartCoroutine(LoadSceneAsync(levelIndex));
        
    }
}

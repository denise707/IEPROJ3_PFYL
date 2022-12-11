using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GamePhase
    {
        Default,
        Mainmenu,
        Tutorial,
        Daytime,
        Nighttime,

    }

    public enum GameState
    {
        Default,
        Win,
        Lose,
    }

    [Header(" Game Phase and States")]
    public GamePhase gamePhase;
    public GameState gameState;

    public static GameManager instance;
    public bool isInventory = false;
    GameObject player;

    [Header("Sound Files")]
    [SerializeField] private AudioClip winSFX;
    [SerializeField] private AudioClip loseSFX;

    [Header(" Nuke Plant")]
    public NukePlantBehavior nukeplant;

    [Header(" Time")]
    public int currentDay = 1;

    [SerializeField] private bool hasGrown = false;




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
        switch (gameState)
        {
            case GameState.Lose: InGameUIManager.instance.GameOverScreen();
                AudioManager.instance.PlaySFX(loseSFX);
                AudioManager.instance.StopBGM();
                gameState = GameState.Default;
                break;
            case GameState.Win:
                InGameUIManager.instance.GameWinScreen();
                AudioManager.instance.PlaySFX(winSFX);
                AudioManager.instance.StopBGM();
                gameState = GameState.Default;
                break;
        }

        //if ((currentDay == 3 || currentDay == 5) && !hasGrown)
        //{
        //    nukeplant.NextPlantPhase();
        //    hasGrown = true;
        //}
        //else if ((currentDay != 3 && currentDay != 5) && hasGrown)
        //{
        //    hasGrown = false;
        //}
    }
}

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
    GameObject player;


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
                gameState = GameState.Default;
                break;
            case GameState.Win:
                InGameUIManager.instance.GameWinScreen();
                gameState = GameState.Default;
                break;
        }
    }
}

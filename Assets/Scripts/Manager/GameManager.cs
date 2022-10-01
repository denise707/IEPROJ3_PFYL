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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerData : MonoBehaviour
{
    public static PlayerData instance;
    // im planning to use this script for storing player data that are needed for the saving mech implementation, but feel free to modify it as much as you can 
    [Header("PLAYER STATS")]
    public  float maxHP = 100;
    public  float currHP = 100;
    public  int GOLD = 100;
    private bool isDead = false;

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
        maxHP = 100;
        currHP = maxHP;
        InGameUIManager.instance.UpdateHP();
        InGameUIManager.instance.UpdateGold();
        InGameUIManager.instance.UpdateBulletUI();


    }

    // Update is called once per frame
    void Update()
    {
       

        if (currHP <= 0 && !isDead)
        {
            GameManager.instance.gameState = GameManager.GameState.Lose;
            isDead = true;
        }
    }

    public void ReceiveGold(int gold)
    {
        this.GOLD += gold;
        InGameUIManager.instance.UpdateGold();
    }
}

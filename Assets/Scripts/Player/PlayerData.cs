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

    [Header("PLAYER GUN STATS")]

    public int maxBulletCount = 10;
    public int bulletCount = 10;
    public float reload_ticks = 0;
    public float reload_time = 3.0f;
    //private bool isReloading = false;

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
        currHP = maxHP;
        InGameUIManager.instance.UpdateHP();
        InGameUIManager.instance.UpdateGold();
        InGameUIManager.instance.UpdateBulletUI();


    }

    // Update is called once per frame
    void Update()
    {
        if(bulletCount < maxBulletCount)
        {
            reload_ticks += Time.deltaTime;

            if (reload_ticks >= reload_time)
            {
                reload_ticks = 0;
                bulletCount++;
                InGameUIManager.instance.UpdateBulletUI();

            }
        }
    }

    public bool IsAtMaxAmmo()
    {
        if (bulletCount >= maxBulletCount)
        {
            return true;
        }
        return false;

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager instance;
    // Start is called before the first frame update

    [Header("PLAYER GUN STATS")]

    public int maxBulletCount = 10;
    public int bulletCount = 10;
    public float reload_ticks = 0;
    public float reload_time = 3.0f;

    public InventoryItemData weaponData;

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

    // Update is called once per frame
    void Update()
    {
        if (bulletCount < maxBulletCount)
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

    public void SetWeaponData(InventoryItemData data )
    {
        this.weaponData = data;
        this.reload_ticks = weaponData.reloadTime;
        this.maxBulletCount = weaponData.maxAmmo;
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

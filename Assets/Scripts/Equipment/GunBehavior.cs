using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBehavior : MonoBehaviour
{
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject bulletPrefab;

    // Start is called before the first frame update
    void Start()
    {
        firePoint = this.gameObject.transform.Find("FirePoint");
        UpdateBullet();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot()
    {
        if (firePoint)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
            {
                GameObject bulletSphere = (GameObject)Instantiate(bulletPrefab, new Vector3(firePoint.position.x, firePoint.position.y, firePoint.position.z), firePoint.rotation);
                //bulletSphere.transform.LookAt(new Vector3(hit.point.x, 2.0f, hit.point.z));
                bulletSphere.transform.rotation = firePoint.transform.rotation;

                WeaponManager.instance.bulletCount -= 1;
                UpdateBullet();
            }
        }
        else
        {
            Debug.Log("cant find fire point");
        }


    }

    void UpdateBullet()
    {
        WeaponManager.instance.bulletCount =  Mathf.Clamp(WeaponManager.instance.bulletCount, 0, WeaponManager.instance.maxBulletCount);
        InGameUIManager.instance.UpdateBulletUI();

        Debug.Log(WeaponManager.instance.bulletCount);
    }
}

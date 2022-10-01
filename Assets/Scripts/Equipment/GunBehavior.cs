using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBehavior : MonoBehaviour
{
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject bulletPrefab;

    private float ticks = 0.0f;
    private const float INTERVAL = 5f;

    //Animator animator;


    // Start is called before the first frame update
    void Start()
    {

        firePoint = this.gameObject.transform.Find("FirePoint");
        //animator = this.gameObject.GetComponent<Animator>();
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

                PlayerData.instance.bulletCount -= 1;
                UpdateBullet();

                //Debug.Log("Spawn Bullet");

            }
            //Debug.Log("Shoot");
        }
        else
        {
            Debug.Log("cant find fire point");
        }


    }

    void UpdateBullet()
    {
        PlayerData.instance.bulletCount =  Mathf.Clamp(PlayerData.instance.bulletCount, 0, PlayerData.instance.maxBulletCount);
        InGameUIManager.instance.UpdateBulletUI();

        Debug.Log(PlayerData.instance.bulletCount);
    }
}

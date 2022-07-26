using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUseTool : MonoBehaviour
{
    [Header ("Shooting")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //firePoint.localPosition = new Vector3(firePoint.localPosition.x, 0.0f, firePoint.localPosition.z);

        if (Input.GetMouseButtonDown(0))
        {
            //ChangeEquippedSprite();
                Shoot();
        }
    }

    void Shoot()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
        {
            GameObject bulletSphere = (GameObject)Instantiate(bulletPrefab, new Vector3(firePoint.position.x, firePoint.position.y, firePoint.position.z ), firePoint.rotation);
            bulletSphere.transform.LookAt(new Vector3(hit.point.x, hit.point.y, hit.point.z));
            Debug.Log("Spawn Bullet");

        }
        Debug.Log("Shoot");

    }
}

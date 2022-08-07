using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponAim : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    //[SerializeField] private Transform aimTransform;

   

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // get mouse pos

        Aim();
    }

    private void Aim()
    {
        /*
        // Tool Aim
        Vector3 difference = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        difference.Normalize();

        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(rotationZ, Vector3.forward);

        Debug.Log(rotationZ);

        if (rotationZ < -90 || rotationZ > 90)
        {
            if (Player.transform.eulerAngles.y == 0)
            {
                transform.localRotation = Quaternion.Euler(180, 0, -rotationZ);
            }
        }
        else
        {
            transform.localRotation = Quaternion.Euler(0, 0, rotationZ);
        }

        */

        // Gun aim
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            transform.LookAt(hit.point); // Look at the point
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0)); // Clamp the x and z rotation
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator))]
public class EquipmentFunction: MonoBehaviour
{

    private enum EquipmentType
    {
        Tool,
        Weapon
    };

    EquipmentType type;
    Animator animator;

    [Header ("Shooting")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    


    // Start is called before the first frame update
    void Start()
    {
        animator = this.gameObject.GetComponent<Animator>();

        // identify the equipment type
        if (this.gameObject.CompareTag("Tool"))
        {
            type = EquipmentType.Tool;
        }
        else if (this.gameObject.CompareTag("Weapon"))
        {
            type = EquipmentType.Weapon;
        }


        firePoint = this.gameObject.transform.Find("FirePoint");
    }

    // Update is called once per frame
    void Update()
    {
        //firePoint.localPosition = new Vector3(firePoint.localPosition.x, 0.0f, firePoint.localPosition.z);

        // execute tool function based on type and name
        if (Input.GetMouseButtonDown(0))
        {
            RunEquipmentFunc();
        }
    }

    #region Equipment Checker
    void RunEquipmentFunc()
    {
        if (type == EquipmentType.Weapon) // weapons
        {
            if (CompareName("Pistol"))
            {
                TriggerPistol(); // function
            }
            else if (CompareName("Machete"))
            {
                // func
            }
            else
            {
                Debug.Log($"Weapon name not found: {this.gameObject.name}");
            }
        }
        else if (type == EquipmentType.Tool) // tools
        {
            if (CompareName("Hoe"))
            {
                // func
                TriggerHoe();
            }
            else if (CompareName("Watering Can"))
            {
                // func
                TriggerWateringCan();
            }
            else
            {
                Debug.Log($"Tool name not found: {this.gameObject.name}" );
            }
        }
        else
        {
            Debug.Log($"Type not found: {this.type}");
        }
    }
    #endregion

    #region Equipment Trigger Functions 
    void TriggerPistol()
    {

        if (firePoint)
        {
            animator.SetTrigger("isTriggered");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
            {
                GameObject bulletSphere = (GameObject)Instantiate(bulletPrefab, new Vector3(firePoint.position.x, firePoint.position.y, firePoint.position.z), firePoint.rotation);
                bulletSphere.transform.LookAt(new Vector3(hit.point.x, hit.point.y, hit.point.z));
                Debug.Log("Spawn Bullet");

            }
            Debug.Log("Shoot");
        }
        else
        {
            Debug.Log("cant find fire point");
        }
       

    }

    void TriggerHoe()
    {
        animator.SetTrigger("isTriggered");
    }

    void TriggerWateringCan()
    {
        animator.SetTrigger("isTriggered");
    }

    #endregion

    #region Utility Functions
    bool CompareName(string name)
    {
        if (this.gameObject.name == name)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion
}

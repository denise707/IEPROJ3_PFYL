using UnityEngine;


[RequireComponent(typeof(Animator))]
public class EquipmentFunction: MonoBehaviour
{

    private enum EquipmentType
    {
        Tool,
        Weapon,
        Plant
    };

    EquipmentType type;
    Animator animator;

    [Header("Temp Seed")]
    [SerializeField] PlantObject plant;

    public Soil soil;



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
        else if (this.gameObject.CompareTag("Plant"))
        {
            type = EquipmentType.Plant;
        }


        //firePoint = this.gameObject.transform.Find("FirePoint");
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
                GetSoil();
                // trigger Hoe
                animator.SetTrigger("isTriggered");
                // trigger hoe func in end of anim
            }
            else if (CompareName("Watering Can"))
            {
                GetSoil();
                // func
                animator.SetTrigger("isTriggered");
                // trigger hoe func in end of anim
            }
            else
            {
                Debug.Log($"Tool name not found: {this.gameObject.name}");
            }
        }
        else if (type == EquipmentType.Plant) // tools
        {
            if (CompareName("Rose"))
            {
                GetSoil();
                TriggerPlanting();
                // trigger hoe func in end of anim
            }
            else
            {
                Debug.Log($"Tool name not found: {this.gameObject.name}");
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
        animator.SetTrigger("isTriggered");
        gameObject.GetComponent<GunBehavior>().Shoot();
    }

    public void TriggerHoe()
    {
        //GetSoil();

        // check if soil

        if (soil)
        {
            if (soil.soilStatus == Soil.SoilStatus.ForHarvesting)
            {
                soil.HarvestPlant();
            }
            else
            {
                soil.TillSoil();
            }
            
            
        }
        else
        {
            Debug.Log("Missing Soil Component");
        }
    }

    void TriggerWateringCan()
    {
        //GetSoil();
        if (soil)
        {

            if (soil.soilStatus == Soil.SoilStatus.ForHarvesting)
            {
                soil.HarvestPlant();
            }
            else
            {
                soil.WaterSoil();
            }

        }
        else
        {
            Debug.Log("Missing Soil Component");
        }

        // get soil comp
        // trigger functions
    }

    void TriggerPlanting()
    {
        //GetSoil();
        if (soil)
        {
            if (soil.soilStatus == Soil.SoilStatus.ForHarvesting)
            {
                soil.HarvestPlant();
            }
            else
            {
                soil.PlantSeed(this.plant);
            }
            

        }
        else
        {
            Debug.Log("Missing Soil Component");
        }

        // get soil comp
        // trigger functions
    }

    #endregion


    void GetSoil()
    {
        

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
        {
            Debug.Log("Hit: " + hit.transform.name);
            //Select stage    
            if (hit.transform.CompareTag("Soil"))
            {
                soil = hit.transform.GetComponent<Soil>();
            }
            
           
        }
        else
        {
            Debug.Log("did not hit anything");
        }

    }

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

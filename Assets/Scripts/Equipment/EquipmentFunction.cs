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

    [Header("Data Reference")]
    [SerializeField] InventoryItemData obj;

    [SerializeField] Transform meleeCollider;
    [SerializeField] LayerMask targetLayer;


    public Soil soil;
    public HoverBehavior hover;

    [Header("Sound Files")] 
    [SerializeField] private AudioClip usetoolSFX;

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
        if (Input.GetMouseButtonDown(0) && (!GameManager.instance.isInventory && Time.timeScale != 0))
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
                animator.SetTrigger("isTriggered");

            }
            else if (CompareName("Rose Weapon"))
            {
                CombatManager.instance.UseRoseDagger(obj.attackDamage);
            }
            else if (CompareName("Cabbage Weapon"))
            {
                CombatManager.instance.UseCabbageBomb(obj.attackDamage);
            }
            else
            {
                Debug.Log($"Weapon name not found: {this.gameObject.name}");
            }
        }
        else if (type == EquipmentType.Tool) // tools
        {
            GetSoilandHover();
            if (hover)
            {
                if (hover.inRange)
                {
                    animator.SetTrigger("isTriggered");
                }
            }
            
        }
        else if (type == EquipmentType.Plant) // tools
        {
            
                GetSoilandHover();
            if (hover.inRange)
            {
                TriggerPlanting();
            }
                // trigger hoe func in end of anim
        }
        
    }
    #endregion

    #region Equipment Trigger Functions 
    void TriggerPistol()
    {
        animator.SetTrigger("isTriggered");
        if (WeaponManager.instance.bulletCount > 0)
        {
            gameObject.GetComponent<GunBehavior>().Shoot();
            AudioManager.instance.PlaySFX(usetoolSFX);
        }
        else
        {
            Debug.Log("No ammo");
        }
    }

    public void TriggerMachete()
    {
        Collider[] colliders = Physics.OverlapSphere(meleeCollider.position, 1f, targetLayer);
        foreach(Collider  c in colliders)
        {
            c.GetComponent<EnemyBehaviour>().ReceiveDamage(WeaponManager.instance.weaponData.attackDamage);
        }
    }

    public void TriggerHoe()
    {
        if (soil && hover.inRange)
        {
            if (soil.soilStatus == Soil.SoilStatus.ForHarvesting)
            {
                soil.HarvestPlant();
            }
            else
            {
                soil.TillSoil();
            }

            AudioManager.instance.PlaySFX(usetoolSFX);
        }
        else
        {
            Debug.Log("Missing Soil Component");
        }
    }

    void TriggerWateringCan()
    {
        if (soil && hover.inRange)
        {

            if (soil.soilStatus == Soil.SoilStatus.ForHarvesting)
            {
                soil.HarvestPlant();
            }
            else
            {
                soil.WaterSoil();
            }
            AudioManager.instance.PlaySFX(usetoolSFX);
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
        if (soil && hover.inRange)
        {
            if (soil.soilStatus == Soil.SoilStatus.ForHarvesting)
            {
                soil.HarvestPlant();
            }
            else
            {
                soil.PlantSeed(this.obj);

                // Remove stack from inventory
                //HotbarSelectorManager.instance.currInvSlot.ClearSlot();
                
            }
            AudioManager.instance.PlaySFX(usetoolSFX);

        }
        else
        {
            Debug.Log("Missing Soil Component");
        }

        // get soil comp
        // trigger functions
    }

    #endregion


    void GetSoilandHover()
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
                hover = hit.transform.GetComponent<HoverBehavior>();
            }
            
           
        }
        else
        {
            Debug.Log("did not hit anything");
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(.1f, .1f, 1, .1f);
        if (meleeCollider)
            Gizmos.DrawSphere(meleeCollider.position, 1f);
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

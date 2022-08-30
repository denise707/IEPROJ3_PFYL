using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private Transform playerSprite;
    [SerializeField] Animator animator;

    [Header("Pivots")]
    [SerializeField] private Transform weaponAim;
    [SerializeField] private Transform toolAim;

    [Header("Spawn Points")]
    [SerializeField] private Transform weaponSP;
    [SerializeField] private Transform toolSP;

    // temp can be accessed from inv?
    [Header("Weapon Temp")]
    [SerializeField] private WeaponObject weapon;
    public bool hasWeapon = false;

    // temp can be accessed from inv?
    [Header("Tool Temp")]
    [SerializeField] private ToolObject hoeTool;
    public bool hasTool = false;

    [SerializeField] private ToolObject wateringCanTool;

    [Header("Item Temp")]
    [SerializeField] private PlantObject plant;
    public bool hasPlant = false;

    private float rotationY;
    bool[] inputs;
    public InventoryObject inventory;
    public InventoryObject hotbar;

    // Start is called before the first frame update
    void Start()
    {
        inputs = new bool[4];
    }

    // Update is called once per frame
    void Update()
    {
        
        UpdatePlayerTool();
        // Debug.Log(aim.rotation.eulerAngles);
        if (weaponAim)
        {
            rotationY = weaponAim.rotation.eulerAngles.y;

        }
        if (animator)
        {
            UpdateAnimation2();
        }
    }

    private void FixedUpdate()
    {
        UpdatePlayerMovement();
    }

    private void UpdatePlayerTool()
    {
        // ADD
        if (Input.GetKeyDown(KeyCode.U) && !hasWeapon)
        {
            // get scriptable from inventory and acces the prefab
            GameObject obj = (GameObject)Instantiate(weapon.prefab ,weaponSP);
            obj.name = weapon.name;

            //bulletSphere.transform.LookAt(new Vector3(hit.point.x, hit.point.y, hit.point.z));
            Debug.Log("Spawn Gun");
            hasWeapon = true;
        }
        // REMOVE
        else if (Input.GetKeyDown(KeyCode.U) && hasWeapon)
        {
           
            Transform obj = weaponSP.GetChild(0);
            Destroy(obj.gameObject);
            Debug.Log("destroyed weapon");
            hasWeapon = false;
        }

        //ADD
        if (Input.GetKeyDown(KeyCode.I) && !hasTool)
        {
            // get scriptable from inventory and acces the prefab
            GameObject obj = (GameObject)Instantiate(hoeTool.prefab, toolSP);
            obj.name = hoeTool.name;
            //bulletSphere.transform.LookAt(new Vector3(hit.point.x, hit.point.y, hit.point.z));
            Debug.Log("Spawn Tool");
            hasTool = true;
        }
        //REMOVE
        else if (Input.GetKeyDown(KeyCode.I) && hasTool)
        {
            Transform obj = toolSP.GetChild(0);
            Destroy(obj.gameObject);
            Debug.Log("destroyed Tool");
            hasTool = false;
        }

        //ADD
        if (Input.GetKeyDown(KeyCode.O) && !hasTool)
        {
            // get scriptable from inventory and acces the prefab
            GameObject obj = (GameObject)Instantiate(plant.prefab, toolSP);
            obj.name = plant.name;
            //bulletSphere.transform.LookAt(new Vector3(hit.point.x, hit.point.y, hit.point.z));
            Debug.Log("Spawn Tool");
            hasTool = true;
        }
        //REMOVE
        else if (Input.GetKeyDown(KeyCode.O) && hasTool)
        {
            Transform obj = toolSP.GetChild(0);
            Destroy(obj.gameObject);
            Debug.Log("destroyed Tool");
            hasTool = false;
        }

        //ADD
        if (Input.GetKeyDown(KeyCode.P) && !hasTool)
        {
            // get scriptable from inventory and acces the prefab
            GameObject obj = (GameObject)Instantiate(wateringCanTool.prefab, toolSP);
            obj.name = wateringCanTool.name;
            //bulletSphere.transform.LookAt(new Vector3(hit.point.x, hit.point.y, hit.point.z));
            Debug.Log("Spawn Tool");
            hasTool = true;
        }
        //REMOVE
        else if (Input.GetKeyDown(KeyCode.P) && hasTool)
        {
            Transform obj = toolSP.GetChild(0);
            Destroy(obj.gameObject);
            Debug.Log("destroyed Tool");
            hasTool = false;
        }

    }

    private void UpdatePlayerMovement()
    {

        // player movement
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0.0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    private void UpdateAnimation()
    {
        if (Input.GetKeyUp(KeyCode.W))
        {
            inputs[0] = false;
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            inputs[1] = false;
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            inputs[2] = false;
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            inputs[3] = false;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            ResetInputState();
            inputs[0] = true;
            animator.SetBool("faceBack", inputs[0]); // w - back
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            ResetInputState();
            inputs[1] = true;
            animator.SetBool("faceFront", inputs[1]); // S - front
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            ResetInputState();
            inputs[2] = true;
            animator.SetBool("faceLeft", inputs[2]); // A - left
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            ResetInputState();
            inputs[3] = true;
            animator.SetBool("faceRight", inputs[3]); // D - right
        }

        
    }

    private void UpdateAnimation2()
    {
        // key for not not moving
        if (Input.GetKeyUp(KeyCode.W))
        {
            inputs[0] = false;
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            inputs[1] = false;
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            inputs[2] = false;
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            inputs[3] = false;
        }

        // direction facing
        if ((Utils.InRange(rotationY, 0, 44) || Utils.InRange(rotationY, 316, 360)) && inputs[0] == false )
        {
            ResetInputState();
            inputs[0] = true;
           // animator.SetTrigger("Flip");
            animator.SetBool("faceBack", inputs[0]); // w - back
        }
        else if (Utils.InRange(rotationY, 135, 225) == true && inputs[1] == false)
        {
            ResetInputState();
            inputs[1] = true;
          //  animator.SetTrigger("Flip");
            animator.SetBool("faceFront", inputs[1]); // S - front
        }
        else if (Utils.InRange(rotationY, 226, 315) ==  true && inputs[2] == false)
        {
            ResetInputState();
            inputs[2] = true;
           // animator.SetTrigger("Flip");
            animator.SetBool("faceLeft", inputs[2]); // A - left
        }
        else if (Utils.InRange(rotationY, 45, 134) == true && inputs[3] == false)
        {
            ResetInputState();
            inputs[3] = true;
            //animator.SetTrigger("Flip");
            animator.SetBool("faceRight", inputs[3]); // D - right
        }

     
            PlayerLookAt();
       
    }

    private void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<GroundItem>();
        if (item)
        {
            //will need update to stack items properly
            Item _item = new Item(item.item);
            if(hotbar.AddItem(_item, 1))
            {
                Destroy(other.gameObject);
            }
            else
            {
                if(inventory.AddItem(_item, 1)){
                    Destroy(other.gameObject);
                }
            }
            
        }
    }

    //Clearing inventory after quitting play
    private void OnApplicationQuit()
    {
        inventory.Container.Clear();
        hotbar.Container.Clear();
    }

    private void ResetInputState()
    {
        for (int i = 0; i < inputs.Length; i++)
        {
            inputs[i] = false;
        }
        animator.SetBool("faceBack", inputs[0]); // w - back
        animator.SetBool("faceFront", inputs[1]); // S - front
        animator.SetBool("faceLeft", inputs[2]); // A - left
        animator.SetBool("faceRight", inputs[3]); // D - right
    }

    private void PlayerLookAt()
    {
        // Player look At
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (inputs[0] || inputs[1])
            {
                playerSprite.LookAt(hit.point); // Look at the point
                transform.rotation = Quaternion.Euler(new Vector3(0, playerSprite.rotation.eulerAngles.y, 0)); // Clamp the x and z rotation

                //Vector3 dir = playerSprite.position - hit.point;
                //Quaternion rotation = Quaternion.LookRotation(dir);
                //playerSprite.rotation = Quaternion.Lerp(transform.rotation, rotation, speed * Time.deltaTime);
            }
            else
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0)); // Clamp the x and z rotation
            }
           
            
        }

    }
}

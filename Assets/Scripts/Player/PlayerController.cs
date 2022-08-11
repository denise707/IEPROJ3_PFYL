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
    [Header("Weapon")]
    [SerializeField] private GameObject pistol;
    public bool hasGun = false;

    // temp can be accessed from inv?
    [Header("Tool")]
    [SerializeField] private GameObject tool;
    public bool hasTool = false;

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
        UpdatePlayerMovement();
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

    private void UpdatePlayerTool()
    {
        if (Input.GetKeyDown(KeyCode.U) && !hasGun)
        {
            // when [tool/wep] is active in inventory -> instantiate or spawn [tool/wep] in corresponding spawn point
            GameObject bulletSphere = (GameObject)Instantiate(pistol ,weaponSP);
            //bulletSphere.transform.LookAt(new Vector3(hit.point.x, hit.point.y, hit.point.z));
            Debug.Log("Spawn Gun");
            hasGun = true;
        }else if (Input.GetKeyDown(KeyCode.U) && hasGun)
        {
            // when [tool/wep] is active in inventory -> instantiate or spawn [tool/wep] in corresponding spawn point
            Transform obj = weaponSP.GetChild(0);
            Destroy(obj.gameObject);
            Debug.Log("destroyed gun");
            hasGun = false;
        }

        if (Input.GetKeyDown(KeyCode.I) && !hasTool)
        {
            // when [tool/wep] is active in inventory -> instantiate or spawn [tool/wep] in corresponding spawn point
            GameObject bulletSphere = (GameObject)Instantiate(tool, toolSP);
            //bulletSphere.transform.LookAt(new Vector3(hit.point.x, hit.point.y, hit.point.z));
            Debug.Log("Spawn Tool");
            hasTool = true;
        }
        else if (Input.GetKeyDown(KeyCode.I) && hasTool)
        {
            // when [tool/wep] is active in inventory -> instantiate or spawn [tool/wep] in corresponding spawn point
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
            if(hotbar.AddItem(new Item(item.item), 1))
            {
                Destroy(other.gameObject);
            }
            else
            {
                if(inventory.AddItem(new Item(item.item),1)){
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

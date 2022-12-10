using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private Transform playerSprite;
    [SerializeField] Animator animator;

    [Header("Pivots")]
    [SerializeField] private Transform ThreeDimAimPivot;


    [Header("Spawn Points")]
    [SerializeField] private Transform weaponSP;
    [SerializeField] private Transform toolSP;
    [SerializeField] private Transform twoDimObjSP;

    // temp can be accessed from inv?
    public bool hasWeapon = false;

    // temp can be accessed from inv?
    public bool hasTool = false;

    public bool hasPlant = false;

    private float rotationY;
    bool[] inputs;

    bool hasStopped = false;

    [Header("Sound Files")]
    [SerializeField] private AudioClip receiveDamageSFX;
    [SerializeField] private AudioSource walkSFX;
    [SerializeField] private AudioClip handSFX;
    private Soil soil;
    private HoverBehavior hover;

    // Start is called before the first frame update
    void Start()
    {
        inputs = new bool[4];
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(aim.rotation.eulerAngles);
        if (GameManager.instance.isInventory == false)
        {
            if (ThreeDimAimPivot)
            {
                rotationY = ThreeDimAimPivot.rotation.eulerAngles.y;
            }

            if (animator)
            {
                UpdatePlayerInput();
                UpdateAnimation();
            }
        }
        else if(isPlayingMoving())
        {
            StopMovement();
        }
       
    }

    private void FixedUpdate()
    {
        if(GameManager.instance.isInventory == false)
            UpdatePlayerMovement();
    }

    public void UpdatePlayerEquip(InventoryItemData itemData)
    {
        if (hasTool && toolSP.childCount > 0)
        {
            Transform obj = toolSP.GetChild(0);
            Destroy(obj.gameObject);
            //Debug.Log("destroyed Tool");
            hasTool = false;
        }

        if (hasWeapon && weaponSP.childCount > 0)
        {
            Transform obj = weaponSP.GetChild(0);
            Destroy(obj.gameObject);
            //Debug.Log("destroyed Weapon");
            hasTool = false;
        }

        if (hasPlant && twoDimObjSP.childCount > 0)
        {
            Transform obj = twoDimObjSP.GetChild(0);
            Destroy(obj.gameObject);
            //Debug.Log("destroyed Weapon");
            hasPlant = false;
        }

        if (itemData == null)
        {
            return;
        }

        // ADD
        if (itemData.itemType == InventoryItemData.ItemType.Weapon)
        {
            // get scriptable from inventory and acces the prefab
            GameObject obj = (GameObject)Instantiate(itemData.prefab ,weaponSP);
            WeaponManager.instance.SetWeaponData(itemData);
            obj.name = itemData.name;

            //bulletSphere.transform.LookAt(new Vector3(hit.point.x, hit.point.y, hit.point.z));
            //Debug.Log($"Spawn {obj.name}");
            hasWeapon = true;
        }

        //ADD
        if (itemData.itemType == InventoryItemData.ItemType.Tool)
        {
            // get scriptable from inventory and acces the prefab
            GameObject obj = (GameObject)Instantiate(itemData.prefab, toolSP);
            obj.name = itemData.name;
            //bulletSphere.transform.LookAt(new Vector3(hit.point.x, hit.point.y, hit.point.z));
           // Debug.Log($"Spawn {obj.name}");
            hasTool = true;
        }

        //ADD
        if ( itemData.itemType == InventoryItemData.ItemType.Plant)
        {
            // get scriptable from inventory and acces the prefab
            GameObject obj = (GameObject)Instantiate(itemData.prefab, twoDimObjSP);
            obj.name = itemData.name;
            //bulletSphere.transform.LookAt(new Vector3(hit.point.x, hit.point.y, hit.point.z));
            // Debug.Log($"Spawn {obj.name}");
            hasPlant = true;
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
        animator.SetBool("isMoving", isPlayingMoving()); // w - back


        // direction facing
        if ((Utils.InRange(rotationY, 0, 44) || Utils.InRange(rotationY, 316, 360)))
        {
            ResetInputState();
            animator.SetBool("faceBack", true); // w - back
            //twoDimObjAim.transform.rotation = Quaternion.EulerAngles(0, 0, 0);
        }
        else if (Utils.InRange(rotationY, 135, 225) == true)
        {
            ResetInputState();
            animator.SetBool("faceFront", true); // S - front
            //twoDimObjAim.transform.rotation = Quaternion.EulerAngles(0, 180, 0);

        }
        else if (Utils.InRange(rotationY, 226, 315) == true)
        {
            ResetInputState();
            animator.SetBool("faceLeft", true); // A - left
            //twoDimObjAim.transform.rotation = Quaternion.EulerAngles(0, -90, 0);

        }
        else if (Utils.InRange(rotationY, 45, 134) == true)
        {
            ResetInputState();
            animator.SetBool("faceRight", true); // D - right
            //twoDimObjAim.transform.rotation = Quaternion.EulerAngles(0, 90, 0);

        }
        // PlayerLookAt();

    }

    private void UpdatePlayerInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CheckHarvest();
        }
        // key for not not moving
        if (Input.GetKeyDown(KeyCode.W))
        {
            inputs[0] = true;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            inputs[1] = true;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            inputs[2] = true;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            inputs[3] = true;
        }

        // key for not not moving
        if (Input.GetKeyUp(KeyCode.W))
        {
            inputs[0] = false;
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            inputs[1] = false;
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            inputs[2] = false;
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            inputs[3] = false;
        }

        if (!inputs[0] && !inputs[1] && !inputs[2] && !inputs[3])
        {
            walkSFX.Stop();
        }

        if (!inputs[0] && !inputs[1] && !inputs[2] && !inputs[3])
        {
            if (Time.timeScale == 1)
                walkSFX.Play();
        }
    }


    private void OnTriggerEnter(Collider other)
    {/*
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
            
        }*/
    }

    //Clearing inventory after quitting play
    private void OnApplicationQuit()
    {

    }

    private void ResetInputState()
    {
        for (int i = 0; i < inputs.Length; i++)
        {
            //inputs[i] = false;
        }
        animator.SetBool("faceBack", false); // w - back
        animator.SetBool("faceFront", false); // S - front
        animator.SetBool("faceLeft", false); // A - left
        animator.SetBool("faceRight", false); // D - right
    }
    private void StopMovement()
    {
        for (int i = 0; i < inputs.Length; i++)
        {
            inputs[i] = false;
        }
        animator.SetBool("isMoving",false); // w - back

    }

    private bool isPlayingMoving()
    {
        for (int i = 0; i <inputs.Length; i++)
        {
            if (inputs[i] == true)
            {
                return true;
            }
        }

        return false;
    }

    void CheckHarvest()
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

                if (soil && hover.inRange)
                {
                    if (soil.soilStatus == Soil.SoilStatus.ForHarvesting)
                    {
                        soil.HarvestPlant();
                    }
                    AudioManager.instance.PlaySFX(handSFX);

                }
                else
                {
                    Debug.Log("Missing Soil Component");
                }
            }


        }
        else
        {
            Debug.Log("did not hit anything");
        }

    }

    public void TakeDamage(float damage)
    {
        PlayerData.instance.currHP -= damage;
        //this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        Debug.Log("Hit");
        AudioManager.instance.PlaySFX(receiveDamageSFX);
        InGameUIManager.instance.UpdateHP();
    }

    public void AddGold(int amnt)
    {
        PlayerData.instance.GOLD += amnt;
        InGameUIManager.instance.UpdateGold();

    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Item"))
        {
            Destroy(collision.gameObject);
        }
    }

    public void SetActiveTakeDamageEffect(bool isActive)
    {
        if (isActive)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            StartCoroutine(RemoveTakeDamageEffect());
        }
    }

    IEnumerator RemoveTakeDamageEffect()
    {
        yield return new WaitForSeconds(0.15f);
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
    }

    public void SetSpeed(float _speed)
    {
        this.speed = _speed;
        if(this.speed < 5f)
        {
            GetComponent<SpriteRenderer>().color = new Color(199f / 255f, 217f / 255f, 85f / 255f, 1f);
        }
        else
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    [SerializeField] Animator animator;
    [SerializeField] private Transform aim;

    private float rotationY;
    bool[] inputs;
    public InventoryObject inventory;

    // Start is called before the first frame update
    void Start()
    {
        inputs = new bool[4];
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayerMovement();
        // Debug.Log(aim.rotation.eulerAngles);
        rotationY = aim.rotation.eulerAngles.y;
        UpdateAnimation2();
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

        if (rotationY >=-45 && rotationY >= -45)
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
        if (Utils.InRange(rotationY, 0, 45) || Utils.InRange(rotationY, 315, 360))
        {
            ResetInputState();
            inputs[0] = true;
            animator.SetBool("faceBack", inputs[0]); // w - back
        }
        else if (Utils.InRange(rotationY, 135, 225) == true)
        {
            ResetInputState();
            inputs[1] = true;
            animator.SetBool("faceFront", inputs[1]); // S - front
        }
        else if (Utils.InRange(rotationY, 226, 314) ==  true)
        {
            ResetInputState();
            inputs[2] = true;
            animator.SetBool("faceLeft", inputs[2]); // A - left
        }
        else if (Utils.InRange(rotationY, 46, 134) == true)
        {
            ResetInputState();
            inputs[3] = true;
            animator.SetBool("faceRight", inputs[3]); // D - right
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<Item>();
        if (item)
        {
            inventory.AddItem(item.item, 1);
            Destroy(other.gameObject);
        }
    }
    //Clearing inventory after quitting play
    private void OnApplicationQuit()
    {
        inventory.Container.Clear();
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

}

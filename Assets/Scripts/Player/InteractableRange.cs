using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableRange : MonoBehaviour
{

    HoverBehavior hover;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Soil"))
        {
            other.GetComponent<HoverBehavior>().inRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Soil") && other.GetComponent<HoverBehavior>().inRange == true)
        {
            other.GetComponent<HoverBehavior>().inRange = false;
        }
    }
}

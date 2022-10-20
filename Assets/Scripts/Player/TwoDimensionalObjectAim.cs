using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoDimensionalObjectAim : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    [SerializeField] private Transform Object;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Tool aim
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            transform.LookAt(hit.point); // Look at the point
            transform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y, 0)); // Clamp the x and z rotation
        }

        float rotationY = transform.rotation.eulerAngles.y;
        // direction facing
        if ((Utils.InRange(rotationY, 0, 44) || Utils.InRange(rotationY, 316, 360)))
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 89, 0));
            Object.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
        else if (Utils.InRange(rotationY, 135, 225) == true)
        {
            // S - front
            transform.rotation = Quaternion.Euler(new Vector3(0, -91, 0));
            Object.rotation = Quaternion.Euler(new Vector3(0, 0, 0));


        }
        else if (Utils.InRange(rotationY, 226, 315) == true)
        {
             // A - left
            transform.rotation = Quaternion.Euler(new Vector3(0, 360, 0));
            Object.rotation = Quaternion.Euler(new Vector3(0, 0, 0));


        }
        else if (Utils.InRange(rotationY, 45, 134) == true)
        {
             // D - right
            transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
            Object.rotation = Quaternion.Euler(new Vector3(0, 180, 0));

        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AngleChecker : MonoBehaviour
{
  
    

    
        [SerializeField]
        float eulerAngX;
        [SerializeField]
        float eulerAngY;
        [SerializeField]
        float eulerAngZ;

    [SerializeField] GameObject clock;

    private void Start()
    {
        clock = GameObject.FindGameObjectWithTag("Clock");
    }

    void Update()
        {

            eulerAngX = clock.transform.eulerAngles.x;
            eulerAngY = clock.transform.eulerAngles.y;
            eulerAngZ = clock.transform.eulerAngles.z;

        }
    
}

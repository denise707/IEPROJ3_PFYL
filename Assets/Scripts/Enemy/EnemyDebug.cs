using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDebug : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Keypad1)) { GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyBehaviour>().ReceiveDamage(10); };
    }
}

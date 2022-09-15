using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SlimePassive : MonoBehaviour
{
    [SerializeField] private GameObject trapCopy;
    [SerializeField] private GameObject spawnLoc;
    public List<GameObject> trapList = new List<GameObject>();

    private float ticks = 0f;
    const float ATTACK_INTERVAL = 1.0f;

    void Update()
    {
        this.ticks += Time.deltaTime;
        if (ticks > ATTACK_INTERVAL)
        {
            int trapChance = Random.Range(1, 100);
            if (trapChance >= 0)
            {
                GameObject newTrap = Instantiate(trapCopy, spawnLoc.transform.position, trapCopy.transform.rotation);
                trapList.Add(newTrap);
            }
            ticks = 0.0f;
        }
    }

    void OnDisable()
    {
        ClearTraps();
    }

    public void ClearTraps()
    {
        foreach (var trap in trapList)
        {
            Destroy(trap);
        }
        trapList.Clear();
    }
}

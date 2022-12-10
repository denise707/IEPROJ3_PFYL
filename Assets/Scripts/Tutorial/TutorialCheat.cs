using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCheat : MonoBehaviour
{
    PlayerController playerController;
    public NukePlantBehavior nukePlant;

    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        playerController.ResetHP();
        nukePlant.ResetHP();
    }
}

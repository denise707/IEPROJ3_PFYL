using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    public static int tutorialStep = 0;
    public static bool isDone = false;
    
    [SerializeField] List<GameObject> soilList;
    [SerializeField] Transform clock;

    bool isColliding = false;

    void Update()
    {
        PlantingTutorial();
    }

    void PlantingTutorial()
    {
        switch (tutorialStep)
        {
            //1. Use Hoe
            case 1:
                if (!isDone)
                {
                    for (int i = 0; i < soilList.Count; i++)
                    {
                        if (soilList[i].GetComponent<Soil>().soilStatus == Soil.SoilStatus.Tilled)
                        {
                            isDone = true;
                        }
                    }
                    if (isDone)
                    {
                        Dialogue.instance.StartDialogue(3);
                    }
                }                                                                 
                break;
            //2. Plant Seed
            case 2:
                if (!isDone)
                {
                    for (int i = 0; i < soilList.Count; i++)
                    {
                        if (soilList[i].GetComponent<Soil>().soilStatus == Soil.SoilStatus.Planted)
                        {
                            isDone = true;
                        }
                    }
                    if (isDone)
                    {
                        Dialogue.instance.StartDialogue(5);
                    }
                }
                break;

            //3. Water and Grow
            case 3:
                if (!isDone)
                {
                    for (int i = 0; i < soilList.Count; i++)
                    {
                        if (soilList[i].GetComponent<Soil>().soilStatus == Soil.SoilStatus.ForHarvesting)
                        {
                            isDone = true;
                        }
                    }
                    if (isDone)
                    {
                        Dialogue.instance.StartDialogue(7);
                    }
                }
                break;
            //4. Harvest
            case 4:
                if (!isDone)
                {
                    int total = 0;
                    for (int i = 0; i < soilList.Count; i++)
                    {
                        if (soilList[i].GetComponent<Soil>().soilStatus == Soil.SoilStatus.Default)
                        {
                            total++;
                        }
                    }
                    if (total >= 8)
                    {
                        isDone = true;
                        Dialogue.instance.StartDialogue(9);
                    }
                }
                break;
            //5. Wait for Night Time 
            case 5:               
                if (!isDone)
                {
                    TimeManager.instance.start = true;
                    if (TimeManager.instance.IsNightTime())
                    {
                        isDone = true;
                        TimeManager.instance.start = false;
                        Dialogue.instance.StartDialogue(11);
                    }
                }
                break;
            //6. Go to Nuke Plant
            case 6:
                if (!isDone)
                {
                    if (isColliding)
                    {
                        isDone = true;
                        Dialogue.instance.StartDialogue(13);
                    }
                }
                break;
            //7. Kill 1 Enemy
            case 7:
                if (!isDone)
                {
                    EnemySpawningManager.instance.spawn = true;
                    if (EnemySpawningManager.instance.enemyKilled >= 1)
                    {
                        isDone = true;
                        Dialogue.instance.StartDialogue(15);
                    }
                }
                break;
            //8. Survive til morning
            case 8:
                if (!isDone)
                {
                    EnemySpawningManager.instance.spawn = true;
                    TimeManager.instance.start = true;
                    if (!TimeManager.instance.IsNightTime())
                    {
                        isDone = true;
                        EnemySpawningManager.instance.spawn = false;
                        Dialogue.instance.StartDialogue(17);
                    }
                }
                break;
            //9. Main Menu
            case 9:
                SceneManager.LoadScene("Main Menu");
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Nuke Plant")
        {
            isColliding = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Nuke Plant")
        {
            isColliding = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Nuke Plant")
        {
            isColliding = false;
        }
    }
}

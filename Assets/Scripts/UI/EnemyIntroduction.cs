using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIntroduction : MonoBehaviour
{
    [SerializeField] GameObject zombieIntro;
    [SerializeField] GameObject golemIntro;
    [SerializeField] GameObject slimeIntro;

    [SerializeField] GameObject roseIntro;
    [SerializeField] GameObject bombIntro;
    [SerializeField] GameObject thornIntro;

    bool displayed = false;

    private void Update()
    {
        if (TimeManager.instance.IsNightTime() && !displayed)
        {
            switch (TimeManager.instance.day)
            {
                case 1:
                    zombieIntro.SetActive(true);
                    displayed = true;
                    StartCoroutine(RemoveInfo(zombieIntro, roseIntro));
                    break;
                case 2:
                    golemIntro.SetActive(true);
                    displayed = true;
                    StartCoroutine(RemoveInfo(golemIntro, bombIntro));
                    break;
                case 3:
                    slimeIntro.SetActive(true);
                    displayed = true;
                    StartCoroutine(RemoveInfo(slimeIntro, thornIntro));
                    break;
            }
        }
        
        if(!TimeManager.instance.IsNightTime())
        {
            displayed = false;
        }
    }

    IEnumerator RemoveInfo(GameObject info, GameObject plant)
    {
        yield return new WaitForSeconds(3f);
        info.SetActive(false);
        plant.SetActive(true);
        StartCoroutine(RemovePlantInfo(plant));
    }

    IEnumerator RemovePlantInfo(GameObject plant)
    {
        yield return new WaitForSeconds(3f);
        plant.SetActive(false);
    }
}

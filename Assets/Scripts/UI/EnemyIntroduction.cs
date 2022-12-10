using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIntroduction : MonoBehaviour
{
    [SerializeField] GameObject zombieIntro;
    [SerializeField] GameObject golemIntro;
    [SerializeField] GameObject slimeIntro;
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
                    StartCoroutine(RemoveInfo(zombieIntro));
                    break;
                case 2:
                    golemIntro.SetActive(true);
                    displayed = true;
                    StartCoroutine(RemoveInfo(golemIntro));
                    break;
                case 3:
                    slimeIntro.SetActive(true);
                    displayed = true;
                    StartCoroutine(RemoveInfo(slimeIntro));
                    break;
            }
        }
        
        if(!TimeManager.instance.IsNightTime())
        {
            displayed = false;
        }
    }

    IEnumerator RemoveInfo(GameObject info)
    {
        yield return new WaitForSeconds(3f);
        info.SetActive(false);
    }
}

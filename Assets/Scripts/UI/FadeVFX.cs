using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FadeVFX : MonoBehaviour
{
    public enum PanelState { Default, FadeIn, FadeOut };

    [SerializeField] float fadeInDur = 2.0f;
    [SerializeField] float fadeOutDur = 2.0f;

    [SerializeField] float minAlpha = 0.1f;
    [SerializeField] float maxAlpha = 0.3f;

    [SerializeField]public float ticks = 0.0f;
    
    public PanelState panelState = PanelState.Default;
    bool mFaded = false;

    void Start()
    {
        if (this == null)
        {
            Debug.Log("Object not Found");
            panelState = PanelState.Default;
        }
    }

    void Update()
    {
        if (panelState == PanelState.FadeIn)
        {
            
            ticks += Time.deltaTime;
            FadeIn();
        }

        else if (panelState == PanelState.FadeOut)
        {
            ticks += Time.deltaTime;
            FadeOut();
        }
    }

    public void UpdateFadeStatus()
    {
        if (gameObject.GetComponent<CanvasGroup>().alpha == minAlpha) 
        {
            panelState = PanelState.Default;
        }

        if (gameObject.GetComponent<CanvasGroup>().alpha == maxAlpha) 
        {
            panelState = PanelState.Default;
        }
    }

    public void FadeIn()
    {
        if(ticks < fadeInDur)
        {
            gameObject.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(minAlpha, maxAlpha, ticks/fadeInDur);
        }

        else
        {
            panelState = PanelState.Default;
            ticks = 0.0f;
            mFaded = false;
        }
    }

    public void FadeOut()
    {
        if (ticks < fadeOutDur)
        {
            gameObject.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(maxAlpha, minAlpha, ticks/fadeOutDur);
        }

        else
        {
            panelState = PanelState.Default;
            ticks = 0.0f;
            mFaded = true;
            this.gameObject.SetActive(false);
        }
    }
}

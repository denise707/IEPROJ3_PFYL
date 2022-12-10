using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemRange : MonoBehaviour
{
    private bool shouldLerp = false;

    public float timeStartedLerping;
    public float lerpTime;

    private Vector3 minScale;
    private Vector3 maxScale;

    [SerializeField] private GameObject range;
    [SerializeField] private GameObject spike;
    [SerializeField] private GameObject maxRange;

    private void OnDisable()
    {
        HideRange();
        HideSpike();
    }

    public void showRange()
    {
        range.SetActive(true);
        maxRange.SetActive(true);
        timeStartedLerping = Time.time;
        shouldLerp = true;
        minScale = new Vector3(0, 0, 0);
        maxScale = new Vector3(6, 6, 6);
    }

    void Update()
    {
        if (shouldLerp)
        {
            range.transform.localScale = Lerp(minScale, maxScale, timeStartedLerping, lerpTime);
        }
    }

    public Vector3 Lerp(Vector3 start, Vector3 end, float timeStartedLerping, float lerpTime = 1)
    {
        float timeSinceStarted = Time.time - timeStartedLerping;

        float percentageComplete = timeSinceStarted / lerpTime;

        var result = Vector3.Lerp(start, end, percentageComplete);
        Debug.Log(result);

        return result;
    }

    public void HideRange()
    {
        range.SetActive(false);
        maxRange.SetActive(false);
        shouldLerp = false;
    }

    public void ShowSpike()
    {
        spike.SetActive(true);
    }

    public void HideSpike()
    {
        range.transform.localScale = new Vector3(0, 0, 0);
        timeStartedLerping = Time.time;
        spike.SetActive(false);
    }
}

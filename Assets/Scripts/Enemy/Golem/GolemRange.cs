using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemRange : MonoBehaviour
{
    private bool shouldLerp = false;

    public float timeStartedLerping;
    public float lerpTime;

    public Vector3 minScale;
    public Vector3 maxScale;

    [SerializeField] private GameObject range;
    [SerializeField] private GameObject spike;

    public void showRange()
    {
        range.SetActive(true);
        timeStartedLerping = Time.time;
        shouldLerp = true;
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

        return result;
    }

    public void HideRange()
    {
        range.SetActive(false);
    }

    public void ShowSpike()
    {
        spike.SetActive(true);
    }

    public void HideSpike()
    {
        spike.SetActive(false);
    }
}

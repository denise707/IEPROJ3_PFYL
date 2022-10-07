using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]

public class LightManager : MonoBehaviour
{
    [Header("Light")]
    [SerializeField] private Light directionalLight;
    [SerializeField] private Color startColor;
    [SerializeField] private Color targetColor;
    [SerializeField] private Color[] lightColor;


    [SerializeField] private float elapsedTime=0.0f;
    [SerializeField] private float lerpDuration;
    [SerializeField] private float interpolation;
    [SerializeField] private float angle;

    public int colorIndexA = 0;
    public int colorIndexB = 1;

    public bool DoneSequence= false;
    public bool SequencePass = false;



    [SerializeField] [Range(0f, 1f)] float lerptime;

    [SerializeField] private Transform clock_rotation;

    public static LightManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        clock_rotation = GameObject.FindGameObjectWithTag("Clock").GetComponent<Transform>();
        lerpDuration = 360/lightColor.Length;

        startColor = lightColor[colorIndexA];
        targetColor = lightColor[colorIndexB];
    }

    // Update is called once per frame
    void Update()
    {
        angle = (360 - clock_rotation.eulerAngles.z);
        elapsedTime =angle %lerpDuration;

        if (elapsedTime <= lerpDuration)
        {
            //sunrise to morning
            interpolation = elapsedTime / lerpDuration;
            directionalLight.color = Color.Lerp(lightColor[colorIndexA], lightColor[colorIndexB], interpolation);

        }

        if (angle <= 72 && angle >= 0)
        {
            colorIndexA = 0;
            colorIndexB = 1;

            startColor = lightColor[colorIndexA];
            targetColor = lightColor[colorIndexB];
        }
        else if (angle > 72 && angle <= 144)
        {
            colorIndexA = 1;
            colorIndexB = 2;
            startColor = lightColor[colorIndexA];
            targetColor = lightColor[colorIndexB];

        }
        else if (angle > 144 && angle <= 216)
        {
            colorIndexA = 2;
            colorIndexB = 3;

            startColor = lightColor[colorIndexA];
            targetColor = lightColor[colorIndexB];
        }
        else if (angle > 216 && angle <= 277)
        {
            colorIndexA = 3;
            colorIndexB = 4;

            startColor = lightColor[colorIndexA];
            targetColor = lightColor[colorIndexB];
        }
        else if (angle > 277 && angle <= 360)
        {
            colorIndexA = 4;
            colorIndexB = 0;

            startColor = lightColor[colorIndexA];
            targetColor = lightColor[colorIndexB];
        }


        //if (DoneSequence == true && SequencePass == true)
        //{
        //    NextLightSequence();
        //    Debug.Log($"{elapsedTime}> {lerpDuration-1.1}");
        //    elapsedTime = 0;
        //    //interpolation = 0;
        //    DoneSequence = false;
        //    SequencePass = false;
        //}


    }
    public void NextLightSequence()
    {
        colorIndexA++;
        colorIndexB++;

        if (colorIndexA >lightColor.Length-1)
        {
            colorIndexA = 0;
        }

        if (colorIndexB > lightColor.Length-1)
        {
            colorIndexB = 0;
        }
    }
}

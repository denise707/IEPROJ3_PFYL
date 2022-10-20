using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]

public class LightManager : MonoBehaviour
{
    [Header("Light")]
    [SerializeField] private Light directionalLight;
    private Light pointLight;

    [SerializeField] private Color startColor;
    [SerializeField] private Color targetColor;
    [SerializeField] private Color[] lightColor;
    [SerializeField] bool useSecondaryLight= false;


    [SerializeField]  public float elapsedTime=0.0f;
    [SerializeField] public float lerpDuration =0;
    [SerializeField] private float interpolation;

    [SerializeField] int colorIndexA = 0;
    [SerializeField]  int colorIndexB = 1;

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

        // set the start and next color to transition to
        startColor = lightColor[0];
        targetColor = lightColor[1];
        GameObject lightObj = GameObject.FindGameObjectWithTag("2D PointLight");
        if (lightObj)
        {
            pointLight = lightObj.GetComponent<Light>();

        }

    }

    // Update is called once per frame
    void Update()
    {
       
        if (elapsedTime > lerpDuration)
        {
            Debug.Log("skip: " + interpolation);
            NextLightSequence();
            elapsedTime = 0;
            interpolation = 0;
        }

        interpolation = elapsedTime / lerpDuration;
        directionalLight.color = Color.Lerp(lightColor[colorIndexA], lightColor[colorIndexB], interpolation);
        if (useSecondaryLight)
        pointLight.color = directionalLight.color;

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
        startColor = lightColor[colorIndexA];
        targetColor = lightColor[colorIndexB];
    }
}

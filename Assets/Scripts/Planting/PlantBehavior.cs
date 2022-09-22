using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantBehavior : MonoBehaviour
{
    public enum PlantPhase
    {
        Phase0_Default,
        Phase1_Seedling,
        Phase2_MidGrown,
        Phase3_FullyGrown
    }

    [Header("Plant Properties")]
    [SerializeField] PlantObject plantToGrow;

    public PlantPhase plantStatus = PlantPhase.Phase0_Default; 
    [SerializeField] int phase = 0;
     public bool simulateGrowth = false;
    [SerializeField] bool isGrowing = false;

    [SerializeField] float growthDuration = 0;

    public float ticks = 0f;

    [Header("Plant Growth Sprite")]
    [SerializeField] List<Sprite> GrowthSpriteList;

    [Header("Plant Drops Sprite")]
    [SerializeField] Sprite DropA_sprt;
    [SerializeField] Sprite DropB_sprt;

    Soil soil;

   
    // Start is called before the first frame update
    void Start()
    {
        if (plantToGrow)
        {
            UpdatePlantProperty(plantToGrow);
        }

        soil = gameObject.GetComponentInParent<Soil>();
       
    }

    // Update is called once per frame
    void Update()
    {
        if (simulateGrowth && !isGrowing)
        {
            EnablePlantGrowth();
            isGrowing = true;
        }
    }

    public void UpdatePlantProperty(PlantObject plantObj)
    {
        this.gameObject.transform.localPosition = gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, 0.5f, gameObject.transform.localPosition.z);
        Debug.Log("Updating Plant Stats");
        this.GrowthSpriteList = plantObj.PlantGrowthSpriteList;
        this.DropA_sprt = plantObj.DropsA;
        this.DropB_sprt = plantObj.DropsB;

        this.growthDuration = plantObj.growthDuration;

        this.phase = 0;

        this.gameObject.GetComponent<SpriteRenderer>().sprite = GrowthSpriteList[phase]; // seed
        plantStatus = PlantPhase.Phase1_Seedling;


    }

    public void ResetPlantProperty()
    {
        this.gameObject.transform.localPosition = gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, 0.5f, gameObject.transform.localPosition.z);
        Debug.Log("Reset Plant Stats");
        //this.GrowthSpriteList.Clear();
        this.DropA_sprt = null;
        this.DropB_sprt = null;

        this.growthDuration = 0;

        this.phase = 0;
        plantStatus = PlantPhase.Phase0_Default;


        //this.gameObject.GetComponent<SpriteRenderer>().sprite = GrowthSpriteList[phase]; // seed
        //plantStatus = PlantPhase.Phase1_Seedling;
    }


    public void EnablePlantGrowth()
    {
        //this.isGrowing = true;
        Debug.Log("Start Growing");
        //ticks = 0;
        isGrowing = true;

        int nTransitions = GrowthSpriteList.Count - 1;


        StartCoroutine(NextPlantPhase(growthDuration / nTransitions));
        
        //PrepareForHarvest();
    }

    IEnumerator NextPlantPhase(float timePerPhase)
    {
        

        for (int i = 0; i < GrowthSpriteList.Count-1; i++)
        {
            yield return new WaitForSeconds(timePerPhase);
            phase++;
            Debug.Log(phase);


            if (phase == 1)
            {
                this.gameObject.transform.localPosition = gameObject.transform.localPosition= new Vector3(gameObject.transform.localPosition.x, 0.80f, gameObject.transform.localPosition.z);
                plantStatus = PlantPhase.Phase2_MidGrown;
                this.gameObject.GetComponent<SpriteRenderer>().sprite = GrowthSpriteList[phase];

            }
            else if (phase == 2)
            {
                plantStatus = PlantPhase.Phase3_FullyGrown;
                this.gameObject.GetComponent<SpriteRenderer>().sprite = GrowthSpriteList[phase];
                PrepareForHarvest();
                isGrowing = false;
                simulateGrowth = false;
            }

        }
        
    }


    void PrepareForHarvest()
    {
        if (soil)
        {

            soil.soilStatus = Soil.SoilStatus.ForHarvesting;

        }
        else {
            Debug.Log("not found");
                
                }
        // enable particle

    }
}

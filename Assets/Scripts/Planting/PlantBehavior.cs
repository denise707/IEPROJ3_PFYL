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
    public PlantPhase plantStatus = PlantPhase.Phase0_Default;
    [SerializeField] int phase = 0;
    [SerializeField] bool isGrowing = false;
    [SerializeField] float growthDuration = 0;

    public float ticks = 0f;

    [Header("Plant Growth Sprite")]
    [SerializeField] List<Sprite> GrowthSpriteList;

    [Header("Plant Drops Sprite")]
    [SerializeField] Sprite DropA_sprt;
    [SerializeField] Sprite DropB_sprt;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrowing)
        {
            ticks += Time.deltaTime;

            if(ticks >= this.growthDuration)
            {
                //StartCoroutine
            }
        }
    }

    public void UpdatePlantProperty(PlantObject plantObj)
    {
        Debug.Log("Updating Plant Stats");
        this.GrowthSpriteList = plantObj.PlantGrowthSpriteList;
        this.DropA_sprt = plantObj.DropsA;
        this.DropA_sprt = plantObj.DropsB;

        this.growthDuration = plantObj.growthDuration;

        this.phase = 0;

        this.gameObject.GetComponent<SpriteRenderer>().sprite = GrowthSpriteList[phase]; // seed
        plantStatus = PlantPhase.Phase1_Seedling;


    }

    public void EnablePlantGrowth()
    {
        //this.isGrowing = true;
        Debug.Log("Start Growing");
        //ticks = 0;

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
                plantStatus = PlantPhase.Phase2_MidGrown;
                this.gameObject.GetComponent<SpriteRenderer>().sprite = GrowthSpriteList[phase];

            }
            else if (phase == 2)
            {
                plantStatus = PlantPhase.Phase3_FullyGrown;
                this.gameObject.GetComponent<SpriteRenderer>().sprite = GrowthSpriteList[phase];
                //PrepareForHarvest();
            }

        }
        
    }


    void PrepareForHarvest()
    {
        this.phase = 0;
        // enable particle
        
    }
}

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
    [SerializeField] InventoryItemData plantToGrow;

    public PlantPhase plantStatus = PlantPhase.Phase0_Default;
    [SerializeField] int phase = 0;
    public bool simulateGrowth = false;
    [SerializeField] bool isGrowing = false;

    [SerializeField] float growthDuration = 0;

    public float ticks = 0f;

    [Header("Plant Growth Sprite")]
    [SerializeField] List<Sprite> GrowthSpriteList;

    [Header("Plant Drops Sprite")]
    [SerializeField] GameObject DropA_sprt;
    [SerializeField] GameObject DropB_sprt;
    [SerializeField] GameObject DropObj;
    [SerializeField] Transform DropLoc;

    [Header("Particle Sprite")]
    [SerializeField]ParticleSystem particle;

    Animator animator;
    Soil soil;


    // Start is called before the first frame update
    void Start()
    {
        if (plantToGrow)
        {
            PlantSeed(plantToGrow);
        }

        soil = gameObject.GetComponentInParent<Soil>();

        animator = gameObject.GetComponent<Animator>();

        if (!animator)
        {
            Debug.Log("Cannot Find animator");
        }

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

    public void PlantSeed(InventoryItemData plantObj)
    {
        this.gameObject.transform.localPosition = gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, 0.5f, gameObject.transform.localPosition.z);
        Debug.Log("Updating Plant Stats");
        this.GrowthSpriteList = plantObj.PlantGrowthSpriteList;
        this.DropA_sprt = plantObj.DropA;
        this.DropB_sprt = plantObj.DropB;

        this.growthDuration = plantObj.growthDuration;

        this.phase = 0;

        this.gameObject.GetComponent<SpriteRenderer>().sprite = GrowthSpriteList[phase]; // seed
        plantStatus = PlantPhase.Phase1_Seedling;
    }

    public void EnablePlantGrowth()
        {
            Debug.Log("Start Growing");
            //ticks = 0;
            isGrowing = true;

            int nTransitions = GrowthSpriteList.Count - 1;


            StartCoroutine(NextPlantPhase(growthDuration / nTransitions));

        }

    IEnumerator NextPlantPhase(float timePerPhase)
    {


        for (int i = 0; i < GrowthSpriteList.Count - 1; i++)
        {
            yield return new WaitForSeconds(timePerPhase);
            phase++;
            Debug.Log(phase);


            if (phase == 1)
            {
                this.gameObject.transform.localPosition = gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, 0.80f, gameObject.transform.localPosition.z);
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
            animator.SetBool("isFullyGrown", true);

            // show and play particle
            particle.gameObject.SetActive(true);
            particle.Play();


        }
        else
        {
            Debug.Log("not found");

        }
        // enable particle

    }

    public void HarvestPlant()
    {
        this.gameObject.transform.localPosition = gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, 0.5f, gameObject.transform.localPosition.z);
        Debug.Log("Reset Plant Stats");

        InitizalizeDrops();
        this.DropA_sprt = null;
        this.DropB_sprt = null;

        this.growthDuration = 0;

        this.phase = 0;
        plantStatus = PlantPhase.Phase0_Default;
        animator.SetBool("isFullyGrown", false);

        // stop and hide particle
        particle.Stop();
        particle.gameObject.SetActive(true);



        //this.gameObject.GetComponent<SpriteRenderer>().sprite = GrowthSpriteList[phase]; // seed
        //plantStatus = PlantPhase.Phase1_Seedling;
    }

    public void InitizalizeDrops()
    {
        for (int i = 0; i < 2; i++)
        {
            GameObject DropObject = (GameObject)Instantiate(DropA_sprt, new Vector3(DropLoc.position.x, DropLoc.position.y, DropLoc.position.z), DropLoc.rotation);
            DropObject.gameObject.transform.LookAt(Camera.main.transform);

            Debug.Log("Drop items");
        }

        //bulletSphere.transform.LookAt(new Vector3(hit.point.x, 2.0f, hit.point.z));

        this.DropA_sprt = null;
        this.DropB_sprt = null;

        this.growthDuration = 0;

        this.phase = 0;
        plantStatus = PlantPhase.Phase0_Default;


        //this.gameObject.GetComponent<SpriteRenderer>().sprite = GrowthSpriteList[phase]; // seed
        //plantStatus = PlantPhase.Phase1_Seedling;
    }
}
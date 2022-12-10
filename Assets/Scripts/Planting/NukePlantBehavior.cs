using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NukePlantBehavior : MonoBehaviour
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

    public float ticks = 0f;

    [Header("Plant Growth Sprite")]
    [SerializeField] List<Sprite> GrowthSpriteList;

    [Header("Particle Sprite")]
    [SerializeField]ParticleSystem particle;

    Animator animator;
    Soil soil;

    [Header("Plant Stats")]
    private float currHealth = 0f;
    private float maxHealth = 500f;


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

        currHealth = maxHealth;
        GetComponentInChildren<NukeHPBar>().UpdateHPBar(currHealth, maxHealth);
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
        this.phase = 0;

        this.gameObject.GetComponent<SpriteRenderer>().sprite = GrowthSpriteList[phase]; // seed
        plantStatus = PlantPhase.Phase1_Seedling;
    }

    public void EnablePlantGrowth()
        {
            Debug.Log("Start Growing");
            //ticks = 0;
            isGrowing = true;


        }

    public void NextPlantPhase()
    {
        if (phase >= GrowthSpriteList.Count)
        {
            return;
            Debug.Log("Fully Grown");
        }
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

        this.phase = 0;
        plantStatus = PlantPhase.Phase0_Default;
        animator.SetBool("isFullyGrown", false);

        // stop and hide particle
        particle.Stop();
        particle.gameObject.SetActive(true);

    }

    public void InitizalizeDrops()
    {
       
    }

    public void ReceiveDamage(float damage)
    {
        // Subtract HP
        this.currHealth -= damage;
        Debug.Log("Nuke plant receive damage: " + currHealth + "   " + maxHealth);

        if (this.currHealth <= 0)
        {
            currHealth = 0;
            GameManager.instance.gameState = GameManager.GameState.Lose;
        }

        // Update HP Bar
        GetComponentInChildren<NukeHPBar>().UpdateHPBar(currHealth, maxHealth);
    }

    public void ResetHP()
    {
        this.currHealth = this.maxHealth;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soil : MonoBehaviour
{
    public  enum SoilStatus
    {
        Default,
        Tilled,
        Planted,
        Watered,
        Growing,
        ForHarvesting
    };

    [Header("Soil Properties")]
    public SoilStatus soilStatus = SoilStatus.Default;
    [SerializeField] private bool isOccupied = false;

    [Header ("Soil Materials")]
    [SerializeField] Material defaultSoil_MT;
    [SerializeField] Material tilledSoil_MT;
    [SerializeField] Material wateredSoil_MT;

    [Header("Plant Object")]
    [SerializeField] GameObject plant;
    [SerializeField]PlantBehavior plantBehavior;

    // Start is called before the first frame update
    void Start()
    {
        plantBehavior = plant.GetComponent<PlantBehavior>();
        if (plantBehavior == null)
        {
            Debug.Log("Plant Behavior Component NOT FOUND");
        }
        else
        {
            Debug.Log("behavior found");
        }

        plant.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // run timer
    }

    public void TillSoil()
    {
        // plow rocky soil and update status
        if(soilStatus == SoilStatus.Default) // can only plow rocky soil
        {
            Debug.Log("Tilled Soil");
            // change material
            this.gameObject.GetComponent<MeshRenderer>().material = tilledSoil_MT;
            soilStatus = SoilStatus.Tilled;
        }
        
    }

    public void PlantSeed(PlantObject plantObj)
    {
        // plant seeds on empty and plowed soil and update status

        if (isOccupied == false && soilStatus == SoilStatus.Tilled)
        {
            plantBehavior.UpdatePlantProperty(plantObj); // update plant comp in plant sprite

            Debug.Log("Seed Planted");
            soilStatus = SoilStatus.Planted;
            isOccupied = true;
            plant.SetActive(true); // show sprite

        }

    }

    public void WaterSoil()
    {
        if (soilStatus == SoilStatus.Tilled) // water tilled soil
        {
            Debug.Log("Watered Soil");
            //change color
            this.gameObject.GetComponent<MeshRenderer>().material = wateredSoil_MT;
            // start growth timer
        }
        else if (soilStatus == SoilStatus.Planted) // water tilled soil
        {
            Debug.Log("Watered Plant");
            //change color
            this.gameObject.GetComponent<MeshRenderer>().material = wateredSoil_MT;
            // start growth timer
            plantBehavior.EnablePlantGrowth();
        }
        else
        {
            Debug.Log("Soil needs to be plowed");
        }

    }

    //public void PlantSeed(PlantObject seed)
    //{
    //    plant.SetActive(true);
    //}

    public void HarvestPlant()
    {
        Debug.Log("Harvest Plant");
        // disable
        this.gameObject.GetComponent<MeshRenderer>().material = defaultSoil_MT;
    }

}

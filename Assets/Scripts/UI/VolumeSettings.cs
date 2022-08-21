using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Text text;
    [SerializeField] GameObject mute;
    [SerializeField] GameObject unmute;

    private Scene scene;

    void Start()
    {
        scene = SceneManager.GetActiveScene();
        if(scene.name == "Level 1 - Test")
        {
            text.text = (Mathf.Round(slider.value * 100.0f)).ToString();
        }

        Unmute();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = (Mathf.Round(slider.value * 100.0f)).ToString();
    }

    public void Mute()
    {
        mute.gameObject.GetComponent<Button>().interactable = false;
        unmute.gameObject.GetComponent<Button>().interactable = true;
    }

    public void Unmute()
    {
        mute.gameObject.GetComponent<Button>().interactable = true;
        unmute.gameObject.GetComponent<Button>().interactable = false;
    }
}

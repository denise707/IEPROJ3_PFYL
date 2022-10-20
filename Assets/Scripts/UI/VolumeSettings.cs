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

        slider.value = AudioManager.instance.GetVolume();
        if (AudioManager.instance.GetMute())
        {
            Mute();
        }
        else
        {
            Unmute();
        }
    }

    // Update is called once per frame
    void Update()
    {
        text.text = (Mathf.Round(AudioManager.instance.GetVolume() * 100.0f)).ToString();
        AudioManager.instance.ChangeVolume(slider.value);
    }

    public void Mute()
    {
        AudioManager.instance.Mute();
        mute.gameObject.GetComponent<Button>().interactable = false;
        unmute.gameObject.GetComponent<Button>().interactable = true;
        AudioManager.instance.SetMute(true);
    }

    public void Unmute()
    {
        AudioManager.instance.Unmute();
        mute.gameObject.GetComponent<Button>().interactable = true;
        unmute.gameObject.GetComponent<Button>().interactable = false;
        AudioManager.instance.SetMute(false);
    }
}

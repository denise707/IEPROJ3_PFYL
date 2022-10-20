using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private AudioSource source;
    private float sliderVolume = 0.5f;
    private bool mute = false;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(this);
        }
    }

    /*
        Plays a oneshot audio clip 
    */
    public void PlaySFX(AudioClip clip)
    {
        source.PlayOneShot(clip);
    }
    /*
        Plays a bgm
    */
    public void PlayBGM()
    {
        if (source.isPlaying)
        {
            //source.Stop();
            /*add bgm controls*/
        }
        source.Play();
    }

    public void ChangeBGMClip(AudioClip clip)
    {
        source.clip = clip;
    }

    public void Mute()
    {
        if (source.isPlaying)
        {
            source.mute = true;
        }
    }

    public void Unmute()
    {
        if (source.isPlaying)
        {
            source.mute = false;
        }
    }

    public void StopBGM()
    {
        if (source.isPlaying)
        {
            source.Stop();
        }
    }

    public void ChangeVolume(float volume)
    {
        sliderVolume = volume;
        source.volume = sliderVolume;
    }

    public float GetVolume()
    {
        return sliderVolume;
    }

    public bool GetMute()
    {
        return mute;
    }

    public void SetMute(bool mute)
    {
        this.mute = mute;
    }
}

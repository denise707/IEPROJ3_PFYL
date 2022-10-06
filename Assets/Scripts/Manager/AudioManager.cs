using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private AudioSource source;
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
            source.Stop();
            /*add bgm controls*/
        }
        source.Play();
    }

}

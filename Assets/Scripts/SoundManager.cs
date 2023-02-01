using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;

    [Header("Audio Sources")]
    [SerializeField] AudioSource Music;
    [SerializeField] AudioSource SoundEffects;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Music.loop = true;
    }

    public void PlayMusic(AudioClip clip)
    {
        Music.clip = clip;
        Music.Play();
    }

    public void PlaySoundEffect(AudioClip clip)
    {
        SoundEffects.clip = clip;
        SoundEffects.Play();
    }

    public void Pause()
    {
        Music.Pause();
        SoundEffects.Pause();
    }

    public void UnPause()
    {
        Music.UnPause();
        SoundEffects.UnPause();
    }
}
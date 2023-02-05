using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;

    [Header("Audio Sources")]
    [SerializeField]
    AudioSource Music;

    [SerializeField]
    AudioSource SoundEffects;

    [Header("Background")]
    public AudioClip arcade;
    public AudioClip sky;
    public AudioClip space;

    [Header("Sound Effects")]
    public AudioClip redBumper;
    public AudioClip greenBumper;
    public AudioClip blueBumper;
    public AudioClip paddleSwing;
    public AudioClip paddleHit;
    public AudioClip wallHit;
    public AudioClip spaceShipHit;
    public AudioClip asteroidBallHit;

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

    public void PlayBackground(AudioClip clip)
    {
        Music.clip = clip;
        Music.volume = 1f;

        if (clip == sky)
        {
            Music.volume = .2f;
        }
        Music.Play();
    }

    public void SwitchBackground(AudioClip clip)
    {
        Music.Stop();
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

    public void Stop()
    {
        Music.Stop();
        SoundEffects.Stop();
    }
}

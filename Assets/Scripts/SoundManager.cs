using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    //public static SoundManager instance = null;

    [Header("Audio Sources")]
    [SerializeField] AudioSource Music;
    [SerializeField] AudioSource SoundEffects;

    [Header("Background")]
    [SerializeField] AudioClip arcade;
    [SerializeField] AudioClip sky;
    [SerializeField] AudioClip space;
    [SerializeField] AudioClip IntroCutscene;
    [SerializeField] AudioClip Blastoff;

    [Header("Sound Effects")]
    [SerializeField] AudioClip asteroidBallHit;
    [SerializeField] AudioClip redBumper;
    [SerializeField] AudioClip greenBumper;
    [SerializeField] AudioClip blueBumper;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip menuClick;
    [SerializeField] AudioClip paddleBallHit;
    [SerializeField] AudioClip paddleBallRoll;
    [SerializeField] AudioClip paddleClick;
    [SerializeField] AudioClip paddleRelease;
    [SerializeField] AudioClip wallHit;
    [SerializeField] AudioClip planeFlyBy;
    [SerializeField] AudioClip spaceShipHit;

    //private void Awake()
    //{
    //    if (instance == null)
    //    {
    //        instance = this;
    //    }
    //    else if (instance != this)
    //    {
    //        Destroy(gameObject);
    //    }
    //    DontDestroyOnLoad(gameObject);
    //}

    private void Start()
    {
        Music.loop = true;
    }

    public void FXasteroidBallHit()
    {
        SoundEffects.PlayOneShot(asteroidBallHit);
    }

    public void FXRedBumper()
    {
        SoundEffects.PlayOneShot(redBumper);
    }

    public void FXGreenBumper()
    {
        SoundEffects.PlayOneShot(greenBumper);
    }

    public void FXBlueBumper()
    {
        SoundEffects.PlayOneShot(blueBumper);
    }

    public void FXDeath()
    {
        SoundEffects.PlayOneShot(death);
    }

    public void FXMenuClick()
    {
        SoundEffects.PlayOneShot(menuClick, 0.4f);
    }

    public void FXPaddleBallHit()
    {
        SoundEffects.PlayOneShot(paddleBallHit);
    }

    public void FXPaddleBallRoll() 
    {
        SoundEffects.PlayOneShot(paddleBallRoll);
    }

    public void FXPaddleClick()
    {
        SoundEffects.PlayOneShot(paddleClick);
    }

    public void FXPaddleRelease()
    {
        SoundEffects.PlayOneShot(paddleRelease);
    }

    public void FXWallHit()
    {
        SoundEffects.PlayOneShot(wallHit, 0.2f);
    }
    
    public void FXPlaneFlyBy()
    {
        SoundEffects.PlayOneShot(planeFlyBy);
    }

    public void FXSpaceShipHit()
    {
        SoundEffects.PlayOneShot(spaceShipHit);
    }

    public void BGArcade()
    {
        Music.clip = arcade;
        Music.volume = 1f;
        Music.Play();
    }

    public void BGSky()
    {
        Music.clip = sky;
        Music.volume = .2f;
        Music.Play();
    }

    public void BGSpace()
    {
        Music.clip = space;
        Music.volume = 1f;
        Music.Play();
    }

    public void BGCutscene()
    {
        Music.clip = IntroCutscene;
        Music.volume = 1f;
        Music.Play();
    }

    public void BGBlastOff()
    {
        Music.clip = Blastoff;
        Music.volume = 1f;
        Music.Play();
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

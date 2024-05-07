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
    [SerializeField] AudioSource Bumpers;
    [SerializeField] AudioSource BallRoll;

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
    [SerializeField] List<AudioClip> balloonHit;

    private float comboPitchMod = 1f;

    private void Start()
    {
        Music.loop = true;
    }

    enum pitch
    {
        c, d, e, f, g, a, b, oct
    }

    private pitch currPitch = pitch.c;

    public void SetVolume(float inVolume) {
        Music.volume = inVolume;
        SoundEffects.volume = inVolume;
        Bumpers.volume = inVolume;
        BallRoll.volume = inVolume;
    }

    public void FXComboPitchUp()
    {
        //Debug.Log(currPitch);

        // don't go higher than 1 octave
        if (currPitch == pitch.oct) return;

        if (currPitch == pitch.e || currPitch == pitch.b)
        {
            // Go up 1 semitone
            comboPitchMod *= Mathf.Pow(1.059463f, 1);
            currPitch++;
        }
        else
        {
            // Go up 2 semitones
            comboPitchMod *= Mathf.Pow(1.059463f, 2);
            currPitch++;
        }


        //Debug.Log("comboPitch: " + comboPitchMod);
    }

    public void FXComboPitchReset()
    {
        comboPitchMod = 1f;
        currPitch = pitch.c;
    }

    public void FXasteroidBallHit()
    {
        SoundEffects.PlayOneShot(asteroidBallHit);
    }

    public void FXRedBumper()
    {
        Bumpers.pitch = comboPitchMod;
        Bumpers.PlayOneShot(redBumper);
        //SoundEffects.pitch = 1;
    }

    public void FXGreenBumper()
    {
        Bumpers.pitch = comboPitchMod;
        Bumpers.PlayOneShot(greenBumper);
        //SoundEffects.pitch = 1;
    }

    public void FXBlueBumper()
    {
        Bumpers.pitch = comboPitchMod;
        Bumpers.PlayOneShot(blueBumper);
        //SoundEffects.pitch = 1;
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
        BallRoll.PlayOneShot(paddleBallRoll);
    }

    public void FXStopBallRoll()
    {
        BallRoll.Stop();
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

    public void FXBlastOff()
    {
        SoundEffects.PlayOneShot(Blastoff);
    }

    public void FXBalloon()
    {
        Bumpers.pitch = comboPitchMod;

        Bumpers.PlayOneShot(balloonHit[Random.Range(0, balloonHit.Count)], .5f);
    }

    public void BGArcade()
    {
        Music.clip = arcade;
        Music.volume = 1f;
        Music.loop = true;
        Music.Play();
    }

    public void BGSky()
    {
        Music.clip = sky;
        Music.volume = .2f;
        Music.loop = true;
        Music.Play();
    }

    public void BGSpace()
    {
        Music.clip = space;
        Music.volume = 1f;
        Music.loop = true;
        Music.Play();
    }

    public void BGCutscene()
    {
        Music.clip = IntroCutscene;
        Music.volume = 1f;
        Music.loop = false;
        Music.Play();
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

    public void FXStop()
    {
        SoundEffects.Stop();
        Bumpers.Stop();
    }
}

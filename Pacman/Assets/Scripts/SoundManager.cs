using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    public static SoundManager instance = null;

    public AudioClip eatingDots;
    public AudioClip eatingGhost;
    public AudioClip pacmanDies;
    public AudioClip powerUpEating;
    public AudioClip ghostMove;

    private AudioSource pacmanAudioSource;
    private AudioSource ghostAudioSource;
    private AudioSource oneShotAudioSource;
	// Use this for initialization
	void Start () {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this) {
            Destroy(gameObject);
        }

        AudioSource[] audioSources = GetComponents<AudioSource>();

        pacmanAudioSource = audioSources[0];
        ghostAudioSource = audioSources[1];
        oneShotAudioSource = audioSources[2];

        PlayClipOnLoop(pacmanAudioSource, eatingDots);
	}

    public void PlayOneShot(AudioClip clip) {
        oneShotAudioSource.PlayOneShot(clip);
    }

    public void PlayClipOnLoop(AudioSource aS, AudioClip clip) {
        if (aS != null && clip != null) {
            aS.loop = true;
            aS.volume = 0.2f;
            aS.clip = clip;
            aS.Play();
        }
    }

    public void PausePacman() {
        if (pacmanAudioSource != null && pacmanAudioSource.isPlaying) {
            pacmanAudioSource.Stop();
        }
    }

    public void ResumePacman() {
        if (pacmanAudioSource != null && !pacmanAudioSource.isPlaying)
        {
            pacmanAudioSource.Play();
        }
    }

    public void PauseGhost()
    {
        if (ghostAudioSource != null && ghostAudioSource.isPlaying)
        {
            ghostAudioSource.Stop();
        }
    }

    public void ResumeGhost()
    {
        if (ghostAudioSource != null && !ghostAudioSource.isPlaying)
        {
            ghostAudioSource.Play();
        }
    }
    // Update is called once per frame
    void Update () {
		
	}
}

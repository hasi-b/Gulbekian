using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] AudioSource bgMusicAudioSource;

    [SerializeField] AudioSource sfxAudioSource;
    [Space]
    [SerializeField] AudioClip bgMusicAudioClip;
    [Space]
    [SerializeField] AudioClip successAudioClip;
    [SerializeField] AudioClip failureAudioClip;
    [SerializeField] AudioClip photoclickAudioClip;
    [SerializeField] AudioClip photoSubmitButtonAudioClip;
    [SerializeField] AudioClip uiButtonAudioClip;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    public void PlaySuccessSound()
    {
        PlaySFXAudio(successAudioClip);
    }

    public void PlayFailureSound()
    {
        PlaySFXAudio(failureAudioClip);
    }
    public void PlayPhotoClickSound()
    {
        PlaySFXAudio(photoclickAudioClip);
    }
    public void PlayPhotoSubmitSound()
    {
        PlaySFXAudio(photoSubmitButtonAudioClip);
    }
    public void PlayUIButtonSound()
    {
        PlaySFXAudio(uiButtonAudioClip);
    }

    private void PlaySFXAudio(AudioClip audioClip)
    {
        sfxAudioSource.PlayOneShot(audioClip);
    }
}

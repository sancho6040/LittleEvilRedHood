using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    [Header("Audio Source")]
    public AudioClip backGround;
    public AudioClip death;
    public AudioClip damage;
    public AudioClip attack;

    private void Start()
    {
         musicSource.clip = backGround;
        musicSource.Play();
    }


    public void PlaySFX(AudioClip clip)
    {
       sfxSource.PlayOneShot(clip);
    }
}

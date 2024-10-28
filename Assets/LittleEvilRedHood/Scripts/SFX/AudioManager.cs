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
    public AudioClip playerhurt;
    public AudioClip playerattack;
    public AudioClip enemyhurt;
    public AudioClip enemyattack;

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

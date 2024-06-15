using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private AudioSource _source;

    public void SetMasterVolume(float volume)
    {
        _mixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
    }

    public void SetMusicVolume(float volume)
    {
        _mixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
    }
    public void SetSFXVolume(float volume)
    {
        _mixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
    }

    public void PlayAudio(AudioClip clip)
    {
        if (_source.clip == clip) return;

        _source.Stop();
        _source.clip = clip;
        _source.Play();
    }
}

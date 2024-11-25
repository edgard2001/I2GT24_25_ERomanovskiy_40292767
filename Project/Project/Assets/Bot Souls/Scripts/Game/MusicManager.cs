using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    private static MusicManager _instance;
    public static MusicManager Instance => _instance;
    
    private static AudioSource _audio;
    
    [SerializeField] private AudioClip[] menuMusic;
    [SerializeField] private AudioClip[] ambientMusic;
    [SerializeField] private AudioClip[] combatMusic;
    
    void Awake()
    {
        if (_instance != null)
            Destroy(gameObject);
        else 
        {
            _instance = this;
            _audio = GetComponent<AudioSource>();
            _audio.loop = true;
            DontDestroyOnLoad(gameObject);
        }
        
    }
    
    public void PlayMusic(MusicType type, int index) 
    {
        _audio.Stop();
        switch (type)
        {
            case MusicType.Menu:
                _audio.clip = menuMusic[index];
                _audio.volume = 0.03f;
                break;
            case MusicType.Ambient:
                _audio.clip = ambientMusic[index];
                _audio.volume = 0.02f;
                break;
            case MusicType.Combat:
                _audio.clip = combatMusic[index];
                _audio.volume = 0.03f;
                break;
        }
        _audio.Play();
    }
    
    public enum MusicType
    {
        Menu,
        Ambient,
        Combat
    }
    
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour, IGameManager {
    public ManagerStatus status {get; private set;}
    public float crossFadeRate = 1.5f;
    [SerializeField] private AudioSource soundSource;
    [SerializeField] private AudioSource music1Source;
    [SerializeField] private AudioSource music2Source;
    [SerializeField] private string introBGMusic;
    [SerializeField] private string levelBGMusic;
    private NetworkService _network;
    private AudioSource _activeMusic;
    private AudioSource _inactiveMusic;
    private bool _crossFading;
    
    public float soundVolume {
        //use global values on AudioListener
        get {return AudioListener.volume;}
        set {AudioListener.volume = value;}
    }

    public bool soundMute {
        //use global values on AudioListener
        get {return AudioListener.pause;}
        set {AudioListener.pause = value;}
    }

    private float _musicVolume;
    public float musicVolume {
        get {return _musicVolume;}
        set {
            _musicVolume = value;
            if (music1Source != null && !_crossFading) {
                //music1Source ignores Listener's volume, but it can be adjusted directly
                music1Source.volume = _musicVolume;
                music2Source.volume = _musicVolume;
            }
        }
    }

    public bool musicMute {
        get {
            if (music1Source != null) {
                //music1Source ignores Listener's pause, but it can be adjusted directly
                return music1Source.mute;
            }
            return false;
        }
        set {
            if (music1Source != null) {
                music1Source.mute = value;
                music2Source.mute = value;
            }
        }
    }

    public void PlaySound(AudioClip clip) {
        soundSource.PlayOneShot(clip);
    }

    public void PlayIntroMusic() {
        PlayMusic(Resources.Load("Music/" + introBGMusic) as AudioClip);
    }

    public void PlayLevelMusic() {
        PlayMusic(Resources.Load("Music/" + levelBGMusic) as AudioClip);
    }

    private void PlayMusic(AudioClip clip) {
        if (_crossFading) {return;}
        StartCoroutine(CrossFadeMusic(clip));
    }

    public void StopMusic() {
        _activeMusic.Stop();
        _inactiveMusic.Stop();
    }

    public void Startup(NetworkService service) {
        Debug.Log("Audio manager starting...");
        _network = service;

        //tell music1Source to ingore the AudioListener volume
        music1Source.ignoreListenerVolume = true;
        music1Source.ignoreListenerPause = true;
        music2Source.ignoreListenerVolume = true;
        music2Source.ignoreListenerPause = true;

        soundVolume = 1f;
        musicVolume = 1f;

        //_active is the one to be faded out
        _activeMusic = music1Source;
        _inactiveMusic = music2Source;
        
        status = ManagerStatus.Started;
    }

    private IEnumerator CrossFadeMusic(AudioClip clip) {
        _crossFading = true;

        _inactiveMusic.clip = clip;
        _inactiveMusic.volume = 0;
        _inactiveMusic.Play();

        float scaledRate = crossFadeRate * _musicVolume;
        while (_activeMusic.volume > 0) {
            _activeMusic.volume -= scaledRate * Time.deltaTime;
            _inactiveMusic.volume += scaledRate * Time.deltaTime;

            //pause for one frame
            yield return null;
        }

        //Swapping _active and _inactive
        AudioSource temp = _activeMusic;
        _activeMusic = _inactiveMusic;
        _activeMusic.volume = _musicVolume;
        _inactiveMusic = temp;
        _inactiveMusic.Stop();

        _crossFading = false;
    }

}

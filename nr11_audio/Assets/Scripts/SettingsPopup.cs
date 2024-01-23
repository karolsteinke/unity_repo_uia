using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsPopup : MonoBehaviour
{
    [SerializeField] private AudioClip click;
    
    public void OnSoundToggle() {
        Managers.Audio.soundMute = !Managers.Audio.soundMute;
        Managers.Audio.PlaySound(click);
    }

    public void OnSoundValue(float volume) {
        Managers.Audio.soundVolume = volume;
    }

    public void OnMusicToggle() {
        Managers.Audio.musicMute = !Managers.Audio.musicMute;
        Managers.Audio.PlaySound(click);
    }

    public void OnMusicValue(float volume) {
        Managers.Audio.musicVolume = volume;
    }

    public void OnPlayMusic(int selector) {
        Managers.Audio.PlaySound(click);

        switch (selector) {
        case 1:
            Managers.Audio.PlayIntroMusic();
            break;
        case 2:
            Managers.Audio.PlayLevelMusic();
            break;
        default:
            Managers.Audio.StopMusic();
            break;
        }
    }
}

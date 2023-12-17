using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private const string PLAYER_REFS_MUSIC_VOLUME = "MusicVolume";


    public static MusicManager Instance { get; private set; }

    private AudioSource auidioSource;
    private float volume = .3f;

    private void Awake() {
        Instance = this;
        auidioSource = GetComponent<AudioSource>();

        volume = PlayerPrefs.GetFloat(PLAYER_REFS_MUSIC_VOLUME, .3f);
        auidioSource.volume = volume;
    }

    public void ChangeVolume() {
        volume += .1f;
        if (volume > 1f) {
            volume = 0f;
        }

        auidioSource.volume = volume;
        PlayerPrefs.SetFloat(PLAYER_REFS_MUSIC_VOLUME, volume);
    }

    public float GetVolume() {
        return volume;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSystem : MonoBehaviour
{
    private AudioSource musicSource;
    private float volume;
    [SerializeField] private AudioClip[] musicClip;

    // Start is called before the first frame update
    void Start()
    {
        musicSource = GetComponent<AudioSource>();
        if (PlayerPrefs.GetFloat("MusicVolume") < 0)
        {
            PlayerPrefs.SetFloat("MusicVolume", 1);
        }
        volume = PlayerPrefs.GetFloat("MusicVolume");
        
    }

    // Update is called once per frame
    void Update()
    {
        volume = PlayerPrefs.GetFloat("MusicVolume");
        musicSource.volume = volume;
        if (!musicSource.isPlaying)
        {
            GetMusicPlay();
        }
    }

    void GetMusicPlay()
    {
        int index = Random.Range(0, musicClip.Length);
        musicSource.clip = musicClip[index];
        musicSource.Play();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public List<AudioClip> musicClips;
    private AudioSource asMusic;
    private int currentClipIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        asMusic = GetComponent<AudioSource>();
        PlayRandomMusic();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleMute();
        }
        else if (Input.GetKeyDown(KeyCode.Period))
        {
            SkipToNextSong();
        }
        else if (Input.GetKeyDown(KeyCode.Comma))
        {
            SkipToPreviousSong();
        }
    }
    void SkipToNextSong()
    {
        currentClipIndex++;
        if (currentClipIndex >= musicClips.Count)
        {
            currentClipIndex = 0;
        }
        asMusic.Stop();
        PlayCurrentMusic();
    }
    void SkipToPreviousSong()
    {
        currentClipIndex--;
        if (currentClipIndex < 0)
        {
            currentClipIndex = musicClips.Count - 1;
        }
        asMusic.Stop();
        PlayCurrentMusic();
    }
    void PlayCurrentMusic()
    {
        if (musicClips.Count > 0)
        {
            asMusic.clip = musicClips[currentClipIndex];
            asMusic.Play();
        }
        else
        {
            Debug.LogError("No music clips added to the list!");
        }
    }

    void ToggleMute()
    {
        asMusic.mute = !asMusic.mute;
    }
    void PlayRandomMusic()
    {
        if (musicClips.Count > 0)
        {
            currentClipIndex = Random.Range(0, musicClips.Count);

            AudioClip randomClip = musicClips[currentClipIndex];

            asMusic.clip = randomClip;
            asMusic.Play();
        }
        else
        {
            Debug.LogError("No music clips added to the list!");
        }
    }
}

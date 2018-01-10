using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

    private static AudioSource Player;
    private static MusicManager Instance;
    private static int CurrentIndex;
    public AudioClip[] Clips;

	void Start () {
        Player = GetComponent<AudioSource>();
        Instance = this;
        Player.volume = PlayerPrefs.GetFloat("MusicVolume", 0.2f);
        if (PlayerPrefs.HasKey("MusicTrackIndex"))
            PlayTrack(PlayerPrefs.GetInt("MusicTrackIndex"));
        else
            PlayTrack(11);
            //PlayRandomTrack();
	}
	
	public static void VolumeUp()
    {
        Debug.Assert(Player != null);
        Player.volume += 0.1f;
        PlayerPrefs.SetFloat("MusicVolume", Player.volume);
        LogController.Log("Volume: " + (int)(Player.volume * 100f)+"%");
    }

    public static void VolumeDown()
    {
        Debug.Assert(Player != null);
        Player.volume -= 0.1f;
        PlayerPrefs.SetFloat("MusicVolume", Player.volume);
        LogController.Log("Volume: " + (int)(Player.volume * 100f)+"%");
    }

    public static void Pause()
    {
       Player.Pause();
       PlayerPrefs.SetInt("MusicPaused", 1);
       LogController.Log("Music Paused");
    }

    public static void UnPause()
    {
        Player.Play();
        PlayerPrefs.SetInt("MusicPaused", 0);
        LogController.Log("Playing " + (CurrentIndex + 1) + ": " + Instance.Clips[CurrentIndex].name);
    }

    public static void TogglePause()
    {
        if (PlayerPrefs.GetInt("MusicPaused", 0) == 1)
            UnPause();
        else
            Pause();
    }

    public static void PlayRandomTrack()
    {
        Debug.Assert(Instance != null);
        CurrentIndex = Random.Range(0, Instance.Clips.Length);
        Player.clip = Instance.Clips[CurrentIndex] as AudioClip;
        Player.Play();
        PlayerPrefs.SetInt("MusicTrackIndex", CurrentIndex);
        LogController.Log("Playing " + (CurrentIndex + 1) + ": " + Instance.Clips[CurrentIndex].name);
        if (PlayerPrefs.GetInt("MusicPaused", 0) == 0)
            Player.Play();
    }

    public static void PlayTrack(int number = 0)
    {
        Debug.Assert(Instance != null);
        Debug.Assert(Instance.Clips.Length != 0);
        if (number >= Instance.Clips.Length)
            number = number - Instance.Clips.Length;
        if (number < 0)
            number = 0;

        CurrentIndex = number;
        PlayerPrefs.SetInt("MusicTrackIndex", CurrentIndex);
        Player.clip = Instance.Clips[CurrentIndex];
        LogController.Log("Playing " + (CurrentIndex + 1) + ": " + Instance.Clips[CurrentIndex].name);
        if (PlayerPrefs.GetInt("MusicPaused",0) == 0)
            Player.Play();

    }

    public static void PlayNextTrack()
    {
        Debug.Assert(Instance != null);
        if (++CurrentIndex < Instance.Clips.Length)
        {
            Player.clip = Instance.Clips[CurrentIndex];
        }
        else
        {
            Player.clip = Instance.Clips[0];
            CurrentIndex = 0;
        }
        PlayerPrefs.SetInt("MusicTrackIndex", CurrentIndex);
        LogController.Log("Playing " + (CurrentIndex + 1) + ": " + Instance.Clips[CurrentIndex].name);
        if (PlayerPrefs.GetInt("MusicPaused", 0) == 0)
            Player.Play();
    }

    public static void PlayPreviousTrack()
    {
        Debug.Assert(Instance != null);
        if (--CurrentIndex >= 0)
        {
            Player.clip = Instance.Clips[CurrentIndex];
        }
        else
        {
            Player.clip = Instance.Clips[Instance.Clips.Length-1];
            CurrentIndex = Instance.Clips.Length-1;
        }
        PlayerPrefs.SetInt("MusicTrackIndex", CurrentIndex);
        LogController.Log("Playing " + (CurrentIndex+1) + ": " + Instance.Clips[CurrentIndex].name);
        if (PlayerPrefs.GetInt("MusicPaused", 0) == 0)
            Player.Play();
    }
}

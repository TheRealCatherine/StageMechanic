using UnityEngine;

public class MusicManager : MonoBehaviour {

    public AudioSource Player;
    public AudioClip[] Clips;

    private static MusicManager Instance;
    private static int CurrentIndex;

    void Start () {
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
        Debug.Assert(Instance.Player != null);
        Instance.Player.volume += 0.1f;
        PlayerPrefs.SetFloat("MusicVolume", Instance.Player.volume);
        LogController.Log("Volume: " + (int)(Instance.Player.volume * 100f)+"%");
    }

    public static void VolumeDown()
    {
        Debug.Assert(Instance.Player != null);
        Instance.Player.volume -= 0.1f;
        PlayerPrefs.SetFloat("MusicVolume", Instance.Player.volume);
        LogController.Log("Volume: " + (int)(Instance.Player.volume * 100f)+"%");
    }

    public static void Pause()
    {
        Instance.Player.Pause();
       PlayerPrefs.SetInt("MusicPaused", 1);
       LogController.Log("Music Paused");
    }

    public static void UnPause()
    {
        Instance.Player.Play();
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
        Instance.Player.clip = Instance.Clips[CurrentIndex] as AudioClip;
        Instance.Player.Play();
        PlayerPrefs.SetInt("MusicTrackIndex", CurrentIndex);
        LogController.Log("Playing " + (CurrentIndex + 1) + ": " + Instance.Clips[CurrentIndex].name);
        if (PlayerPrefs.GetInt("MusicPaused", 0) == 0)
            Instance.Player.Play();
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
        Instance.Player.clip = Instance.Clips[CurrentIndex];
        LogController.Log("Playing " + (CurrentIndex + 1) + ": " + Instance.Clips[CurrentIndex].name);
        if (PlayerPrefs.GetInt("MusicPaused",0) == 0)
            Instance.Player.Play();

    }

    public static void PlayNextTrack()
    {
        Debug.Assert(Instance != null);
        if (++CurrentIndex < Instance.Clips.Length)
        {
            Instance.Player.clip = Instance.Clips[CurrentIndex];
        }
        else
        {
            Instance.Player.clip = Instance.Clips[0];
            CurrentIndex = 0;
        }
        PlayerPrefs.SetInt("MusicTrackIndex", CurrentIndex);
        LogController.Log("Playing " + (CurrentIndex + 1) + ": " + Instance.Clips[CurrentIndex].name);
        if (PlayerPrefs.GetInt("MusicPaused", 0) == 0)
            Instance.Player.Play();
    }

    public static void PlayPreviousTrack()
    {
        Debug.Assert(Instance != null);
        if (--CurrentIndex >= 0)
        {
            Instance.Player.clip = Instance.Clips[CurrentIndex];
        }
        else
        {
            Instance.Player.clip = Instance.Clips[Instance.Clips.Length-1];
            CurrentIndex = Instance.Clips.Length-1;
        }
        PlayerPrefs.SetInt("MusicTrackIndex", CurrentIndex);
        LogController.Log("Playing " + (CurrentIndex+1) + ": " + Instance.Clips[CurrentIndex].name);
        if (PlayerPrefs.GetInt("MusicPaused", 0) == 0)
            Instance.Player.Play();
    }
}

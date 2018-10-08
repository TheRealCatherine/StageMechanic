using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SongTitlePopulator : MonoBehaviour
{
    void Update()
    {
		if(MusicManager.IsPaused())
			GetComponent<Text>().text = "Paused";
		else
			GetComponent<Text>().text = MusicManager.TrackName();
    }
}

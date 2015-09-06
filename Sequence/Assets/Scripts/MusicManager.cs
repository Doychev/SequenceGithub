using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

	public AudioClip newMusic;

	private bool music = true;
	private bool sfx = true;

	// Use this for initialization
	void Start () {
		music = PlayerPrefs.GetInt ("music") == 1;
		music = !music;
		alterMusicVolume ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void alterMusicVolume () {
		music = !music;
		GameObject go = GameObject.Find("MusicPlayer");
		if (music) {
			go.audio.volume = 1;
			PlayerPrefs.SetInt ("music", 1);
		} else {
			go.audio.volume = 0;
			PlayerPrefs.SetInt ("music", 0);
		}
	}

	public void alterSfxVolume () {
		sfx = !sfx;
		AudioSource go = GameObject.Find("ButtonsHolder").GetComponent<AudioSource>();
		if (sfx) {
			go.volume = 1;
			PlayerPrefs.SetInt ("sfx", 1);
		} else {
			go.volume = 0;
			PlayerPrefs.SetInt ("sfx", 0);
		}
	}
}

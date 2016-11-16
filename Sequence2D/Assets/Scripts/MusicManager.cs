using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

	private int music;

    private static MusicManager instance = null;
    public static MusicManager Instance
    {
        get { return instance; }
    }
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    //Use this for initialization
    void Start () {
		music = PlayerPrefs.GetInt ("music", 1);
        music = (music + 1) % 2;
		alterMusicVolume ();
	}

	public void alterMusicVolume () {
        music = (music + 1) % 2;
		if (music == 1) {
            instance.GetComponent<AudioSource>().volume = 1;
			PlayerPrefs.SetInt ("music", 1);
		} else {
            instance.GetComponent<AudioSource>().volume = 0;
			PlayerPrefs.SetInt ("music", 0);
		}
	}
}

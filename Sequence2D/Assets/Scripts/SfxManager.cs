using UnityEngine;
using System.Collections;

public class SfxManager : MonoBehaviour {

    public AudioClip failSound, levelSound, successSound;

    private int sfx;

    private static SfxManager instance = null;
    public static SfxManager Instance
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
		sfx = PlayerPrefs.GetInt ("sfx", 1);
        sfx = (sfx + 1) % 2;
		alterSfxVolume ();
	}

	public void alterSfxVolume () {
        sfx = (sfx + 1) % 2;
		if (sfx == 1) {
            instance.GetComponent<AudioSource>().volume = 1;
			PlayerPrefs.SetInt ("sfx", 1);
		} else {
            instance.GetComponent<AudioSource>().volume = 0;
			PlayerPrefs.SetInt ("sfx", 0);
		}
	}

    public void playFailSound()
    {
        if (failSound)
        {
            GetComponent<AudioSource>().PlayOneShot(failSound);
        }
    }

    public void playLevelSound()
    {
        if (levelSound)
        {
            GetComponent<AudioSource>().PlayOneShot(levelSound);
        }
    }

    public void playSuccessSound()
    {
        if (successSound)
        {
            GetComponent<AudioSource>().PlayOneShot(successSound);
        }
    }
}

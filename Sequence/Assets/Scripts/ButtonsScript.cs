using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using GoogleMobileAds.Api;

public class ButtonsScript : MonoBehaviour {

	private GameObject[] buttons;
	private ArrayList sequence;
	private int currentDigit = 0;
	private bool playing = false;
	private float flashTime = 0.35f;
	private float pauseTime = 0.08f;
	private int score = 0;
	private bool restarting = true;
	private bool gameOver = false;
	private int gamesPlayed;
	private InterstitialAd interstitial;
	private string interstitialId = "ca-app-pub-2205037698598396/2750780076";
	private bool sfx = true;
	private bool tutorial = true;
	private bool tutorialShowed;

	public int levelDifficulty;
	public GameObject levelLight;
	public GameObject scoreText;
	public AudioClip successSound;
	public AudioClip failSound;
	public AudioClip levelSound;

	public bool getPlaying() {
		return playing;
	}

	// Use this for initialization
	void Start () {
		interstitial = new InterstitialAd(interstitialId);
		interstitial.LoadAd(new AdRequest.Builder().AddKeyword("unity").Build());
		gamesPlayed = PlayerPrefs.GetInt ("gamesPlayed");
		buttons = GameObject.FindGameObjectsWithTag ("GameButton");
		string[] buttonNames = new string[9];
		int count = 0;
		foreach (GameObject buttonTemp in buttons) {
			buttonNames[count] = buttonTemp.name;
			count++;
		}
		System.Array.Sort (buttonNames, buttons);
		sfx = PlayerPrefs.GetInt ("sfx") == 1;
		sfx = !sfx;
		GameObject.Find ("MusicManager").GetComponent<MusicManager> ().alterSfxVolume ();

		if (PlayerPrefs.GetInt ("tutorial") != 0) {
			tutorial = PlayerPrefs.GetInt ("tutorial") == 1;
		}
		if (tutorial) {
			tutorial = false;
			tutorialShowed = true;
			PlayerPrefs.SetInt("tutorial", 2);
			GameObject.Find("tutorialElements").SetActive(true);
		} else {
			GameObject.Find("tutorialElements").SetActive(false);
		}
		StartCoroutine(StartGame ());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void restartLevel() {
		if (playing || gameOver) {
			gameOver = false;
			currentDigit = 0;
			levelDifficulty = 2;
			playing = false;
			score = 0;
			scoreText.GetComponent<Text>().text = "Score: 0";
			foreach (GameObject button in buttons) {
				button.GetComponent<Renderer> ().material.color = Color.white;
				button.GetComponent<Light> ().enabled = false;
			}
			restarting = true;
			StartCoroutine(StartGame ());
		}
	}

	public IEnumerator StartGame() {
		while (tutorialShowed) {
			yield return new WaitForSeconds(0.25f);
		}
		playing = false;
		if (restarting) {
			Light light = levelLight.GetComponent<Light> ();
			light.color = Color.red;
			yield return new WaitForSeconds(0.25f);
			light.color = Color.yellow;
			yield return new WaitForSeconds(0.10f);
			light.color = Color.red;
			yield return new WaitForSeconds(0.25f);
			light.color = Color.yellow;
			restarting = false;
		}
		sequence = new ArrayList();
		for (int i = 0; i < levelDifficulty; i++) {
			int number = Mathf.FloorToInt (Random.Range (1, 9));
			sequence.Add(number);
		}
		yield return new WaitForSeconds(0.75f);
		for (int i = 0; i < sequence.Count; i++) {
			yield return flashButton(buttons[(int) sequence[i]], flashTime, Color.yellow);
			if (i != sequence.Count - 1) {
				yield return resetButton(buttons[(int) sequence[i]], pauseTime);
			} else {
				yield return resetButton(buttons[(int) sequence[i]], 0f);
			}
		}
		playing = true;
	}

	public WaitForSeconds flashButton(GameObject button, float time, Color color) {
		button.GetComponent<Renderer> ().material.color = color;
		button.GetComponent<Light> ().color = color;
		button.GetComponent<Light> ().enabled = true;
		return new WaitForSeconds(time);
	}

	public WaitForSeconds resetButton(GameObject button, float time) {
		button.GetComponent<Renderer> ().material.color = Color.white;
		button.GetComponent<Light> ().enabled = false;
		return new WaitForSeconds(time);
	}

	public IEnumerator checkInput(int buttonNumber) {
		playing = false;
		GameObject button = buttons [buttonNumber-1];

		if (sequence [currentDigit].Equals (buttonNumber-1)) {
			if (successSound) {
				audio.PlayOneShot (successSound);
			}
			currentDigit++;
			score++;
			scoreText.GetComponent<Text>().text = "Score: " + score;
			yield return flashButton(button, 0.15f, Color.green);
			yield return resetButton(button, 0.02f);
			if (currentDigit >= sequence.Count) {
				if (levelSound) {
					audio.PlayOneShot (levelSound);
				}
				unlockScoreAchievements(levelDifficulty);
				Light light = levelLight.GetComponent<Light> ();
				light.color = Color.green;
				yield return new WaitForSeconds(0.25f);
				light.color = Color.yellow;
				yield return new WaitForSeconds(0.10f);
				light.color = Color.green;
				yield return new WaitForSeconds(0.25f);
				light.color = Color.yellow;
				currentDigit = 0;
				levelDifficulty++;
				StartCoroutine(StartGame());
			} else {
				playing = true;
			}
		} else {
			if (failSound) {
				audio.PlayOneShot (failSound);
			}
			gameOver = true;
			gamesPlayed++;
			PlayerPrefs.SetInt("gamesPlayed", gamesPlayed);
			PlayerPrefs.Save();
			unlockGameCountAchievements(gamesPlayed);
			yield return flashButton(button, flashTime, Color.red);
			buttons[(int) sequence [currentDigit]].GetComponent<Renderer> ().material.color = Color.green;
			yield return resetButton(button, pauseTime);
			Light light = levelLight.GetComponent<Light> ();
			light.color = Color.red;
			yield return new WaitForSeconds(0.25f);
			light.color = Color.yellow;
			yield return new WaitForSeconds(0.10f);
			light.color = Color.red;
			yield return new WaitForSeconds(0.25f);
			light.color = Color.yellow;
			yield return new WaitForSeconds(0.10f);
			light.color = Color.red;
			buttons[(int) sequence [currentDigit]].GetComponent<Renderer> ().material.color = Color.white;
			currentDigit = 0;
			Social.ReportScore(score, "CgkIpLHTydocEAIQBg", (bool success) => {
				// handle success or failure
			});
			if (Random.Range (1, 100) > 60) {
				if (interstitial.IsLoaded()) {
					interstitial.Show();
					interstitial = new InterstitialAd(interstitialId);
					interstitial.LoadAd(new AdRequest.Builder().AddKeyword("unity").Build());
				}		
			}
		}
	}

	void unlockGameCountAchievements(int result) {
		string achievement = "";
		switch (result) {
		case 1:
			achievement =  "CgkIpLHTydocEAIQBw";
			break;
		case 10:
			achievement =  "CgkIpLHTydocEAIQCA";
			break;
		case 50:
			achievement =  "CgkIpLHTydocEAIQCQ";
			break;
		case 100:
			achievement =  "CgkIpLHTydocEAIQCg";
			break;
		case 1000:
			achievement =  "CgkIpLHTydocEAIQCw";
			break;
		default:
			break;
		}
		if (achievement.Length > 1) {
			Social.ReportProgress(achievement, 100.0f, (bool success) => {
				// handle success or failure
			});
		}
	}

	void unlockScoreAchievements(int result) {
		result--;
		string achievement = "";
		switch (result) {
		case 1:
			achievement =  "CgkIpLHTydocEAIQAQ";
			break;
		case 3:
			achievement =  "CgkIpLHTydocEAIQAg";
			break;
		case 6:
			achievement =  "CgkIpLHTydocEAIQAw";
			break;
		case 10:
			achievement =  "CgkIpLHTydocEAIQBA";
			break;
		case 30:
			achievement =  "CgkIpLHTydocEAIQBQ";
			break;
		default:
			break;
		}
		if (achievement.Length > 1) {
			Social.ReportProgress(achievement, 100.0f, (bool success) => {
				// handle success or failure
			});
		}
	}

	public void switchTutorialButton() {
		tutorial = !tutorial;
		if (tutorial) {
			PlayerPrefs.SetInt("tutorial", 1);
			GameObject.Find ("showAgainCheckbox").GetComponent<Button>().image.color = Color.green;
		} else {
			PlayerPrefs.SetInt("tutorial", 2);
			GameObject.Find ("showAgainCheckbox").GetComponent<Button>().image.color = Color.red;
		}
	}

	public void closeTutorial() {
		tutorialShowed = false;
		GameObject.Find("tutorialElements").SetActive(false);
	}
}

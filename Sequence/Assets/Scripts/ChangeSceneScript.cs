using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using GooglePlayGames;

public class ChangeSceneScript : MonoBehaviour {

	void Start () {
		Screen.orientation = ScreenOrientation.Portrait;
		DontDestroyOnLoad(this.GetComponent<AudioSource>());
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void changeScene() {
		Application.LoadLevel ("mainScene");
	}
	
	public void changeSceneToMenu() {
		Application.LoadLevel ("menuScene");
	}
	
	public void showLeaderboard() {
		Social.ShowLeaderboardUI();
	}
	
	public void showAcheivements() {
		Social.ShowAchievementsUI();
	}
}

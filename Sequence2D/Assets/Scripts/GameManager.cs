using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void StartGame()
    {
        SceneManager.LoadScene("game");
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("menu");
    }

    public void ShowLeaderboard()
    {
        GameObject.Find("SocialManager").GetComponent<SocialManager>().showLeaderboards();
    }

    public void ShowAchievements()
    {
        GameObject.Find("SocialManager").GetComponent<SocialManager>().showAchievements();
    }

    public void Share()
    {
        GameObject.Find("SocialManager").GetComponent<ShareCreator>().share();
    }



}

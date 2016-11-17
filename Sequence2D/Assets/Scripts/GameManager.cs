using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    private GameObject navigation, modeSelection;

	// Use this for initialization
	void Start () {
        if (SceneManager.GetActiveScene().name.Equals("menu"))
        {
            navigation = GameObject.Find("Navigation");
            modeSelection = GameObject.Find("Mode Selection");
            modeSelection.SetActive(false);
        }
    }

    public void SelectMode()
    {
        navigation.SetActive(false);
        modeSelection.SetActive(true);
    }

    public void BackToMain()
    {
        navigation.SetActive(true);
        modeSelection.SetActive(false);
    }

    public void StartGame(int mode)
    {
        ModeManager.GameMode gameMode = ModeManager.GameMode.CLASSIC;
        if (mode == 2)
        {
            gameMode = ModeManager.GameMode.TEMPO;
        }
        else if (mode == 3)
        {
            gameMode = ModeManager.GameMode.WILD;
        }
        GameObject.Find("ModeManager").GetComponent<ModeManager>().selectMode(gameMode);
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

    public void Rate()
    {
        GameObject.Find("SocialManager").GetComponent<SocialManager>().rateApp();
    }
}

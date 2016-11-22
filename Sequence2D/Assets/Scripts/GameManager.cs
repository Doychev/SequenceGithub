using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    private GameObject navigation, modeSelection;
    private GameObject menuTutorialPanel, classicExplanations, tempoExplanations, wildExplanations;
    private GameObject classicButton, tempoButton, wildButton;
    private bool showMenuTutorial;

    private int currentTutorialScreen = 0;

	// Use this for initialization
	void Start () {
        if (SceneManager.GetActiveScene().name.Equals("menu"))
        {
            navigation = GameObject.Find("Navigation");
            modeSelection = GameObject.Find("Mode Selection");
            menuTutorialPanel = GameObject.Find("Menu Tutorial");
            classicExplanations = GameObject.Find("Classic Explanations");
            tempoExplanations = GameObject.Find("Tempo Explanations");
            wildExplanations = GameObject.Find("Wild Explanations");
            classicButton = GameObject.Find("Classic");
            tempoButton = GameObject.Find("Tempo");
            wildButton = GameObject.Find("Wild");
            showMenuTutorial = PlayerPrefs.GetInt("showMenuTutorial", 1) == 1;

            modeSelection.SetActive(false);
            menuTutorialPanel.SetActive(false);
            classicExplanations.SetActive(false);
            tempoExplanations.SetActive(false);
            wildExplanations.SetActive(false);
        }
    }

    public void SelectMode()
    {
        navigation.SetActive(false);
        modeSelection.SetActive(true);

        if (showMenuTutorial)
        {
            menuTutorialPanel.SetActive(true);
        }
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

    public void closeMenuTutorial()
    {
        classicButton.SetActive(true);
        tempoButton.SetActive(true);
        wildButton.SetActive(true);
        menuTutorialPanel.SetActive(false);
        PlayerPrefs.SetInt("showMenuTutorial", 0);
    }

    public void showNextTutorialScreen()
    {
        currentTutorialScreen++;
        switch (currentTutorialScreen)
        {
            case 1:
                GameObject.Find("Tutorial Choice").SetActive(false);
                classicButton.SetActive(true);
                classicExplanations.SetActive(true);
                tempoButton.SetActive(false);
                tempoExplanations.SetActive(false);
                wildButton.SetActive(false);
                wildExplanations.SetActive(false);
                break;
            case 2:
                classicButton.SetActive(false);
                classicExplanations.SetActive(false);
                tempoButton.SetActive(false);
                tempoExplanations.SetActive(false);
                wildButton.SetActive(true);
                wildExplanations.SetActive(true);
                break;
            case 3:
                classicButton.SetActive(false);
                classicExplanations.SetActive(false);
                tempoButton.SetActive(true);
                tempoExplanations.SetActive(true);
                wildButton.SetActive(false);
                wildExplanations.SetActive(false);
                GameObject.Find("Next Text").GetComponent<Text>().text = "Finish";
                break;
            default:
                closeMenuTutorial();
                break;
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using System.Collections;
using UnityEngine.EventSystems;

public class GameplayManager : MonoBehaviour {

    public GameObject[] buttons, buttonOverlays;
    public GameObject levelText, scoreText, announcementsText;
    public GameObject savingPanel, tutorialPanel, tutorialChoice, tutorialHighlights, tutorialEnd;

    private int levelDifficulty;
    private int currentDigit;
    private int currentScore = 0;
    private int totalScore = 0;
    private int guidedClicks = 0;
    private bool showTutorial = true;
    private bool savedInCurrentGame = false;
    private bool gameStarted = false;
    private ArrayList currentSequence;

    private ModeManager.GameMode gameMode;

    private const int GUIDED_CLICKS_MAX = 5;

    private SfxManager sfxManager;
    private SocialManager socialManager;

    private bool playing = false;

	// Use this for initialization
	void Start () {

        sfxManager = GameObject.Find("SfxManager").GetComponent<SfxManager>();
        socialManager = GameObject.Find("SocialManager").GetComponent<SocialManager>();
        gameMode = GameObject.Find("ModeManager").GetComponent<ModeManager>().getSelectedMode();
        savingPanel.SetActive(false);


        if (gameMode.Equals(ModeManager.GameMode.CLASSIC))
        {
            bool migrated = PlayerPrefs.GetInt("classicTotalScoreSet", 0) == 1;
            if (migrated)
            {
                totalScore = PlayerPrefs.GetInt("classicTotalScore", 0);
            }
            else
            {
                totalScore = PlayerPrefs.GetInt("totalScore", 0);
                PlayerPrefs.SetInt("classicTotalScore", totalScore);

                int highscore = PlayerPrefs.GetInt("highscore", 0);
                PlayerPrefs.SetInt("classicHighscore", highscore);

                PlayerPrefs.SetInt("classicTotalScoreSet", 1);
            }
        }
        else if (gameMode.Equals(ModeManager.GameMode.TEMPO))
        {
            totalScore = PlayerPrefs.GetInt("tempoTotalScore", 0);
        }
        else if (gameMode.Equals(ModeManager.GameMode.WILD))
        {
            totalScore = PlayerPrefs.GetInt("wildTotalScore", 0);
        }

        showTutorial = PlayerPrefs.GetInt("showTutorial", 1) == 1;
        showTutorial = true;
        tutorialHighlights.SetActive(false);
        tutorialEnd.SetActive(false);

        if (!showTutorial)
        {
            tutorialPanel.SetActive(false);
            StartGame(gameMode);
        }
    }

    public void startTutorial()
    {
        tutorialChoice.SetActive(false);
        tutorialPanel.SetActive(false);
        StartGame(gameMode);
    }

    public void closeTutorial()
    {
        PlayerPrefs.SetInt("showTutorial", -1);
        showTutorial = false;
        tutorialPanel.SetActive(false);
        if (!gameStarted)
        {
            StartGame(gameMode);
        }
    }

    public void showTutorialEnd()
    {
        tutorialHighlights.SetActive(false);
        tutorialEnd.SetActive(true);
    }

    public void finishTutorial()
    {
        closeTutorial();
        StartCoroutine(InitiateSequence(true));
    }

    public void StartGame(ModeManager.GameMode mode)
    {
        gameStarted = true;
        savedInCurrentGame = false;
        levelDifficulty = 2;
        currentScore = 0;
        setLevelText(levelDifficulty - 1);
        setScoreText(0);
        setAnnouncementsText("Watch carefully!");
        StartCoroutine(highlightAll(GameButtonSpriteHandler.SpriteType.NORMAL));
        StartCoroutine(InitiateSequence());
    }

    private void setLevelText(int level)
    {
        levelText.GetComponent<Text>().text = "Level: " + level;
    }

    private void setScoreText(int score)
    {
        scoreText.GetComponent<Text>().text = "Score: " + score;
    }

    private void setAnnouncementsText(string text)
    {
        announcementsText.GetComponent<Text>().text = text;
    }

    public IEnumerator InitiateSequence(bool adding = false)
    {
        playing = false;
        currentDigit = 0;
        if (gameMode.Equals(ModeManager.GameMode.CLASSIC))
        {
            if (adding)
            {
                int random = Random.Range(1, 10);
                currentSequence.Add(random);
            }
            else
            {
                currentSequence = new ArrayList();
                for (int i = 0; i < levelDifficulty; i++)
                {
                    int random = Random.Range(1, 10);
                    currentSequence.Add(random);
                }
            }
        }
        else if (gameMode.Equals(ModeManager.GameMode.TEMPO))
        {

        }
        else if (gameMode.Equals(ModeManager.GameMode.WILD))
        {
            currentSequence = new ArrayList();
            for (int i = 0; i < levelDifficulty; i++)
            {
                int random = Random.Range(1, 10);
                currentSequence.Add(random);
            }
        }

        yield return new WaitForSeconds(0.45f);
        bool tutorialHighlightsActive = tutorialHighlights.activeInHierarchy;
        if (showTutorial)
        {
            tutorialHighlights.SetActive(false);
        }
        setAnnouncementsText("Watch carefully!");
        yield return new WaitForSeconds(0.75f);
        for (int i = 0; i < currentSequence.Count; i++)
        {
            highlightButton((int)currentSequence[i], GameButtonSpriteHandler.SpriteType.HIGHLIGHT);
            yield return new WaitForSeconds(0.2f);
            highlightButton((int)currentSequence[i], GameButtonSpriteHandler.SpriteType.NORMAL);
            if (i < currentSequence.Count - 1)
            {
                yield return new WaitForSeconds(0.08f);
            }
        }
        if (showTutorial)
        {
            tutorialHighlights.SetActive(tutorialHighlightsActive);
        }
        setAnnouncementsText("Repeat the sequence!");
        if (showTutorial)
        {
            yield return new WaitForSeconds(0.2f);
            tutorialPanel.SetActive(true);
            tutorialHighlights.SetActive(true);
            Color color = buttonOverlays[(int)currentSequence[0] - 1].GetComponent<Image>().color;
            buttonOverlays[(int)currentSequence[0] - 1].GetComponent<Image>().color = new Color(color.r, color.g, color.b, 0);
        }
        playing = true;
    }

    public void simulateClick(int number)
    {
        if (currentSequence[currentDigit].Equals(number + 1))
        {
            buttons[number].GetComponent<Button>().onClick.Invoke();
        }
    }

    public void highlightButton(int number, GameButtonSpriteHandler.SpriteType type)
    {
        number--;
        buttons[number].GetComponent<GameButtonSpriteHandler>().switchSprite(type);
    }

    public IEnumerator delayedHighlightButton(int number, GameButtonSpriteHandler.SpriteType type, float delay)
    {
        yield return new WaitForSeconds(delay);
        highlightButton(number, type);
    }

    public IEnumerator highlightAll(GameButtonSpriteHandler.SpriteType type, bool backToNormal = true)
    {
        for (int i = 1; i <= buttons.Length; i++)
        {
            highlightButton(i, type);
        }
        if (backToNormal)
        {
            yield return new WaitForSeconds(0.4f);
            for (int i = 1; i <= buttons.Length; i++)
            {
                highlightButton(i, GameButtonSpriteHandler.SpriteType.NORMAL);
            }
        }
    }

    public IEnumerator swapOverlayColor(int position, float delay, float alpha)
    {
        yield return new WaitForSeconds(delay);
        Color color = buttonOverlays[position - 1].GetComponent<Image>().color;
        buttonOverlays[position - 1].GetComponent<Image>().color = new Color(color.r, color.g, color.b, alpha);
    }

    public void buttonClicked(int number)
    {
        if (playing)
        {
            playing = false;
            if (currentSequence[currentDigit].Equals(number))
            {
                if (showTutorial)
                {
                    guidedClicks++;
                    StartCoroutine(swapOverlayColor((int) currentSequence[currentDigit], 0.1f, 1.0f));
                }
                currentDigit++;
                currentScore++;
                totalScore++;
                setScoreText(currentScore);
                socialManager.unlockAchievements(totalScore, currentScore);
                if (currentDigit >= levelDifficulty)
                {
                    if (showTutorial)
                    {
                        tutorialPanel.SetActive(false);
                    }
                    sfxManager.playLevelSound();
                    setAnnouncementsText("Well done!");
                    StartCoroutine(highlightAll(GameButtonSpriteHandler.SpriteType.SUCCESS));
                    levelDifficulty++;
                    setLevelText(levelDifficulty - 1);
                    if (showTutorial)
                    {
                        tutorialPanel.SetActive(true);
                    }
                    if (showTutorial && guidedClicks > GUIDED_CLICKS_MAX)
                    {
                        showTutorialEnd();
                    } else
                    {
                        StartCoroutine(InitiateSequence(true));
                    }
                }
                else
                {
                    sfxManager.playSuccessSound();
                    highlightButton(number, GameButtonSpriteHandler.SpriteType.HIGHLIGHT);
                    StartCoroutine(delayedHighlightButton(number, GameButtonSpriteHandler.SpriteType.NORMAL, 0.2f));
                    if (showTutorial)
                    {
                        playing = false;
                        StartCoroutine(swapOverlayColor((int) currentSequence[currentDigit], 0.1f, 0.0f));
                        playing = true;
                    }
                    playing = true;
                }
            }
            else
            {
                sfxManager.playFailSound();
                StartCoroutine(highlightAll(GameButtonSpriteHandler.SpriteType.FAIL, false));
                if (!savedInCurrentGame)
                {
                    StartCoroutine(activatePanel());
                } else
                {
                    endGame();
                }
            }
        }
    }

    public IEnumerator activatePanel()
    {
        yield return new WaitForSeconds(0.3f);
        savingPanel.SetActive(true);
    }

    public void savingAnswer(bool save)
    {
        savingPanel.SetActive(false);
        if (save)
        {
            savedInCurrentGame = true;
            ShowRewardedAd();
        }
        else
        {
            endGame();
        }
    }

    public void savePlayer()
    {
        currentSequence.RemoveAt(currentSequence.Count - 1);
        StartCoroutine(highlightAll(GameButtonSpriteHandler.SpriteType.NORMAL, false));
        StartCoroutine(InitiateSequence(true));
    }

    public void endGame()
    {
        setAnnouncementsText("Wrong! Game over!");
        if (gameMode.Equals(ModeManager.GameMode.CLASSIC)) {
            PlayerPrefs.SetInt("classicTotalScore", totalScore);
            if (currentScore > PlayerPrefs.GetInt("classicHighscore", 0))
            {
                PlayerPrefs.SetInt("classicHighscore", currentScore);
            }
        }
        else if (gameMode.Equals(ModeManager.GameMode.TEMPO)) {
            PlayerPrefs.SetInt("tempoTotalScore", totalScore);
            if (currentScore > PlayerPrefs.GetInt("tempoHighscore", 0))
            {
                PlayerPrefs.SetInt("tempoHighscore", currentScore);
            }
        }
        else if (gameMode.Equals(ModeManager.GameMode.WILD))
        {
            PlayerPrefs.SetInt("wildTotalScore", totalScore);
            if (currentScore > PlayerPrefs.GetInt("wildHighscore", 0))
            {
                PlayerPrefs.SetInt("wildHighscore", currentScore);
            }
        }
        socialManager.postScoreToLeaderboard(currentScore, gameMode);
        gameStarted = false;
    }

    public void ShowRewardedAd()
    {
        if (Advertisement.IsReady("rewardedVideo"))
        {
            ShowOptions options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show("rewardedVideo", options);
        }
    }

    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                savePlayer();
                break;
            case ShowResult.Skipped:
                endGame();
                break;
            case ShowResult.Failed:
                endGame();
                break;
        }
    }
}
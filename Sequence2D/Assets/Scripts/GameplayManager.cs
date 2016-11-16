using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using System.Collections;

public class GameplayManager : MonoBehaviour {

    public GameObject[] buttons;
    public GameObject levelText, scoreText, announcementsText;
    public GameObject savingPanel;

    private int levelDifficulty;
    private int currentDigit;
    private int currentScore = 0;
    private int totalScore = 0;
    private bool savedInCurrentGame = false;
    private ArrayList currentSequence;

    private SfxManager sfxManager;
    private SocialManager socialManager;

    private bool playing = false;

	// Use this for initialization
	void Start () {

        sfxManager = GameObject.Find("SfxManager").GetComponent<SfxManager>();
        socialManager = GameObject.Find("SocialManager").GetComponent<SocialManager>();
        savingPanel = GameObject.Find("Saving Panel");
        savingPanel.SetActive(false);
        totalScore = PlayerPrefs.GetInt("totalScore", 0);

        //show tutorial if needed
        StartGame();

	}

    public void StartGame()
    {
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
        yield return new WaitForSeconds(0.45f);
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
        setAnnouncementsText("Repeat the sequence!");
        playing = true;
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

    public void buttonClicked(int number)
    {
        if (playing)
        {
            playing = false;
            if (currentSequence[currentDigit].Equals(number))
            {
                currentDigit++;
                currentScore++;
                totalScore++;
                setScoreText(currentScore);
                socialManager.unlockAchievements(totalScore, currentScore);
                if (currentDigit >= levelDifficulty)
                {
                    sfxManager.playLevelSound();
                    setAnnouncementsText("Well done!");
                    StartCoroutine(highlightAll(GameButtonSpriteHandler.SpriteType.SUCCESS));
                    levelDifficulty++;
                    setLevelText(levelDifficulty - 1);
                    StartCoroutine(InitiateSequence(true));
                }
                else
                {
                    sfxManager.playSuccessSound();
                    highlightButton(number, GameButtonSpriteHandler.SpriteType.HIGHLIGHT);
                    StartCoroutine(delayedHighlightButton(number, GameButtonSpriteHandler.SpriteType.NORMAL, 0.2f));
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
        PlayerPrefs.SetInt("totalScore", totalScore);
        if (currentScore > PlayerPrefs.GetInt("highscore", 0))
        {
            PlayerPrefs.SetInt("highscore", currentScore);
        }
        socialManager.postScoreToLeaderboard(currentScore);
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
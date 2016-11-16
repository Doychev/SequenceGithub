using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class SocialManager : MonoBehaviour {

    private static SocialManager instance = null;
    public static SocialManager Instance
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

    // Use this for initialization
    void Start () {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();

        Social.localUser.Authenticate((bool success) => {});
    }

    public void showAchievements()
    {
        if (!Social.localUser.authenticated)
        {
            Social.localUser.Authenticate((bool success) => { });
        }
        Social.ShowAchievementsUI();
    }

    public void showLeaderboards()
    {
        if (!Social.localUser.authenticated)
        {
            Social.localUser.Authenticate((bool success) => { });
        }
        Social.ShowLeaderboardUI();
    }

    public void postScoreToLeaderboard(int score)
    {
        Social.ReportScore(score, SocialIds.leaderboard_sequence_leaderboard, (bool success) =>
        {
            // handle success or failure
        });
    }

    public void unlockAchievements(int totalScore, int highScore)
    {
        if (highScore >= 1)
        {
            unlockAchievement(SocialIds.achievement_welcome);
        }
        if (highScore >= 10)
        {
            unlockAchievement(SocialIds.achievement_starting_to_get_into_this);
        }
        if (highScore >= 25)
        {
            unlockAchievement(SocialIds.achievement_step_by_step);
        }
        if (highScore >= 75)
        {
            unlockAchievement(SocialIds.achievement_guru);
        }
        if (highScore >= 200)
        {
            unlockAchievement(SocialIds.achievement_godlike);
        }
        if (totalScore >= 50)
        {
            unlockAchievement(SocialIds.achievement_newbie);
        }
        if (totalScore >= 200)
        {
            unlockAchievement(SocialIds.achievement_scholar);
        }
        if (totalScore >= 500)
        {
            unlockAchievement(SocialIds.achievement_regular);
        }
        if (totalScore >= 2000)
        {
            unlockAchievement(SocialIds.achievement_pro);
        }
        if (totalScore >= 5000)
        {
            unlockAchievement(SocialIds.achievement_247);
        }
    }

    public void unlockAchievement(string achievementId)
    {
        Social.ReportProgress(achievementId, 100.0f, (bool success) => {
            // handle success or failure
        });
    }


}

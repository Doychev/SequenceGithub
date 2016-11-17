using UnityEngine;
using System.Collections;

public class ModeManager : MonoBehaviour {

    private static ModeManager instance = null;
    public static ModeManager Instance
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

    private GameMode selectedMode;

    public enum GameMode
    {
        CLASSIC,
        WILD,
        TEMPO
    }

    public void selectMode(GameMode mode)
    {
        selectedMode = mode;
    }

    public GameMode getSelectedMode()
    {
        return selectedMode;
    }
}

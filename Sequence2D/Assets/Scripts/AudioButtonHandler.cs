using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AudioButtonHandler : MonoBehaviour
{
    public string prefName;
    public Sprite[] spriteArray;
    private int currentSprite = 0;

    private Image image;

    void Start()
    {
        currentSprite = PlayerPrefs.GetInt(prefName, 1);
        image = GetComponent<Image>();
        image.sprite = spriteArray[currentSprite];
    }

    public void switchSprite()
    {
        currentSprite = (currentSprite + 1) % 2;
        image.sprite = spriteArray[currentSprite];
    }

    public void switchMusic()
    {
        GameObject.Find("MusicManager").GetComponent<MusicManager>().alterMusicVolume();
    }

    public void switchSfx()
    {
        GameObject.Find("SfxManager").GetComponent<SfxManager>().alterSfxVolume();
    }
}

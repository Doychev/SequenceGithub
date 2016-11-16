using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameButtonSpriteHandler : MonoBehaviour {

    public Sprite[] spriteArray;

    public enum SpriteType
    {
        NORMAL = 0,
        HIGHLIGHT = 1,
        SUCCESS = 2,
        FAIL = 3
    }

    private Image image;

    void Start()
    {
        image = GetComponent<Image>();
    }

    public void switchSprite(SpriteType type)
    {
        if (image != null)
        {
            image.sprite = spriteArray[(int)type];
        }
    }
}

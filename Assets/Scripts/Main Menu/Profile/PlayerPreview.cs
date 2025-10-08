using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPreview : MonoBehaviour
{
    [SerializeField] private SpriteListSO playerColors;
    [SerializeField] private SpriteListSO playerFaces;

    [SerializeField] private Image color;
    [SerializeField] private Image face;

    public void SetPlayer(string colorId,  string faceId)
    {
        color.sprite = GetPlayerColorSprite(colorId);
        face.sprite = GetPlayerFaceSprite(faceId);
    }

    private Sprite GetPlayerColorSprite(string colorId)
    {
        foreach (SpriteSO color in playerColors.spriteSOs)
        {
            if (color.id == colorId)
            {
                return color.sprite;
            }
        }
        throw new ArgumentException("The function 'GetPlayerColorSprite' couldn't return any sprite!");
    }

    private Sprite GetPlayerFaceSprite(string spriteId)
    {
        foreach (SpriteSO face in playerFaces.spriteSOs)
        {
            if (face.id == spriteId)
            {
                return face.sprite;
            }
        }
        throw new ArgumentException("The function 'GetPlayerFaceSprite' couldn't return any sprite!");
    }
}

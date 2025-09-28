using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProfileUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private TextMeshProUGUI playerNickName;
    [SerializeField] private Image playerColor;
    [SerializeField] private Image playerFace;

    [SerializeField] private SpriteListSO playerColors;
    [SerializeField] private SpriteListSO playerFaces;

    private Profile.PlayerData associatedPlayerId;

    public void SetPlayerInfo(Profile.PlayerData playerId, bool isClientSide = false)
    {
        associatedPlayerId = playerId;
        playerName.text = playerId.name;
        playerNickName.text = playerId.nickName;
        playerColor.sprite = GetPlayerColorSprite(playerId.colorId);
        playerFace.sprite = GetPlayerFaceSprite(playerId.faceId);

        if (isClientSide) return;

        transform.SetParent(PlayerProfilePanel.Instance.transform);
    }

    public Profile.PlayerData GetPlayerId()
    {
        return associatedPlayerId;
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

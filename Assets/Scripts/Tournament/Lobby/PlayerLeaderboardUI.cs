using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLeaderboardUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerNickName;
    [SerializeField] private Image playerColor;
    [SerializeField] private Image playerFace;

    [SerializeField] private SpriteListSO playerColors;
    [SerializeField] private SpriteListSO playerFaces;

    [SerializeField] private TextMeshProUGUI position;
    [SerializeField] private TextMeshProUGUI time;
    [SerializeField] private TextMeshProUGUI leaderDelta;

    private Profile.PlayerData associatedPlayerId;

    private float associatedTime;

    public void UpdateTime(float newTime, float newLeaderTime, int newPosition)
    {
        position.text = newPosition.ToString();
        time.text = TimeFormatter.FormatTime(newTime);
        associatedTime = newTime;


        if (newPosition == 1)
        {
            leaderDelta.text = "LEADER";
        }
        else
        {
            leaderDelta.text = TimeFormatter.FormatLeaderTime(newTime, newLeaderTime);
        }

        transform.SetSiblingIndex(newPosition - 1);
    }

    public void SetPlayerInfo(Profile.PlayerData playerId)
    {
        associatedPlayerId = playerId;
        playerNickName.text = playerId.nickName;
        playerColor.sprite = GetPlayerColorSprite(playerId.colorId);
        playerFace.sprite = GetPlayerFaceSprite(playerId.faceId);

        transform.SetParent(PlayerLeaderboardPanel.Instance.transform);
    }

    public Profile.PlayerData GetPlayerId()
    {
        return associatedPlayerId;
    }

    public float GetTime()
    {
        return associatedTime;
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

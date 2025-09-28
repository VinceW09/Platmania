using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class TournamentPlayer : NetworkBehaviour
{
    [SerializeField] private SpriteListSO playerColors;
    [SerializeField] private SpriteListSO playerFaces;
    [SerializeField] private GameObject playerProfileUI;
    [SerializeField] private GameObject playerLeaderboardUI;

    private GameObject spawnedPlayerProfileUI;
    private GameObject spawnedPlayerLeaderboardUI;

    private float bestTime;

    private void Start()
    {
        if (IsOwner)
        {
            CreatePlayerProfileUI_ServerRpc(GetPlayerData());
            CreatePlayerLeaderboardUI_ServerRpc(GetPlayerData());

            DontDestroyOnLoad(gameObject);
        }
    }
    
    [ServerRpc]
    private void UpdatePlayerLeaderboardUI_ServerRpc(Profile.PlayerData playerData, float newTime)
    {
        Debug.Log("Player: " + playerData.playerId + " got a new time of: " + newTime);

        List<float> playerTimes = new List<float>();
        int newPosition = 0;

        foreach (PlayerLeaderboardUI playerLeaderboardUI in PlayerLeaderboardPanel.Instance.transform.GetComponentsInChildren<PlayerLeaderboardUI>())
        {
            if (playerLeaderboardUI.GetPlayerId().playerId != playerData.playerId)
            {
                playerTimes.Add(playerLeaderboardUI.GetTime());
            }
        }

        playerTimes.OrderBy(t => t).ToList();


        for (int i = 0; i < playerTimes.Count; i++)
        {
            if (newTime < playerTimes[i] || playerTimes[i] == 0)
            {
                playerTimes.Insert(i, newTime);
                newPosition = i + 1;
                break;
            }
        }

        if (playerTimes.Count <= 0)
        {
            playerTimes.Add(newTime);
            newPosition = 1;
        }

        if (newPosition == 0)
        {
            playerTimes.Add(newTime);
            newPosition = playerTimes.Count;
        }

        
        foreach (PlayerLeaderboardUI playerLeaderboardUI in PlayerLeaderboardPanel.Instance.transform.GetComponentsInChildren<PlayerLeaderboardUI>())
        {
            if (playerLeaderboardUI.GetPlayerId().playerId == playerData.playerId)
            {
                playerLeaderboardUI.UpdateTime(newTime, playerTimes[0], newPosition);
            }
        }

        int index = 0;
        foreach (PlayerLeaderboardUI playerLeaderboardUI in PlayerLeaderboardPanel.Instance.transform.GetComponentsInChildren<PlayerLeaderboardUI>())
        {
            playerLeaderboardUI.UpdateTime(playerTimes[index], playerTimes[0], index+1);

            index++;
        }   
    }
    
    [ServerRpc]
    private void CreatePlayerLeaderboardUI_ServerRpc(Profile.PlayerData playerData)
    {
        spawnedPlayerLeaderboardUI = Instantiate(playerLeaderboardUI, PlayerLeaderboardPanel.Instance.transform);
        spawnedPlayerLeaderboardUI.GetComponent<PlayerLeaderboardUI>().SetPlayerInfo(playerData);
    }

    [ServerRpc]
    private void CreatePlayerProfileUI_ServerRpc(Profile.PlayerData playerData)
    {
        spawnedPlayerProfileUI = Instantiate(playerProfileUI, PlayerProfilePanel.Instance.transform);
        spawnedPlayerProfileUI.GetComponent<PlayerProfileUI>().SetPlayerInfo(playerData);
    }

    [ServerRpc(RequireOwnership = false)]
    public void DestroyPlayerProfileUI_ServerRpc(Profile.PlayerData playerData)
    {
        foreach (PlayerProfileUI playerProfile in PlayerProfilePanel.Instance.transform.GetComponentsInChildren<PlayerProfileUI>())
        {
            if (playerProfile.GetPlayerId().playerId == playerData.playerId)
            {
                Destroy(playerProfile.gameObject);
                return;
            }
        }
        Debug.LogError("Error destroying playerprofileUI of this " + playerData + " playerId.");
    }

    [ServerRpc(RequireOwnership = false)]
    public void DestroyPlayerLeaderboardUI_ServerRpc(Profile.PlayerData playerData)
    {
        foreach (PlayerLeaderboardUI playerLeaderboardUI in PlayerLeaderboardPanel.Instance.transform.GetComponentsInChildren<PlayerLeaderboardUI>())
        {
            if (playerLeaderboardUI.GetPlayerId().playerId == playerData.playerId)
            {
                Destroy(playerLeaderboardUI.gameObject);
                return;
            }
        }
        Debug.LogError("Error destroying playerprofileUI of this " + playerData + " playerId.");
    }

    public Profile.PlayerData GetPlayerData()
    {
        Profile.PlayerData playerData = new Profile.PlayerData();

        playerData.nickName = PlayerPrefs.GetString("player_nickname");
        playerData.name = PlayerPrefs.GetString("player_name");
        playerData.colorId = PlayerPrefs.GetString("player_color_id");
        playerData.faceId = PlayerPrefs.GetString("player_face_id");
        playerData.playerId = PlayerPrefs.GetString("player_id");

        return playerData;
    }

    public void SetNewTime(float time)
    {
        Debug.Log("Setting new time: " + time + "\nold time: " + bestTime);

        if (time < bestTime || bestTime == 0)
        {
            bestTime = time;
            UpdatePlayerLeaderboardUI_ServerRpc(GetPlayerData(), time);
        }
        else
        {
            // No improvement (show animation or something)
        }
    }

    public float GetBestTime()
    {
        return bestTime;
    }
}

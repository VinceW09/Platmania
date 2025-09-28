using System;
using System.Collections;
using System.Linq.Expressions;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport;
using UnityEngine;

public class ServerManager : NetworkBehaviour
{
    public static ServerManager Singleton { get; private set; }

    private UnityTransport unityTransport;

    private int levelNumber;

    private void Start()
    {
        Singleton = this;
        unityTransport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        DontDestroyOnLoad(gameObject);

        if (Loader.ReturnLatestBool() == true)
        {
            // it is a client
            try
            {
                unityTransport.ConnectionData.Address = Loader.ReturnLatestMessage();

                NetworkManager.Singleton.StartClient();

                Debug.Log("CLIENT INFO: ServerIP:" + unityTransport.ConnectionData.Address + ";Port:" + unityTransport.ConnectionData.Port);
            }
            catch (Exception)
            {
                Debug.LogWarning($"Couldn't find server with ip {Loader.ReturnLatestMessage()}. Redirecting to main menu.");
                StartCoroutine(ShutdownAfterDelay());
            }

                
            StartCoroutine(WaitForPlayerObject(() =>
            {
                Profile.PlayerData playerData = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<TournamentPlayer>().GetPlayerData();
                LobbyUI.Singleton.SetupClient(playerData);
            }));
        }

        GameManager.Singleton.OnTournamentStart += OnTournamentStart;
        GameManager.Singleton.OnTournamentEnd += OnTournamentEnd;
    }

    private void OnTournamentStart(object sender, EventArgs e)
    {
        LoadLevel_ClientRpc(levelNumber);
    }

    private void OnTournamentEnd(object sender, EventArgs e)
    {
        QuitLobby_ClientRpc();
    }

    private IEnumerator WaitForPlayerObject(Action action)
    {
        while (NetworkManager.Singleton.LocalClient.PlayerObject == null)
        {
            yield return null;
        }

        action();
    }

    [ClientRpc]
    private void LoadLevel_ClientRpc(int number)
    {
        Loader.LoadScene(Scenes.sceneList[Scenes.SceneKey.LEVEL_].name + number);
    }

    public void SetLevelNumber(int newNumber)
    {
        levelNumber = newNumber;
    }

    public void Disconnect()
    {
        if (IsClient)
        {
            TournamentPlayer tournamentPlayer = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<TournamentPlayer>();
            tournamentPlayer.DestroyPlayerProfileUI_ServerRpc(tournamentPlayer.GetPlayerData());
            tournamentPlayer.DestroyPlayerLeaderboardUI_ServerRpc(tournamentPlayer.GetPlayerData());
        }

        if (IsServer)
        {
            QuitLobby_ClientRpc();
        }

        StartCoroutine(ShutdownAfterDelay());
    }

    private IEnumerator ShutdownAfterDelay()
    {
        yield return null;

        NetworkManager.Singleton.Shutdown();
        Destroy(NetworkManager.Singleton.gameObject);
        Destroy(gameObject);
        Loader.LoadScene(Scenes.sceneList[Scenes.SceneKey.MAINMENU].name);
    }

    [ClientRpc]
    private void QuitLobby_ClientRpc()
    {
        Destroy(NetworkManager.LocalClient.PlayerObject);
        NetworkManager.Singleton.Shutdown();
        Destroy(NetworkManager.Singleton.gameObject);
        Destroy(gameObject);
        Loader.LoadScene(Scenes.sceneList[Scenes.SceneKey.MAINMENU].name);
    }
}

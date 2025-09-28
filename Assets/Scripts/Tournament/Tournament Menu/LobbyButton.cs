using System.Collections;
using System.Collections.Generic;
using Unity.Netcode.Transports.UTP;
using Unity.Netcode;
using UnityEngine;
using TMPro;

public class LobbyButton : MonoBehaviour
{
    private TextMeshProUGUI text;
    private string ServerIp  = string.Empty;
    private string lobbyName = string.Empty;

    private void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        text.text = lobbyName;
    }

    public void JoinServer()
    {
        Loader.LoadScene(Scenes.sceneList[Scenes.SceneKey.LOBBY].name, ServerIp, true);
    }

    public void SetIp(string ip)
    {
        ServerIp = ip;
    }

    public void SetName(string name)
    {
        lobbyName = name; 
    }
}

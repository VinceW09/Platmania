using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using NativeWebSocket;

public class JoinLobby : MonoBehaviour
{
    [SerializeField] private Transform serverList;
    [SerializeField] private GameObject lobbyButtonPrefab;
    private LobbyButton lobbyButton;

    private HashSet<string> discoveredServers = new HashSet<string>();

    private UdpClient udpClient;
    private static readonly Queue<Action> actions = new Queue<Action>();


    private void Start()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer) return;

        udpClient = new UdpClient(47777);
        udpClient.BeginReceive(OnUdpDataReceived, udpClient);
    }

    private void Update()
    {
        lock (actions)
        {
            while (actions.Count > 0)
                actions.Dequeue().Invoke();
        }
    }

    private void OnUdpDataReceived(IAsyncResult result)
    {
        IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
        byte[] data = udpClient.EndReceive(result, ref remoteEP);
        string message = Encoding.UTF8.GetString(data);
        Enqueue(() => HandleServerMessage(message));
        udpClient.BeginReceive(OnUdpDataReceived, udpClient);
    }

    private void Enqueue(Action action)
    {
        lock (actions)
        {
            actions.Enqueue(action);
        }
    }

    private void HandleServerMessage(string message)
    {
        string ip = message.Split(';')[0].Split(':')[1];
        string port = message.Split(';')[1].Split(':')[1];
        string name = message.Split(';')[2].Split(':')[1];

        if (!discoveredServers.Contains(ip))
        {
            discoveredServers.Add(ip);
            GameObject lobbyButtonGameObject = Instantiate(lobbyButtonPrefab, serverList);
            lobbyButton = lobbyButtonGameObject.GetComponent<LobbyButton>();
            lobbyButton.SetIp(ip);
            lobbyButton.SetName(name);
        }
    }

    private void OnDestroy()
    {
        udpClient?.Close();
    }
}
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using TMPro;
using System.Collections;
using UnityEngine.Networking;

public class ServerBroadcaster : NetworkBehaviour
{
    private UnityTransport unityTransport;
    private UdpClient udpClient;
    private float broadcastInterval = 1f;
    private float lastBroadcastTime;
    private string serverName;
    private bool broadcasting = false;
    private string ip;

    [SerializeField] TextMeshProUGUI iptext;

    private void Awake()
    {
        ip = GetLocalIPAddress();
    }

    private void Start()
    {
        unityTransport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        udpClient = new UdpClient();

        
        if (Loader.ReturnLatestBool() == false)
        {
            // Server startup
            broadcasting = true;
            unityTransport.ConnectionData.Address = ip;
            NetworkManager.Singleton.StartServer();
            serverName = Loader.ReturnLatestMessage();
            udpClient.EnableBroadcast = true;

            Debug.Log("SERVER INFO: ServerIP:" + unityTransport.ConnectionData.Address + ";Port:" + unityTransport.ConnectionData.Port);
            LobbyUI.Singleton.SetupServer();
        }
            
        iptext.text = "IP: " + unityTransport.ConnectionData.Address;

        GameManager.Singleton.OnTournamentStart += OnTournamentStart;
    }

    private void OnTournamentStart(object sender, EventArgs e)
    {
        StopBroadcasting();
    }

    private void Update()
    {
        if (IsServer && broadcasting)
        {
            if (Time.time - lastBroadcastTime > broadcastInterval)
            {
                BroadcastServer();
                lastBroadcastTime = Time.time;
            }
        }
    }

    private void BroadcastServer()
    {
        string message = $"ServerIP:{unityTransport.ConnectionData.Address};Port:{unityTransport.ConnectionData.Port};ServerName:{serverName}";
        byte[] data = Encoding.UTF8.GetBytes(message);
        udpClient.Send(data, data.Length, new IPEndPoint(IPAddress.Broadcast, 47777));
    }

    private string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
                return ip.ToString();
        }
        return string.Empty;
    }

    public void StopBroadcasting()
    {
        broadcasting = false;
        udpClient?.Close();
    }

    private void OnApplicationQuit()
    {
        udpClient?.Close();
    }
}
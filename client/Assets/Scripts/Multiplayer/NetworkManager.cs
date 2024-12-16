using UnityEngine;
using RiptideNetworking;
using RiptideNetworking.Utils;
using System;
using UnityEngine.UI;

public enum ServerToClientId : ushort
{   
    playerSpawned = 1,
    playerMovement,
    animalMovement,
    puntuacion,
    Ganador
}

public enum ClientToServerId : ushort
{
    name = 1,
    conected,
    input,
    sumar,
    puntuacion,
}

public class NetworkManager : MonoBehaviour
{
    private static NetworkManager _singleton;

    public static NetworkManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
            {
                _singleton = value;
            }
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(NetworkManager)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }

    public Client Client { get; private set; }



    //[SerializeField] private string ip;
    //[SerializeField] private string serverIP = "192.168.100.16";
    [SerializeField] public InputField hostPortField;
    //private string serverIP = hostPortField.text;
    [SerializeField] private ushort serverPort = 7777;


    private void Awake()
    {
        Singleton = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);

        Client = new Client();
        Client.Connected += DidConnect;
        Client.ConnectionFailed += FailedToConnect;
        Client.ClientDisconnected += PlayerLeft;
        Client.Disconnected += DidDisconnect;

    }

    private void FixedUpdate()
    {
        Client.Tick();
    }
    
    private void OnApplicationQuit()
    {
        Client.Disconnect();
    }

    public void Connect()
    {
        Client.Connect($"{hostPortField.text}:{serverPort}");
        SendConnect();
    } 

    private void DidConnect(object sender, EventArgs e)
    {
        UIManager.Singleton.SendName();
    }

    private void FailedToConnect(object sender, EventArgs e)
    {
        UIManager.Singleton.BackToMain();
    }

    private void PlayerLeft(object sender, ClientDisconnectedEventArgs e)
    {
        if(Player.list.TryGetValue(e.Id, out Player player))
            Destroy(Player.list[e.Id].gameObject);

    }

    private void DidDisconnect(object sender, EventArgs e)
    {
        UIManager.Singleton.BackToMain();
        foreach (Player player in Player.list.Values)
        {
            Destroy(player.gameObject);
        }
    }

    private void SendConnect()
    {
        Message message = Message.Create(MessageSendMode.unreliable, ClientToServerId.input);
        message.AddBool(true);
        NetworkManager.Singleton.Client.Send(message);
    }

}

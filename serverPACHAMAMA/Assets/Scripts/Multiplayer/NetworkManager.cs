using UnityEngine;
using RiptideNetworking;
using RiptideNetworking.Utils;
using System.Collections;

public enum ServerToClientId : ushort
{
    playerSpawned = 1,
    playerMovement,
    animalMovement,
    puntuacion,
    Ganador,
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

    public Server Server { get; private set; }

    [SerializeField] private ushort port = 7777;
    [SerializeField] private ushort maxClientCount;

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        Application.targetFrameRate = 60;

        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);

        Server = new Server();
        Server.Start(port, maxClientCount);

        Server.ClientDisconnected += PlayerLeft;

    }

    private void FixedUpdate()
    {
        Server.Tick();

    }

    private void OnApplicationQuit()
    {
        Server.Stop();
    }

    private void PlayerLeft(object sender, ClientDisconnectedEventArgs e)
    {
        if (Player.list.TryGetValue(e.Id, out Player player))
            Destroy(Player.list[e.Id].gameObject);
    }

    public IEnumerator Ganador()
    {
        yield return new WaitForSeconds(5f);
        Server.Stop();
    }

}

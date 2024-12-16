using RiptideNetworking;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Windows;

public class Player : MonoBehaviour
{
    public static Dictionary<ushort, Player> list = new Dictionary<ushort, Player>();

    public ushort Id { get; private set; }
    public string Username { get; private set; }

    public bool IsPlaying;

    private int puntuacion = 0;


    public PlayerMovement Movement => movement;
    [SerializeField] private PlayerMovement movement;


    [SerializeField] private TextMeshPro nameTag;

    private void OnDestroy()
    {

        list.Remove(Id);
    }

    public static void Spawn(ushort id, string username)
    {
        foreach (Player otherPlayer in list.Values)
        {
            otherPlayer.SendSpawned(id);
        }

        Player player = Instantiate(GameLogic.Singleton.PlayerPrefab, new Vector3(0f, 1f, 0f), Quaternion.identity).GetComponent<Player>();
        player.name = $"Player {id}({(string.IsNullOrEmpty(username) ? "Guest" : username)})";
        player.Id = id;
        player.Username = string.IsNullOrEmpty(username) ? $"Guest {id}" : username;
        
        player.setUserTag(username, id);

        player.SendSpawned();
        list.Add(id, player);
    }

    private void setUserTag(string username, int id)
    {
        if (string.IsNullOrEmpty(username))
        {
            nameTag.SetText($"Guest {id}");
        }
        else
        {
            nameTag.SetText(username);
        }

    }

    #region Messages
    private void SendSpawned()
    {
        NetworkManager.Singleton.Server.SendToAll(AddSpawnData(Message.Create(MessageSendMode.reliable, ServerToClientId.playerSpawned)));
    } 

    private void SendSpawned(ushort toClientId)
    {
        NetworkManager.Singleton.Server.Send(AddSpawnData(Message.Create(MessageSendMode.reliable, ServerToClientId.playerSpawned)), toClientId);
    }

    private Message AddSpawnData(Message message)
    {
        message.AddUShort(Id);
        message.AddString(Username);
        message.AddVector3(transform.position);

        return message;
    }

    private void SendIntializePunctuations()
    {
        Message message = Message.Create(MessageSendMode.reliable, ServerToClientId.puntuacion);
    }

    private static void SendPuntuacion(ushort toClientId, int punctuation)
    {
        Message message = Message.Create(MessageSendMode.reliable, ServerToClientId.puntuacion);
        message.AddUShort(toClientId);
        message.AddInt(punctuation);
        NetworkManager.Singleton.Server.SendToAll(message);
    }

    private static void SendGanador(ushort toClientId)
    {
        Message message = Message.Create(MessageSendMode.reliable, ServerToClientId.Ganador);
        message.AddUShort(toClientId);
        NetworkManager.Singleton.Server.SendToAll(message);
    }


    [MessageHandler((ushort)ClientToServerId.name)]
    private static void Name(ushort fromClientId, Message message)
    {
        Spawn(fromClientId, message.GetString());
    }

    [MessageHandler((ushort)ClientToServerId.input)]
    private static void Input(ushort fromClientId, Message message)
    {

        if (list.TryGetValue(fromClientId, out Player player))
        {
            player.Movement.SetHorizontalAxis(message.GetFloat());
            player.Movement.SetVerticalAxis(message.GetFloat());
        }
            
    }
    
    [MessageHandler((ushort)ClientToServerId.sumar)]
    private static void SumarPuntos(ushort fromClientId, Message message)
    {

        if (list.TryGetValue(fromClientId, out Player player))
        {
            player.puntuacion += message.GetInt();
            SendPuntuacion(fromClientId, player.puntuacion);

            if (player.puntuacion == 7)
            {
                SendGanador(fromClientId);
                NetworkManager.Singleton.StartCoroutine(NetworkManager.Singleton.Ganador());
            }

        }

        //SendPuntuacion(fromClientId);
    }


    #endregion

}

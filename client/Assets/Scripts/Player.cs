using UnityEngine;
using RiptideNetworking;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine.UI;
using TMPro;
using TMPro.Examples;

public class Player : MonoBehaviour
{
    public static Dictionary<ushort, Player> list = new Dictionary<ushort, Player>();

    public ushort Id { get; private set; }
    public bool IsLocal { get; private set; }

    //public bool IsPlaying;

    //[SerializeField] private Transform camTransform;
    [SerializeField] private TextMeshPro nameTag;

    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRender;
    private static CameraController myCameraController;

    private string username;
    private int puntuacion = 0;

    private float moveHorizontal;
    private float moveVertical;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRender = GetComponent<SpriteRenderer>();
        myCameraController = Camera.main.GetComponent<CameraController>();

    }

    private void FixedUpdate()
    {
        AdjustPlayerFacingDirection();
    }


    private void OnDestroy()
    {
        list.Remove(Id);

    }

    private void Move( Vector3 newPosition)
    {
        //Vector3 lastPosition = rb.position;
        
        rb.MovePosition(newPosition);

        myAnimator.SetFloat("moveHorizontal", moveHorizontal);
        myAnimator.SetFloat("moveVertical", moveVertical);


        //Debug.Log($"posicion x: + {rb.position.x - lastPosition.x}");

        if (!IsLocal)
        {
            
        }
    }


    public static void Spawn(ushort id, string username, Vector3 position)
    {
        Player player;

        if (id == NetworkManager.Singleton.Client.Id)
        {
            
            player = Instantiate(GameLogic.Singleton.LocalPlayerPrefab, position, Quaternion.identity).GetComponent<Player>();
            player.IsLocal = true;

            myCameraController.SetTarget(player.transform);
        }
        else
        {
            player = Instantiate(GameLogic.Singleton.PlayerPrefab, position, Quaternion.identity).GetComponent<Player>();
            player.IsLocal = false;
        }

        player.name = $"Player {id} ({(string.IsNullOrEmpty(username) ? $"Guest" : username)})";
        player.Id = id;
        player.username = username;


        player.setUserTag(username, id);

        list.Add(id, player);

        setPnlPunctuations();

    }

    private static void setPnlPunctuations()
    {

        foreach (var entry in Player.list)
        {
            ushort key = entry.Key;
            Player player = entry.Value;

            myCameraController.addPlayerToPanel($"({(string.IsNullOrEmpty(player.username) ? $"Guest {key}" : player.username)})", key - 1, player.puntuacion);
        }
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

    private void esGanador()
    {
        myCameraController.mostrarGanaste();
    }

    private void esPerdedor()
    {
        myCameraController.mostrarPerdiste();
    }
    private void AdjustPlayerFacingDirection()
    {

        if (moveHorizontal < 0)
        {
            mySpriteRender.flipX = true;
        }
        else if (moveHorizontal > 0)
        {
            mySpriteRender.flipX = false;
        }

    }

    #region Messages
    [MessageHandler((ushort)ServerToClientId.playerSpawned)]
    private static void SpawnPlayer(Message message)
    {
        Spawn(message.GetUShort(), message.GetString(), message.GetVector3());
        
    }

    [MessageHandler((ushort)ServerToClientId.playerMovement)]
    private static void PlayerMovement(Message message)
    {
        if (list.TryGetValue(message.GetUShort(), out Player player))
        {
            player.Move(message.GetVector3());
            player.moveHorizontal = message.GetFloat();
            player.moveVertical = message.GetFloat();

        }
    }

    [MessageHandler((ushort)ServerToClientId.puntuacion)]
    private static void SumarPuntuacion(Message message)
    {
        if (list.TryGetValue(message.GetUShort(), out Player player))
        {
            player.puntuacion = message.GetInt();

            myCameraController.setPunctuationText(player.puntuacion);

            setPnlPunctuations();

        }

    }

    [MessageHandler((ushort)ServerToClientId.Ganador)]
    private static void Ganador(Message message)
    {
        ushort idGanador = message.GetUShort();

        foreach (KeyValuePair<ushort, Player> entry in list)
        {
            ushort key = entry.Key;
            Player player = entry.Value;

            if (key == idGanador)
            {
                player.esGanador();
            }
            else
            {
                player.esPerdedor();
            }
        }

        /*if (list.TryGetValue(idGanador, out Player player))
        {
            player.esGanador();

        }
        else{
            foreach (KeyValuePair<ushort, Player> entry in Player.list)
            {
                ushort key = entry.Key;

                if ()
                {

                }
            }
        }*/

    }

    #endregion
}

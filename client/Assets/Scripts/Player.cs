using UnityEngine;
using RiptideNetworking;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public static Dictionary<ushort, Player> list = new Dictionary<ushort, Player>();

    public ushort Id { get; private set; }
    public bool IsLocal { get; private set; }

    //[SerializeField] private Transform camTransform;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRender;

    private string username;

    private float moveHorizontal;
    private float moveVertical;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRender = GetComponent<SpriteRenderer>();
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

        Debug.Log($"posicion: + {newPosition}");

        //Debug.Log($"posicion x: + {rb.position.x - lastPosition.x}");

        if (!IsLocal)
        {
            
        }
    }

    private void FixedUpdate()
    {
        AdjustPlayerFacingDirection();
    }

    public static void Spawn(ushort id, string username, Vector3 position)
    {
        Player player;
        if (id == NetworkManager.Singleton.Client.Id)
        {
            player = Instantiate(GameLogic.Singleton.LocalPlayerPrefab, position, Quaternion.identity).GetComponent<Player>();
            player.IsLocal = true;

            Camera.main.GetComponent<CameraController>().SetTarget(player.transform);
        }
        else
        {
            player = Instantiate(GameLogic.Singleton.PlayerPrefab, position, Quaternion.identity).GetComponent<Player>();
            player.IsLocal = false;
        }

        player.name = $"Player {id} ({(string.IsNullOrEmpty(username) ? "Guest" : username)})";
        player.Id = id;
        player.username = username;

        list.Add(id, player);

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

    #endregion
}

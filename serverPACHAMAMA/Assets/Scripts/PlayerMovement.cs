using UnityEngine;
using RiptideNetworking;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private float movementSpeed;

    private float moveSpeed;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRender;

    private float moveHorizontal;
    private float moveVertical;


    private void OnValidate()
    {
        if(player == null)
            player = GetComponent<Player>();

        Initialize();

    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRender = GetComponent<SpriteRenderer>();
        Initialize();
    }

    private void Update()
    {
        Vector3 moveDirection = new Vector3(moveHorizontal, moveVertical, 0) * moveSpeed;
        myAnimator.SetFloat("moveHorizontal", moveHorizontal);
        myAnimator.SetFloat("moveVertical", moveVertical);

        rb.MovePosition(rb.position + (Vector2)moveDirection * Time.deltaTime);
        SendMovement();
    }

    private void FixedUpdate()
    {
        AdjustPlayerFacingDirection();
    }

    private void Initialize()
    {
        moveSpeed = movementSpeed;
    }

    private void AdjustPlayerFacingDirection()
    {
        if (moveHorizontal < 0)
        {
            mySpriteRender.flipX = true;
        }
        else if(moveHorizontal > 0)
        {
            mySpriteRender.flipX = false;
        }
    }


    public void SetHorizontalAxis(float horizontal)
    {
        moveHorizontal = horizontal;
    }

    public void SetVerticalAxis(float vertical)
    {
        moveVertical = vertical;
    }

    private void SendMovement()
    {

        Message message = Message.Create(MessageSendMode.unreliable, ServerToClientId.playerMovement);
        message.AddUShort(player.Id); //id jugador
        message.AddVector3(rb.position);//posicion del jugador
        message.AddFloat(moveHorizontal);//mover horizontall
        message.AddFloat(moveVertical);//mover vertical
        NetworkManager.Singleton.Server.SendToAll(message);

    }

}

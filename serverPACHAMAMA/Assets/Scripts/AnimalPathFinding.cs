using RiptideNetworking;
using UnityEngine;

public class AnimalPathFinding : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] public string myUrl;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Vector2 moveDir;
    private bool isRunning;
    //private Animator myAnimator;
    private int animalIndex;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        //myAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        //SendURL();
    }

    private void FixedUpdate()
    {

        rb.MovePosition(rb.position + moveDir * (moveSpeed * Time.fixedDeltaTime));
        //myAnimator.SetBool("isRunning", isRunning);
        SendMovement();
    }


    public void MoveTo(Vector2 targetPosition)
    {
        moveDir = targetPosition;
        //myAnimator.SetBool("isRunning", isRunning);
        isRunning = true;

        if (targetPosition.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (targetPosition.x > 0)
        {
            spriteRenderer.flipX = false;
        }

    }

    public void Initialize(int index)
    {
        animalIndex = index; 
    }

    public void StopMoving()
    {
        moveDir = Vector2.zero;
        isRunning = false;
    }

    #region Messages

    private void SendMovement()
    {
        Message message = Message.Create(MessageSendMode.unreliable, ServerToClientId.animalMovement);
        message.AddInt(animalIndex); //indice del animal
        message.AddVector2(rb.position);//posicion del animal
        message.AddFloat(moveDir.x);// direccion de la vista del animal
        message.AddBool(isRunning); //estado del movimiento
        NetworkManager.Singleton.Server.SendToAll(message);
    }


    #endregion
}

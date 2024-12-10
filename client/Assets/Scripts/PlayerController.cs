using RiptideNetworking;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //[SerializeField] private Transform camTransform;

    private bool[] inputs;


    private UnityEngine.Vector2 movement;
    //private Animator myAnimator;
    //private SpriteRenderer mySpriteRender;

    private float moveHorizontal;
    private float moveVertical;

    private void Awake() {
        //myAnimator = GetComponent<Animator>();
        //mySpriteRender = GetComponent<SpriteRenderer>();
    }


    private void Update() {

        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");

        //myAnimator.SetFloat("moveHorizontal", moveHorizontal);
        //myAnimator.SetFloat("moveVertical", moveVertical);

        SendAxis();

    }

    /*private void FixedUpdate() {
        AdjustPlayerFacingDirection();

    }*/

    #region Messages

    private void SendAxis()
    {

        Message message = Message.Create(MessageSendMode.unreliable, ClientToServerId.input);
        message.AddFloat(moveHorizontal);
        message.AddFloat(moveVertical);
        NetworkManager.Singleton.Client.Send(message);
    }

    #endregion


    /*private void AdjustPlayerFacingDirection(){

        if(moveHorizontal < 0)
        {
            mySpriteRender.flipX = true;    
        }else if(moveHorizontal > 0)
        {
            mySpriteRender.flipX = false;
        }

    }*/


}

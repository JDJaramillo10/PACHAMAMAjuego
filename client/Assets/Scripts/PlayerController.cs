using RiptideNetworking;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private bool[] inputs;


    private Vector2 movement;

    private float moveHorizontal;
    private float moveVertical;



    private void Update() {

        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");

        SendAxis();

    }


    #region Messages

    private void SendAxis()
    {

        Message message = Message.Create(MessageSendMode.unreliable, ClientToServerId.input);
        message.AddFloat(moveHorizontal);
        message.AddFloat(moveVertical);
        NetworkManager.Singleton.Client.Send(message);
    }

    #endregion


}

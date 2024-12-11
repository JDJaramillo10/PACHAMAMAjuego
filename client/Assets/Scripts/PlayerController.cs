using RiptideNetworking;
using Unity.VisualScripting;
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

        if (Input.GetKeyDown(KeyCode.I))
        {
            SendObtenerPuntuacion();
        }

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

    private void SendObtenerPuntuacion()
    {
        Message message = Message.Create(MessageSendMode.unreliable, ClientToServerId.puntuacion);
        message.AddBool(true);
        NetworkManager.Singleton.Client.Send(message);
    }

    #endregion


}

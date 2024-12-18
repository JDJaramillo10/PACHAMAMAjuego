using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using RiptideNetworking;
using RiptideNetworking.Utils;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private static UIManager _singleton;

    public static UIManager Singleton
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
                Debug.Log($"{nameof(UIManager)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }

    [Header("Connect")]
    [SerializeField] private GameObject connectUI;
    [SerializeField] private InputField usernameField;
    //[SerializeField] private InputField hostPortField;

    private void Awake()
    {
        Singleton = this;
    }

    public void ConnectClicked()
    {
        usernameField.interactable = false;
        NetworkManager.Singleton.hostPortField.interactable = false;
        connectUI.SetActive(false);

        NetworkManager.Singleton.Connect();
        //SceneManager.LoadScene("Main");
    }

    public void BackToMain()
    {
        usernameField.interactable = true;
        NetworkManager.Singleton.hostPortField.interactable = true;
        connectUI.SetActive(true);
        //SceneManager.LoadScene("ConnectScene");
    }

    public void SendName()
    {
        Message message = Message.Create(MessageSendMode.reliable, ClientToServerId.name); 
        message.AddString(usernameField.text);
        NetworkManager.Singleton.Client.Send(message);
    }


}

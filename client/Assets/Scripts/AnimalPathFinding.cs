using RiptideNetworking;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Linq;

public class AnimalPathFinding : MonoBehaviour
{

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Vector2 moveDir;
    private Animator myAnimator;
    private int myAnimIndex;
    private string animName;
    private bool adivinado;

    private bool messageReceived;

    [SerializeField] public GameObject questionPanel;
    [SerializeField] public Text questionText;
    [SerializeField] public Button opcion1;
    [SerializeField] public Button opcion2;
    [SerializeField] public Button opcion3;
    [SerializeField] public Button opcion4;
    [SerializeField] public RawImage imagen;

    private Texture imageTexture;

    [SerializeField] private string myURL;
    [SerializeField] private string nombreCientifico;

    public bool playerIsClose;


    private void Awake(){
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        myAnimator = GetComponent<Animator>();
        adivinado = false;
    }

    private void Start()
    {
        StartCoroutine(DownloadImageFromURL(myURL));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerIsClose && !adivinado)
        {
            if (questionPanel.activeInHierarchy)
            {
                zeroText();
            }
            else
            {
                questionPanel.SetActive(true);
                ShowText();
            }
        }
    }

    IEnumerator DownloadImageFromURL(string url)
    {

        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Texture downloadedTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;

            imageTexture = downloadedTexture;
        }
        else
        {
            Debug.Log(request.error);
        }
    }

    public void InitializeIndex(int index)
    {
        myAnimIndex = index;
    }

    public void ShowText()
    {
        questionText.text = $"Cúal es el nombre científico del {gameObject.name}";


        imagen.texture = imageTexture;

        // Lista de opciones para las respuestas
        List<string> opciones = new List<string>();

        // Obtenemos el nombre del animal actual (este objeto)
        string nombreCorrecto = nombreCientifico;

        // Añadir la respuesta correcta a las opciones
        opciones.Add(nombreCorrecto);

        // Obtener una lista de nombres de animales diferentes al actual
        List<string> nombresOtrosAnimales = AnimalManager.Singleton.animals
            .Select(animal => animal.nombreCientifico)
            .Where(nombre => nombre != nombreCorrecto) // Excluir el animal actual
            .OrderBy(_ => Random.value) // Ordenar aleatoriamente
            .Take(3) // Tomar 3 nombres
            .ToList();

        // Añadir los nombres de los otros animales a las opciones
        opciones.AddRange(nombresOtrosAnimales);

        // Mezclar las opciones de manera aleatoria
        opciones = opciones.OrderBy(_ => Random.value).ToList();

        // Asignar las opciones a los botones
        opcion1.GetComponentInChildren<Text>().text = opciones[0];
        opcion2.GetComponentInChildren<Text>().text = opciones[1];
        opcion3.GetComponentInChildren<Text>().text = opciones[2];
        opcion4.GetComponentInChildren<Text>().text = opciones[3];

        opcion1.onClick.AddListener(() => HandleAnswerClick(opcion1));
        opcion2.onClick.AddListener(() => HandleAnswerClick(opcion2));
        opcion3.onClick.AddListener(() => HandleAnswerClick(opcion3));
        opcion4.onClick.AddListener(() => HandleAnswerClick(opcion4));
    }

    public void zeroText()
    {
        questionText.text = "";
        opcion1.GetComponentInChildren<Text>().text = "";
        opcion2.GetComponentInChildren<Text>().text = "";
        opcion3.GetComponentInChildren<Text>().text = "";
        opcion4.GetComponentInChildren<Text>().text = "";
        questionPanel.SetActive(false);

        opcion1.onClick.RemoveAllListeners();
        opcion2.onClick.RemoveAllListeners();
        opcion3.onClick.RemoveAllListeners();
        opcion4.onClick.RemoveAllListeners();

    }


    private void FixedUpdate(){

        if (messageReceived)
        {
            rb.MovePosition(moveDir);
            messageReceived = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("LocalPlayer"))
        {
            playerIsClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("LocalPlayer"))
        {
            playerIsClose = false;
        }

    }

    public void HandleAnswerClick(Button clickedButton)
    {
        string buttonText = clickedButton.GetComponentInChildren<Text>().text;

        if (buttonText == nombreCientifico)
        {
            adivinado = true;
            AddScore();

            Camera.main.GetComponent<CameraController>().unlockImage(nombreCientifico);

            StartCoroutine(ShowCorrectoPanelForSeconds(1f));

            Debug.Log("¡Correcto! Este es el nombre científico.");
        }
        else
        {
            StartCoroutine(ShowIncorrectoPanelForSeconds(1f));
            Debug.Log("Respuesta incorrecta.");
        }

        zeroText();
    }

    private IEnumerator ShowCorrectoPanelForSeconds(float seconds)
    {
        Camera.main.GetComponent<CameraController>().setCorrectoActive();
        yield return new WaitForSeconds(seconds); // Esperar
        Camera.main.GetComponent<CameraController>().setCorrectoInactive();

    }

    private IEnumerator ShowIncorrectoPanelForSeconds(float seconds)
    {
        Camera.main.GetComponent<CameraController>().setIncorrectoActive();
        yield return new WaitForSeconds(seconds); // Esperar
        Camera.main.GetComponent<CameraController>().setIncorrectoInactive();

    }

    public void MoveTo(Vector2 targetPosition, float facingDir, bool received){
        moveDir = targetPosition;
        myAnimator.SetBool("isRunning", true);
        messageReceived = received;

        if (facingDir < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (facingDir > 0)
        {
            spriteRenderer.flipX = false;
        }

    }

    public void StopMoving()
    {
        myAnimator.SetBool("isRunning", false);
    }


    #region Messages

    private void AddScore()
    {
        Message message = Message.Create(MessageSendMode.reliable, ClientToServerId.sumar);
        message.AddInt(1);
        NetworkManager.Singleton.Client.Send(message);
    }

    #endregion

}

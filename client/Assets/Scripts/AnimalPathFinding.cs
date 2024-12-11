using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AnimalPathFinding : MonoBehaviour
{

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Vector2 moveDir;
    private Animator myAnimator;
    private int myAnimIndex;
    private string animName;
    private string nombreCientifico;

    private bool messageReceived;

    [SerializeField] public GameObject questionPanel;
    [SerializeField] public Text questionText;
    [SerializeField] public Button opcion1;
    [SerializeField] public Button opcion2;
    [SerializeField] public Button opcion3;
    [SerializeField] public Button opcion4;

    public bool playerIsClose;


    private void Awake(){
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        myAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerIsClose)
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

    public void InitializeIndex(int index)
    {
        myAnimIndex = index;
    }

    public void InitializeNombreCientífico(string nombreC)
    {
        nombreCientifico = nombreC;
    }

    public void ShowText()
    {
        questionText.text = $"Cúal es el nombre científico del {gameObject.name}";

        opcion1.GetComponentInChildren<Text>().text = "Hola";
        opcion2.GetComponentInChildren<Text>().text = "Venado";
        opcion3.GetComponentInChildren<Text>().text = "Zorro";
        opcion4.GetComponentInChildren<Text>().text = "Manu";

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
            Debug.Log("¡Correcto! Este es el nombre científico.");
        }
        else
        {
            Debug.Log("Respuesta incorrecta.");
        }

        zeroText();
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


}

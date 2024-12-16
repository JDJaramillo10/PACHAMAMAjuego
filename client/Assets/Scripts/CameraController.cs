using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    private Transform target;

    [SerializeField] private GameObject punctuationContent;
    [SerializeField] public List<Text> texts;
    [SerializeField] private Text punctuationText;

    [SerializeField] private GameObject Correcto;
    [SerializeField] private GameObject Incorrecto;

    [SerializeField] private List<Image> Images;

    [SerializeField] private GameObject Ganaste;
    [SerializeField] private GameObject Perdiste;

    //private Text texto;

    // Update is called once per frame


    void Update()
    {
        if (target != null)
        {
            // Seguir al jugador
            transform.position = new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z);
        }
        

    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void setPunctuationText(int puntuacion)
    {
        punctuationText.text = $"Puntuacion: {puntuacion}";
    }

    public void addPlayerToPanel(string name, int id, int puntuacion)
    {
        texts[id].text = $"{name} : {puntuacion}";

    }

    public void setCorrectoActive()
    {
        Correcto.SetActive(true);
    }

    public void setCorrectoInactive()
    {
        Correcto.SetActive(false);
    }
    public void setIncorrectoActive()
    {
        Incorrecto.SetActive(true);
    }

    public void setIncorrectoInactive()
    {
        Incorrecto.SetActive(false);
    }

    public void unlockImage(string animalName)
    {
        foreach (Image image in Images)
        {
            if(image.name == animalName)
            {
                image.color = Color.white;
            }
        }
    }

    public IEnumerator mostrarGanaste()
    {
        Ganaste.SetActive(true);

        yield return new WaitForSeconds(5f);

        Ganaste.SetActive(false);
    }

    public IEnumerator mostrarPerdiste()
    {
        Perdiste.SetActive(true);

        yield return new WaitForSeconds(5f);

        Perdiste.SetActive(false);
    }

}

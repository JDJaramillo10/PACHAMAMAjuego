using RiptideNetworking;
using System.Collections.Generic;
using UnityEngine;

public class AnimalManager : MonoBehaviour
{
    [SerializeField] public List<AnimalPathFinding> animals;

    private static AnimalManager _singleton;

    public static AnimalManager Singleton
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
                Debug.Log($"{nameof(AnimalManager)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }
    void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {

    }

    private class AnimalMovementMessage
    {
        public int AnimalIndex;
        public Vector2 NewPosition;
        public float FacingDir;
        public bool IsRunning;
        public bool messageReceived;
    }

    private static Queue<AnimalMovementMessage> movementQueue = new Queue<AnimalMovementMessage>();


    private void Update()
    {
        while (movementQueue.Count > 0)
        {
            var message = movementQueue.Dequeue(); // Procesa el siguiente mensaje en la cola
            MoveAnimal(message.AnimalIndex, message.NewPosition, message.FacingDir, message.IsRunning, message.messageReceived);
        }
    }

    private void MoveAnimal(int animalIndex, Vector2 newPosition, float facingDirection,bool isRunning, bool messageReceived)
    {
        if (animalIndex < 0 || animalIndex >= animals.Count)
        {
            Debug.LogWarning($"Animal index {animalIndex} fuera de rango");
            return;
        }

        AnimalPathFinding animal = animals[animalIndex];
        //Debug.Log($"Moviendo animal {animalIndex} a {newPosition}");

        animal.MoveTo(newPosition, facingDirection,messageReceived);


        if (!isRunning)
        {
            animal.StopMoving();
        }

    }

    public string animalNameByIndex(int index)
    {
        if (index < 0 || index >= animals.Count) // Validar si el índice está dentro del rango
        {
            Debug.LogError("Índice fuera de rango");
            return null; // O puedes retornar una cadena vacía: ""
        }

        string name = animals[index].name;
        return name;
    }


    [MessageHandler((ushort)ServerToClientId.animalMovement)]
    private static void HandleAnimalMovement(Message message)
    {
        var movementMessage = new AnimalMovementMessage
        {
            AnimalIndex = message.GetInt(),
            NewPosition = message.GetVector2(),
            FacingDir = message.GetFloat(),
            IsRunning = message.GetBool(),
            messageReceived = true
        };

        movementQueue.Enqueue(movementMessage);
    }

    


}

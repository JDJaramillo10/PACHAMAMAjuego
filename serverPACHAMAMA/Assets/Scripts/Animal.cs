using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using RiptideNetworking;

public class Animal : MonoBehaviour
{

    [SerializeField] private List<AnimalPathFinding> animals;

    private static Animal _singleton;

    public static Animal Singleton
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
                Debug.Log($"{nameof(Animal)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }
    private enum State
    {
        Roaming
    }

    private State state;

    private void Awake()
    {

        state = State.Roaming;
    }

    private void Start()
    {
        for (int i = 0; i < animals.Count; i++)
        {
            animals[i].Initialize(i);
            Debug.Log($"animal: {animals[i].name} numero: {i}");
        }

        StartCoroutine(RoamingRoutine());

    }

    private IEnumerator RoamingRoutine()
    {
        while (state == State.Roaming)
        {
            foreach (var animal in animals)
            {
                animal.StopMoving();
                yield return new WaitForSeconds(3f);

                Vector2 roamPosition = GetRoamingPosition();

                animal.MoveTo(roamPosition);
                Debug.Log($"animal: {animal.name} moviendose a: {roamPosition}");

                yield return new WaitForSeconds(2f);

            }
        }
    }

    private Vector2 GetRoamingPosition()
    {
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }


}

using System.Collections;
using UnityEngine;

public class Animal : MonoBehaviour
{
    private enum State{
        Roaming
    }

    private State state;
    private AnimalPathFinding animalPathFinding;

    private void Awake(){
        animalPathFinding = GetComponent<AnimalPathFinding>();
        state = State.Roaming;
    }

    private void Start(){
        StartCoroutine(RoamingRoutine());
    }

    private IEnumerator RoamingRoutine(){
        while(state == State.Roaming){
            Vector2 roamPosition = GetRoamingPosition();
            animalPathFinding.MoveTo(roamPosition);
            yield return new WaitForSeconds(2f);
        }
    }

    private Vector2 GetRoamingPosition(){
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    
}

using RiptideNetworking;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Animal : MonoBehaviour
{
    /*    
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator myAnimator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        myAnimator = GetComponent<Animator>();
    }

    public void MoveTo(Vector3 targetPosition, bool isRunning)
    {

        rb.MovePosition(targetPosition);
        myAnimator.SetBool("isRunning", isRunning);

        if (targetPosition.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (targetPosition.x > 0)
        {
            spriteRenderer.flipX = false;
        }

    }*/
    /*
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

            animalPathFinding.StopMoving();

            yield return new WaitForSeconds(3f);

            Vector2 roamPosition = GetRoamingPosition();
            animalPathFinding.MoveTo(roamPosition);
            yield return new WaitForSeconds(2f);
        }
    }

    private Vector2 GetRoamingPosition(){
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }*/

    /*
    #region Messages

    [MessageHandler((ushort)ServerToClientId.animalMovement)]
    private static void PlayerMovement(Message message)
    {
        
        //MoveTo(message.GetVector3(), message.GetBool());

    }

    #endregion*/

}

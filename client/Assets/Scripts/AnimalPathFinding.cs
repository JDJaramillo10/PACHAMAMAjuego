using System.Collections;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class AnimalPathFinding : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Vector2 moveDir;
    private Animator myAnimator;
    private bool messageReceived;


    private void Awake(){
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        myAnimator = GetComponent<Animator>();
    }

    private void FixedUpdate(){

        if (messageReceived)
        {
            rb.MovePosition(moveDir);
            messageReceived = false;
        }
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

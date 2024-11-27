using System.Numerics;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class AnimalPathFinding : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;

    private Rigidbody2D rb;
    private UnityEngine.Vector2 moveDir;

    private void Awake(){
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate(){
        rb.MovePosition(rb.position + moveDir * (moveSpeed * Time.fixedDeltaTime));
    }

    public void MoveTo(UnityEngine.Vector2 targetPosition){
        moveDir = targetPosition;
    }
}

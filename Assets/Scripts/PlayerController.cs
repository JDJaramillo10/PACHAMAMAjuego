using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting.APIUpdating;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;

    private PlayerControls playerControls;
    private UnityEngine.Vector2 movement;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRender;

    private void Awake(){
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRender = GetComponent<SpriteRenderer>();
    }

    private void OnEnable(){
        playerControls.Enable();
    }

    private void Update(){
        PlayerInput();
    }

    private void FixedUpdate(){
        AdjustPlayerFacingDirection();
        Move();
    }

    private void PlayerInput(){
        movement = playerControls.Movement.Move.ReadValue<UnityEngine.Vector2>();

        myAnimator.SetFloat("moveX", movement.x);
        myAnimator.SetFloat("moveY", movement.y);
    }

    private void Move(){
        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }

    private void AdjustPlayerFacingDirection(){
        UnityEngine.Vector3 mousePos = Input.mousePosition;
        UnityEngine.Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        if(mousePos.x < playerScreenPoint.x){
            mySpriteRender.flipX = true;    
        }else{
            mySpriteRender.flipX = false;
        }


    }


}

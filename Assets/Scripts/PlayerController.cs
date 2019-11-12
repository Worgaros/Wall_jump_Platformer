using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour {
    private Rigidbody2D body;
    
    Vector2 direction;
    
    [SerializeField] float speed = 1;
    
    [SerializeField] float jumpForce = 5;
    [SerializeField] float raycastJumpLength = 0.6f;
    [SerializeField] float timeStopJump = 0.1f;
    float timerStopJump = 0f;
    [SerializeField] float jumpFallingModifier = 1.08f;
    [SerializeField] bool canJump = false;
    bool touchingWall = false;
    
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        direction = new Vector2(Input.GetAxis("Horizontal") * speed, body.velocity.y);
        timerStopJump -= Time.deltaTime;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, raycastJumpLength, 1 << LayerMask.NameToLayer("Objetcs"));
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.left, raycastJumpLength, 1 << LayerMask.NameToLayer("Wall"));
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.right, raycastJumpLength, 1 << LayerMask.NameToLayer("Wall"));

        if (timerStopJump <= 0) 
        {
            
            if (hit.rigidbody != null)
            {
                canJump = true;
            }
            else if (hitRight.rigidbody != null || hitLeft.rigidbody != null)
            {
                canJump = true;
                touchingWall = true;
            }
            else
            {
                canJump = false;
                touchingWall = false;
            }
        }
        
        if (hitRight.rigidbody != null && body.velocity.y < 0 || hitLeft.rigidbody != null && body.velocity.y < 0)
        {
           body.gravityScale = 0.2f;
        }
        else
        {
           body.gravityScale = 1;
        }

        if (Input.GetButtonDown("Jump") && canJump)
        {
            direction = new Vector2(body.velocity.x, jumpForce); 
            canJump = false; 
            timerStopJump = timeStopJump;
        }
        
        if (body.velocity.y < 0 && !touchingWall)
        {
            direction = new Vector2(body.velocity.x, body.velocity.y * jumpFallingModifier);
        }
        
        body.velocity = direction;
    }
    
    void OnDrawGizmos() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine((Vector2)transform.position, (Vector2)transform.position + Vector2.down * raycastJumpLength);
        Gizmos.DrawLine((Vector2)transform.position, (Vector2)transform.position + Vector2.left * raycastJumpLength);
        Gizmos.DrawLine((Vector2)transform.position, (Vector2)transform.position + Vector2.right * raycastJumpLength);
    }
}
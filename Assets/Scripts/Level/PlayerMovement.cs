using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D myRigidbody;
    [SerializeField] private Collider2D groundCheckCollider;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private int movementSpeed;
    [SerializeField] private int jumpForce;
    [SerializeField] private int doubleJumpforce;
    [SerializeField] float dashForce;
    [SerializeField] float dashDuration;

    private float movementVector;
    private bool usedDoubleJump;
    private bool dashing;
    private bool usedDash;

    private void Update()
    {
        if (dashing)
        {
            Vector3 dashVelocity = new Vector3(movementVector * dashForce, 0);
            transform.position += dashVelocity * Time.deltaTime;

            return;
        }
        HorizontalMovement();
        Jump();
        Dash();
        ValueReset();
    }

    private void HorizontalMovement()
    {
        // Create custom vector 
        if (ButtonHandler.Instance.rightPress == true || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            movementVector = 1;
        }
        else if (ButtonHandler.Instance.leftPress == true || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            movementVector = -1;
        }
        else
        {
            movementVector = 0;
        }

        // Making the player move
        Vector3 horizontalVelocity = new Vector3(movementVector * movementSpeed, myRigidbody.linearVelocity.y);
        transform.position += horizontalVelocity * Time.deltaTime;
    }

    private void Jump()
    {
        // UI
        if (isGrounded() && ButtonHandler.Instance.jumpPress == true)
        {
            Vector3 jumpVelocity = new Vector3(0, jumpForce);
            myRigidbody.linearVelocity = jumpVelocity;
            ButtonHandler.Instance.jumpPress = false;
        }
        else if (!isGrounded() && !usedDoubleJump && ButtonHandler.Instance.jumpPress == true)
        {
            Vector3 jumpVelocity = new Vector3(0, doubleJumpforce);
            myRigidbody.linearVelocity = jumpVelocity;
            ButtonHandler.Instance.jumpPress = false;
            usedDoubleJump = true;
        }

        // Keyboard
        if (isGrounded() && Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 jumpVelocity = new Vector3(0, jumpForce);
            myRigidbody.linearVelocity = jumpVelocity;
        }
        else if (!isGrounded() && !usedDoubleJump && Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 jumpVelocity = new Vector3(0, doubleJumpforce);
            myRigidbody.linearVelocity = jumpVelocity;
            usedDoubleJump = true;
        }
    }

    private void Dash()
    {
        // UI
        if (ButtonHandler.Instance.dashPress == true && !isGrounded() && !usedDash && movementVector != 0)
        {
            usedDash = true;
            ButtonHandler.Instance.dashPress = false;
            StartCoroutine(DashRoutine());
        }
        if (ButtonHandler.Instance.dashPress == true) ButtonHandler.Instance.dashPress = false;

        // Keyboard
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isGrounded() && !usedDash && movementVector != 0)
        {
            usedDash = true;
            StartCoroutine(DashRoutine());
        }
    }

    IEnumerator DashRoutine()
    {
        dashing = true;

        float gravScale = myRigidbody.gravityScale;
        myRigidbody.gravityScale = 0;
        myRigidbody.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(dashDuration);

        dashing = false;
        myRigidbody.gravityScale = gravScale;
    }

    private void ValueReset()
    {
        if (isGrounded())
        {
            usedDoubleJump = false;
            usedDash = false;
        }
    }

    private bool isGrounded()
    {
        if (groundCheckCollider.IsTouchingLayers(groundLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}

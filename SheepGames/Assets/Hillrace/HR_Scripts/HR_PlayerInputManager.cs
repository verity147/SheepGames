using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public partial class HR_Player : MonoBehaviour
{
    private void HandleDrinkingInput()
    {
        if (!Stun)
        {
            if (Input.GetButtonDown("Down") && drinkingAllowed)
            {
                myAnimator.SetTrigger("drink");
                myAnimator.SetBool("drinking", true);
                drinking = true;
                GetComponentInChildren<HR_PlayerCanvas>().Visible();
            }

            if (Input.GetButtonUp("Down"))
            {
                myAnimator.SetBool("drinking", false);
                drinking = false;
            }
        }
    }

    private void HandleMovementInput()
    {
        if (!drinking && !Stun)
        {
            if (Input.GetButtonDown("Left") || Input.GetButtonDown("Right"))
            {
                currentLerpTime = 0f;
            }

            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.01f)
            {
                ManageRunSpeed();
            }

            ///stop the player
            if (Input.GetButtonUp("Left") || Input.GetButtonUp("Right") || Input.GetButton("Left") && Input.GetButton("Right"))
            {
                myRigidbody.velocity = new Vector2(0f, myRigidbody.velocity.y);
            }

            if (Input.GetButtonDown("Jump") && IsGrounded)
            {
                if (isSwimming)
                {
                    PrepareJump(Jumpstate.swimming);
                }
                else if(!isSwimming && drinkTime > 0f)
                {
                    PrepareJump(Jumpstate.boosted);
                }
                else
                {
                    PrepareJump(Jumpstate.normal);
                }
            }
        }
        if (jump && (transform.position.y >= startY + jumpHeight || Input.GetButtonUp("Jump")))
        {
            jump = false;
            myRigidbody.gravityScale = normalGravity;
        }
    }
}

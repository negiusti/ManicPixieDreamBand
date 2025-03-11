using PixelCrushers.DialogueSystem;
using UnityEngine;
using Rewired;

public class PlayerMovement : Movement
{
    private float moveInput;
    private bool onRail;
    private Player player;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        player = ReInput.players.GetPlayer(0);
    }

    new void Update()
    {
        base.Update();
        if (currState == MovementState.Guitar || currState == MovementState.Drum)
        {
            return;
        }
        if (!Phone.Instance.IsLocked() || (DialogueManager.Instance.IsConversationActive && !DialogueManager.LastConversationStarted.EndsWith("_Purchase") && !DialogueManager.LastConversationStarted.EndsWith("_Sk8")))
        {
            animator.SetBool("IsMoving", false);
            currState = isSkating ? MovementState.SkateIdle :
                isRollerSkating ? MovementState.RollerskateIdle : MovementState.Idle;
            return;
        }

        // No skating outside and make sure you put on some shoes
        if (!isRollerSkating && player.GetButtonDown("Toggle Skateboard") && Tutorial.changedSkin && FindObjectsOfType<OutdoorLocation>().Length > 0)
        {
            if (!Tutorial.hasSkated) Tutorial.hasWalked = false;
            Tutorial.hasSkated = true;
            isSkating = !isSkating;
        }
        // No skating outside and make sure you put on some shoes
        if (!isSkating && player.GetButtonDown("Toggle Skates") && Tutorial.changedSkin && FindObjectsOfType<OutdoorLocation>().Length > 0)
        {
            if (!Tutorial.hasSkated) Tutorial.hasWalked = false;
            Tutorial.hasSkated = true;
            isRollerSkating = !isRollerSkating;
            RollerskatesOnOff(isRollerSkating);
        }

        moveInput = player.GetAxis("Move Horizontal");
        //moveInput = Input.GetAxis("Horizontal");

        if (player.GetButtonDown("Ollie") &&
            !lockAnim &&
             AllowedToMoveDuringConvo())
        {
            if (isSkating)
                Ollie();
            if (isRollerSkating)
                Rollie();
        }

        if (player.GetButtonDown("Kickflip"))
        {
            if (isSkating)
                Flip();
        }

        if (player.GetButtonDown("Manual"))
        {
            if (isSkating)
                Grind();
        }

        if (onLRamp && (isSkating || isRollerSkating))
        {
            moveInput = -.9f;
        }
        if (onRRamp && (isSkating || isRollerSkating))
        {
            moveInput = .9f;
        }

        animator.SetBool("IsMoving", moveInput != 0f);
        if (moveInput == 0f)
        {
            currState = isSkating? MovementState.SkateIdle :
                isRollerSkating? MovementState.RollerskateIdle : MovementState.Idle;
        }
        else
        {
            currState = isSkating ? MovementState.Skate :
                isRollerSkating? MovementState.Rollerskate : MovementState.Walk;
            MoveLeftRight();
        }
    }

    private void MoveLeftRight()
    {
        if (moveInput != 0) Tutorial.hasWalked = true;

        Quaternion currentRotation = transform.rotation;
        if (moveInput < 0 && currentRotation.y > 0)
        {
            currentRotation.y = 0;

        }
        else if (moveInput > 0 && currentRotation.y < 180)
        {
            currentRotation.y = 180;
        }

        Vector3 position = transform.position;
        float moveSpeed = isSkating || isRollerSkating ? skateMoveSpeed : walkMoveSpeed;
        if (SceneChanger.Instance.GetCurrentScene() == "HillBombMinigame" && moveInput > 0)
            moveSpeed *= 1.5f;

        position.x += moveInput * moveSpeed * Time.deltaTime;
        currentRotation.z = 0f;
        position.x = Mathf.Clamp(position.x, minX, maxX);
        transform.position = position;
        transform.rotation = currentRotation;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle") && IsSkating())
        {
            EatShit();
        }
        if (other.CompareTag("Seat"))
        {
            gameObject.transform.Translate(Vector3.down * .5f);
        }
        if (other.CompareTag("Rail") && isSkating)
        {
            gameObject.transform.Translate(Vector3.up * .5f);
            onRail = true;
        }
        if (other.CompareTag("LRamp") && moveInput < 0 && isSkating && !lockAnim)
        {
            onLRamp = true;
            animator.Play("BaseCharacter_SkateboardRamp");
        } else if (other.CompareTag("LRamp") && moveInput < 0 && isRollerSkating && !lockAnim)
        {
            onLRamp = true;
            animator.Play("BaseCharacter_RollerskateRamp");
        }
        if (other.CompareTag("RRamp") && moveInput > 0 && isSkating && !lockAnim)
        {
            onRRamp = true;
            animator.Play("BaseCharacter_SkateboardRampRight");
        }
        else if (other.CompareTag("RRamp") && moveInput > 0 && isRollerSkating && !lockAnim)
        {
            onRRamp = true;
            animator.Play("BaseCharacter_RollerskateRampRight");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Seat"))
        {
            gameObject.transform.Translate(Vector3.up * .5f);
        }
        if (other.CompareTag("Rail") && onRail)
        {
            gameObject.transform.Translate(Vector3.down * .5f);
            onRail = false;
        }

        if (other.CompareTag("LRamp"))
        {
            onLRamp = false;
        }
        if (other.CompareTag("RRamp"))
        {
            onRRamp = false;
        }
    }
}

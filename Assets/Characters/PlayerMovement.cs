using PixelCrushers.DialogueSystem;
using UnityEngine;

public class PlayerMovement : Movement
{
    private float moveInput;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    new void Update()
    {
        base.Update();
        if (currState == MovementState.Guitar || currState == MovementState.Drum)
        {
            return;
        }
        if (!Phone.Instance.IsLocked() || (DialogueManager.Instance.IsConversationActive && !DialogueManager.LastConversationStarted.EndsWith("_Purchase")))
        {
            animator.SetBool("IsMoving", false);
            currState = isSkating ? MovementState.SkateIdle :
                isRollerSkating ? MovementState.RollerskateIdle : MovementState.Idle;
            return;
        }

        if (!isRollerSkating && Input.GetKeyDown(KeyCode.S) && Tutorial.changedSkin) // Make sure you get dressed and put on shoes before skating
        {
            if (!Tutorial.hasSkated) Tutorial.hasWalked = false;
            Tutorial.hasSkated = true;
            isSkating = !isSkating;
        }
        if (!isSkating && Input.GetKeyDown(KeyCode.R) && Tutorial.changedSkin) // Make sure you get dressed and put on shoes before skating+
        {
            if (!Tutorial.hasSkated) Tutorial.hasWalked = false;
            Tutorial.hasSkated = true;
            isRollerSkating = !isRollerSkating;
            RollerskatesOnOff(isRollerSkating);
        }

        moveInput = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isSkating)
                Ollie();
            if (isRollerSkating)
                Rollie();
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
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Seat"))
        {
            gameObject.transform.Translate(Vector3.up * .5f);
        }
    }
}

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

        if (!isRollerSkating && Input.GetKeyDown(KeyCode.S))
        {
            isSkating = !isSkating;
        }
        if (!isSkating && Input.GetKeyDown(KeyCode.R))
        {
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
        Quaternion currentRotation = transform.localRotation;
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
    }
}

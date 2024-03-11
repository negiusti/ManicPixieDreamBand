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

        if (Input.GetKeyDown(KeyCode.S))
        {
            isSkating = !isSkating;
        }

        moveInput = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && isSkating)
        {
            Ollie();
        }

        animator.SetBool("IsMoving", moveInput != 0f);
        if (moveInput == 0f)
        {
            currState = isSkating? MovementState.SkateIdle : MovementState.Idle;
        }
        else
        {
            currState = isSkating ? MovementState.Skate : MovementState.Walk;
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
        float moveSpeed = isSkating ? skateMoveSpeed : walkMoveSpeed;
        position.x += moveInput * moveSpeed * Time.deltaTime;
        position.x = Mathf.Clamp(position.x, minX, maxX);
        transform.position = position;
        transform.rotation = currentRotation;
    }
}

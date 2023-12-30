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
        if (currState == MovementState.Guitar || currState == MovementState.Drums)
        {
            return;
        }

        moveInput = Input.GetAxis("Horizontal");
        if (moveInput == 0f)
        {
            prevState = currState;
            currState = MovementState.Idle;
        }
        else
        {
            prevState = currState;
            currState = MovementState.Walk;
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
        position.x += moveInput * moveSpeed * Time.deltaTime;
        position.x = Mathf.Clamp(position.x, minX, maxX);
        transform.position = position;
        transform.rotation = currentRotation;
    }
}

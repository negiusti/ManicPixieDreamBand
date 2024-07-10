using UnityEngine;

public class NPCMovement : Movement
{
    private float targetX;
    private bool moving;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    new void Update()
    {
        base.Update();
        float currentX = transform.position.x;
        Debug.Log("LOOK HERE: "+ currState.ToString() + " " + currentX + " " + targetX);
        if (moving && Mathf.Abs(currentX - targetX) > 0.1f)
        {
            Debug.Log("Walking: " + currentX + " " + targetX);
            WalkToTargetX();
        } else if (moving)
        {
            currState = MovementState.Idle;
            moving = false;
        }
    }

    public void WalkTo(float x)
    {
        moving = true;
        targetX = x;
        Debug.Log("Walking to: " + transform.position.x + " " + targetX);
    }

    //public new void PlayInstrument(string instLabel, float xPos)
    //{
    //    WalkTo(xPos);
    //    base.PlayInstrument(instLabel, xPos);        
    //}

    private void WalkToTargetX()
    {
        currState = MovementState.Walk;
        Quaternion currentRotation = transform.localRotation;
        float currentX = transform.position.x;
        float moveDirection = currentX > targetX ? -1 : 1;
        Debug.Log("currentX: " + currentX + " targetX: " + targetX);
        if (currentX > targetX)
        {
            currentRotation.y = 0;
            Debug.Log("currentRotation.y: " + currentRotation.y);
        }
        else
        {
            currentRotation.y = 180;
            Debug.Log("currentRotation.y: " + currentRotation.y);
        }

        Vector3 position = transform.position;
        float moveSpeed = isSkating || isRollerSkating ? skateMoveSpeed : walkMoveSpeed;
        position.x += moveDirection * moveSpeed * Time.deltaTime;
        //position.x = Mathf.Clamp(position.x, minX, maxX);
        transform.position = position;
        transform.localRotation = currentRotation;
        //Debug.Log("currentRotation.y: " + currentRotation.y);
    }
}

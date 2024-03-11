using UnityEngine;

public class NPCMovement : Movement
{
    private float targetX;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        targetX = this.transform.position.x;
    }

    new void Update()
    {
        base.Update();
        //float currentX = transform.position.x;
        //if (Mathf.Abs(currentX - targetX) > 0.1f)
        //{
        //    WalkToTargetX();
        //}
    }

    public void WalkTo(float x)
    {
        targetX = x;
    }

    //public new void PlayInstrument(string instLabel, float xPos)
    //{
    //    WalkTo(xPos);
    //    base.PlayInstrument(instLabel, xPos);        
    //}

    private void WalkToTargetX()
    {
        Quaternion currentRotation = transform.localRotation;
        float currentX = transform.position.x;
        float moveDirection = currentX > targetX ? 1 : -1;
        if (currentX > targetX && currentRotation.y > 0)
        {
            currentRotation.y = 0;

        }
        else if (currentX < targetX && currentRotation.y < 180)
        {
            currentRotation.y = 180;
        }

        Vector3 position = transform.position;
        float moveSpeed = isSkating ? skateMoveSpeed : walkMoveSpeed;
        position.x += moveDirection * moveSpeed * Time.deltaTime;
        position.x = Mathf.Clamp(position.x, minX, maxX);
        transform.position = position;
        transform.rotation = currentRotation;
    }
}

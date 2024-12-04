using System.Collections;
using UnityEngine;

public class NPCMovement : Movement
{
    private float targetX;
    private bool walking;
    private bool skating;
    private Coroutine skateRoutine;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    new void Update()
    {
        base.Update();
        float currentX = transform.localPosition.x;
        //Debug.Log("LOOK HERE: "+ currState.ToString() + " " + currentX + " " + targetX);
        
        if (walking && Mathf.Abs(currentX - targetX) > 0.5f)
        {
            //Debug.Log("Walking: " + currentX + " " + targetX);
            WalkToTargetX();
            //Debug.Log("transform.rotation.eulerAngles.y: " + transform.rotation.eulerAngles.y);
        } else if (walking)
        {
            currState = MovementState.Idle;
            walking = false;
        } else if (skating && Mathf.Abs(currentX - targetX) > 0.5f)
        {
            Debug.Log("Skating: " + currentX + " " + targetX);
            SkateToTargetX();
            //Debug.Log("transform.rotation.eulerAngles.y: " + transform.rotation.eulerAngles.y);
        }
        else if (skating)
        {
            currState = MovementState.SkateIdle;
            skating = false;
        }
    }

    public void WalkTo(float x)
    {
        walking = true;
        targetX = x;
        Debug.Log("Walking to: " + transform.position.x + " " + targetX);
    }

    public void SkateTo(float x)
    {
        skating = true;
        targetX = x;
        Debug.Log("Skating to: " + transform.position.x + " " + targetX);
    }

    public void StopSkating()
    {
        if (skateRoutine != null)
        {
            StopCoroutine(skateRoutine);
            skateRoutine = null;
        }

        skating = false;
        walking = false;
        currState = MovementState.Idle;
    }

    public void SkateBetween(float minx, float maxx, float seconds)
    {
        skateRoutine = StartCoroutine(SkateBetweenFor(minx, maxx, seconds));
    }

    private IEnumerator SkateBetweenFor(float minx, float maxx, float seconds)
    {
        skating = true;
        targetX = minx;
        float timeElapsed = 0f;
        float timeElapsedSinceTrick = 0f;
        bool alternateTricks = false;

        while (timeElapsed < seconds)
        {
            float currentX = transform.position.x;
            if (Mathf.Abs(currentX - minx) < 0.5f)
            {
                SkateTo(maxx);
            } else if (Mathf.Abs(currentX - maxx) < 0.5f)
            {
                SkateTo(minx);
            }
            if (timeElapsedSinceTrick > 2f)
            {
                timeElapsedSinceTrick = 0f;
                if (alternateTricks)
                    Ollie();
                else
                    Grind();
                alternateTricks = !alternateTricks;
            }
            timeElapsedSinceTrick += 0.5f;
            timeElapsed += 0.5f;
            yield return new WaitForSeconds(0.5f);
        }
        StopSkating();
        yield return null;
    }

    //public new void PlayInstrument(string instLabel, float xPos)
    //{
    //    WalkTo(xPos);
    //    base.PlayInstrument(instLabel, xPos);        
    //}

    private void WalkToTargetX()
    {
        currState = MovementState.Walk;
        Quaternion currentRotation = transform.rotation;
        float currentX = transform.position.x;
        float moveDirection = currentX > targetX ? -1 : 1;
        Debug.Log("currentX: " + currentX + " targetX: " + targetX);
        if (moveDirection < 0f && currentRotation.eulerAngles.y > 0f)
        {
            currentRotation.eulerAngles = new Vector3(0f, 0f, 0f);

        }
        else if (moveDirection > 0f && currentRotation.eulerAngles.y < 180f)
        {
            currentRotation.eulerAngles = new Vector3(0f, 180f, 0f);
        }

        Vector3 position = transform.position;
        float moveSpeed = walkMoveSpeed;
        position.x += moveDirection * moveSpeed * Time.deltaTime;
        //position.x = Mathf.Clamp(position.x, minX, maxX);
        transform.position = position;
        //transform.rotation = Quaternion.Euler(0f, currentRotation.y, 0f);
        transform.rotation = currentRotation;
        Debug.Log("transform.rotation.eulerAngles.y: " + transform.rotation.eulerAngles.y);
    }

    private void SkateToTargetX()
    {
        currState = MovementState.Skate;
        Quaternion currentRotation = transform.rotation;
        float currentX = transform.localPosition.x;
        float moveDirection = currentX > targetX ? -1 : 1;
        Debug.Log("currentX: " + currentX + " targetX: " + targetX);
        if (moveDirection < 0f && currentRotation.eulerAngles.y > 0f)
        {
            currentRotation.eulerAngles = new Vector3(0f, 0f, 0f);

        }
        else if (moveDirection > 0f && currentRotation.eulerAngles.y < 180f)
        {
            currentRotation.eulerAngles = new Vector3(0f, 180f, 0f);
        }

        Vector3 position = transform.localPosition;
        float moveSpeed = skateMoveSpeed;
        position.x += moveDirection * moveSpeed * Time.deltaTime;
        //position.x = Mathf.Clamp(position.x, minX, maxX);
        transform.localPosition = position;
        //transform.rotation = Quaternion.Euler(0f, currentRotation.y, 0f);
        transform.rotation = currentRotation;
        Debug.Log("transform.rotation.eulerAngles.y: " + transform.rotation.eulerAngles.y);
    }
}

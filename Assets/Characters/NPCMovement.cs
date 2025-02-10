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
            //Debug.Log("Skating: " + currentX + " " + targetX);
            SkateToTargetX();
            //Debug.Log("transform.rotation.eulerAngles.y: " + transform.rotation.eulerAngles.y);
        }
        else if (skating)
        {
            currState = MovementState.SkateIdle;
            skating = false;
        }

        animator.SetBool("IsMoving", walking || skating);
    }

    public void WalkTo(float x)
    {
        walking = true;
        targetX = x;
        //Debug.Log("Walking to: " + transform.position.x + " " + targetX);
    }

    public void SkateTo(float x)
    {
        skating = true;
        targetX = x;
        isSkating = true;
        currState = MovementState.Skate;
        //Debug.Log("Skating to: " + transform.position.x + " " + targetX);
    }

    public void StopSkating()
    {
        if (skateRoutine != null)
        {
            StopCoroutine(skateRoutine);
            skateRoutine = null;
        }

        skating = false;
        isSkating = false;
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
         
        while (timeElapsed < seconds || seconds < 0)
        {
            float currentX = transform.position.x;
            if (Mathf.Abs(currentX - minx) < 0.5f)
            {
                SkateTo(maxx);
            } else if (Mathf.Abs(currentX - maxx) < 0.5f)
            {
                SkateTo(minx);
            }
            if (timeElapsedSinceTrick > 5f && !lockAnim)
            {
                timeElapsedSinceTrick = 0f;
                if (alternateTricks)
                    Flip();
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

    void OnTriggerEnter2D(Collider2D other)
    {
        float currentX = transform.position.x;
        float moveDirection = currentX > targetX ? -1 : 1;
        if (other.CompareTag("Seat"))
        {
            gameObject.transform.Translate(Vector3.down * .5f);
        }
        else if (other.CompareTag("Obstacle") && skating && !lockAnim)
        {
            // Rex is too afraid to kickflip due to their injuries, remember?
            if (character.CharacterName() == "Rex")
            {
                Ollie();
                return;
            }
            switch (Random.Range(0, 2))
            {
                case 0:
                    Ollie();
                    break;
                case 1:
                    Flip();
                    break;
            }
        }
        //else if (other.CompareTag("LRamp") && isSkating && !lockAnim && moveDirection < 0)
        //{
        //    onLRamp = true;
        //    animator.Play("BaseCharacter_SkateboardRamp");
        //}
        //else if (other.CompareTag("LRamp") && isRollerSkating && !lockAnim && moveDirection < 0)
        //{
        //    onLRamp = true;
        //    animator.Play("BaseCharacter_RollerskateRamp");
        //}
        else if (other.CompareTag("RRamp") && isSkating && !lockAnim && moveDirection > 0)
        {
            onRRamp = true;
            animator.Play("BaseCharacter_SkateboardRampRight");
        }
        else if (other.CompareTag("RRamp") && isRollerSkating && !lockAnim && moveDirection > 0)
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
        if (other.CompareTag("LRamp"))
        {
            onLRamp = false;
        }
        if (other.CompareTag("RRamp"))
        {
            onRRamp = false;
        }
    }

    private void WalkToTargetX()
    {
        currState = MovementState.Walk;
        Quaternion currentRotation = transform.rotation;
        float currentX = transform.position.x;
        float moveDirection = currentX > targetX ? -1 : 1;
        //Debug.Log("currentX: " + currentX + " targetX: " + targetX);
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
        //Debug.Log("transform.rotation.eulerAngles.y: " + transform.rotation.eulerAngles.y);
    }

    private void SkateToTargetX()
    {
        currState = MovementState.Skate;
        Quaternion currentRotation = transform.rotation;
        float currentX = transform.localPosition.x;
        float moveDirection = currentX > targetX ? -1 : 1;
        //Debug.Log("currentX: " + currentX + " targetX: " + targetX);
        if (moveDirection < 0f && currentRotation.eulerAngles.y > 0f)
        {
            currentRotation.eulerAngles = new Vector3(0f, 0f, 0f);

        }
        else if (moveDirection > 0f && currentRotation.eulerAngles.y < 180f)
        {
            currentRotation.eulerAngles = new Vector3(0f, 180f, 0f);
        }

        Vector3 position = transform.localPosition;
        float moveSpeed = skateMoveSpeed * Random.Range(1f, 1.5f);
        position.x += moveDirection * moveSpeed * Time.deltaTime;
        //position.x = Mathf.Clamp(position.x, minX, maxX);
        transform.localPosition = position;
        //transform.rotation = Quaternion.Euler(0f, currentRotation.y, 0f);
        transform.rotation = currentRotation;
        //Debug.Log("transform.rotation.eulerAngles.y: " + transform.rotation.eulerAngles.y);
    }
}

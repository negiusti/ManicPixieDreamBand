using UnityEngine;
//using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private float moveSpeed = 4f;
    private float moveInput;
    private Character player;
    private Animator animator;    
    private float minX, maxX;
    public GameObject background;
    

    public enum MovementState
    {
        Walk,
        Idle,
        Guitar,
        Drums
    };

    private MovementState currState;
    private MovementState prevState;

    private bool HasStateChanged()
    {
        return currState != prevState;
    }

    void Update()
    {
        if (HasStateChanged())
        {
            if (currState == MovementState.Guitar)
            {
                animator.CrossFade("BaseCharacter_Guitar", .05f);
                return;
            }
            else if (currState == MovementState.Drums)
            {
                animator.CrossFade("BaseCharacter_Drum", .05f);
                return;
            }
            else if (currState == MovementState.Idle)
            {
                animator.CrossFade("BaseCharacter_Idle", .05f);
            }
            else if (currState == MovementState.Walk)
            {
                animator.CrossFade("BaseCharacter_Walk", .05f);
            }
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
        Quaternion currentRotation = player.transform.localRotation;
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

    public void PlayInstrument(string instLabel)
    {
        prevState = currState;
        player.SetInstrumentSprite(instLabel);
        if (instLabel.Contains("Guitar") || instLabel.Contains("Bass"))
            currState = MovementState.Guitar;
        else if (instLabel.Contains("Drums"))
            currState = MovementState.Drums;
    }

    public void StopPlayingInstrument()
    {
        prevState = currState;
        currState = MovementState.Idle;
        player.HideInstrumentSprite();
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Character>();
        animator = GetComponentInChildren<Animator>();
        currState = MovementState.Idle;
        if (background != null)
        {
            // Calculate the movement bounds based on the background's collider
            Bounds backgroundBounds = background.GetComponent<Collider2D>().bounds;
            float objectHalfWidth = GetComponent<Collider2D>().bounds.extents.x;

            minX = backgroundBounds.min.x + objectHalfWidth;
            maxX = backgroundBounds.max.x - objectHalfWidth;
        }
    }

    //private void ChangeChildSortingLayers(Transform parent)
    //{
    //    string targetSortingLayer = "Player";
    //    // Iterate through all child objects
    //    foreach (Transform child in parent)
    //    {
    //        // Get the Sprite Renderer component of the child object
    //        SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();

    //        if (spriteRenderer != null)
    //        {
    //            // Change the sorting layer to the target sorting layer
    //            spriteRenderer.sortingLayerName = targetSortingLayer;
    //        }

    //        // Recursively call the function for nested child objects
    //        ChangeChildSortingLayers(child);
    //    }
    //}

}

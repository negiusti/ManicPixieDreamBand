using UnityEngine;

public abstract class Movement : MonoBehaviour
{
    protected static float moveSpeed = 4f;
    protected Character player;
    protected Animator animator;
    protected float minX, maxX;
    protected GameObject background;
    protected float prevYPos;

    public enum MovementState
    {
        Walk,
        Idle,
        Guitar,
        Drums
    };

    protected MovementState currState;
    protected MovementState prevState;

    // Use this for initialization
    protected virtual void Start()
    {
        player = GetComponent<Character>();
        animator = GetComponentInChildren<Animator>();
        currState = MovementState.Idle;
        background = GameObject.FindGameObjectWithTag("Background");

        if (background != null)
        {
            // Calculate the movement bounds based on the background's collider
            Bounds backgroundBounds = background.GetComponent<Collider2D>().bounds;
            float objectHalfWidth = GetComponent<Collider2D>().bounds.extents.x;

            minX = backgroundBounds.min.x + objectHalfWidth;
            maxX = backgroundBounds.max.x - objectHalfWidth;
        }
        prevYPos = transform.position.y;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (HasStateChanged())
        {
            if (currState == MovementState.Guitar)
            {
                animator.CrossFade("BaseCharacter_Guitar", .05f);
            }
            else if (currState == MovementState.Drums)
            {
                animator.CrossFade("BaseCharacter_Drum", .05f);
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
    }

    public void PlayInstrument(string instLabel, float xPos)
    {
        prevState = currState;
        player.SetInstrumentSprite(instLabel);
        if (instLabel.Contains("Guitar") || instLabel.Contains("Bass"))
            currState = MovementState.Guitar;
        else if (instLabel.Contains("Drums"))
            currState = MovementState.Drums;
        transform.position = new Vector3(xPos, transform.position.y + 1f, transform.position.z);
    }

    public void StopPlayingInstrument()
    {
        prevState = currState;
        currState = MovementState.Idle;
        player.HideInstrumentSprite();
        transform.position = new Vector3(transform.position.x, prevYPos, transform.position.z);
    }

    private bool HasStateChanged()
    {
        return currState != prevState;
    }
}

using System.Collections.Generic;
using UnityEngine;

public abstract class Movement : MonoBehaviour
{
    protected static float walkMoveSpeed = 4f;
    protected static float skateMoveSpeed = 8f;
    protected Character player;
    protected Animator animator;
    protected float minX, maxX;
    protected GameObject background;
    protected Vector3 prevPos;
    protected string prevLayer;
    protected int prevLayerOrder;
    protected bool isSkating;

    public enum MovementState
    {
        Walk,
        Idle,
        Guitar,
        Drum,
        Skate,
        SkateIdle
    };

    private static string WalkAnim = "BaseCharacter_Walk";
    private static string IdleAnim = "BaseCharacter_Idle";
    private static string GuitarAnim = "BaseCharacter_Guitar";
    private static string DrumAnim = "BaseCharacter_Drum";
    private static string SkateAnim = "BaseCharacter_Skateboard";
    private static string SkateIdleAnim = "BaseCharacter_SkateboardIdle";

    private Dictionary<MovementState, string> stateToAnimation = new Dictionary<MovementState, string> {
        {MovementState.Walk, WalkAnim },
        {MovementState.Idle, IdleAnim },
        {MovementState.Guitar, GuitarAnim },
        {MovementState.Drum, DrumAnim },
        {MovementState.Skate, SkateAnim },
        {MovementState.SkateIdle, SkateIdleAnim }
            };
    protected MovementState currState;
    private string currAnim;

    // Use this for initialization
    protected virtual void Start()
    {
        isSkating = false;
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
        prevPos = transform.position;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (HasStateChanged())
        {
            if (currState == MovementState.Guitar)
            {
                currAnim = GuitarAnim;
            }
            else if (currState == MovementState.Drum)
            {
                currAnim = DrumAnim;
            }
            else if (currState == MovementState.Idle)
            {
                currAnim = IdleAnim;
            }
            else if (currState == MovementState.Walk)
            {
                currAnim = WalkAnim;
            } else if (currState == MovementState.Skate)
            {
                currAnim = SkateAnim;
            }
            else if (currState == MovementState.SkateIdle)
            {
                currAnim = SkateIdleAnim;
            }
            animator.CrossFade(currAnim, .05f);
        }
    }

    public void PlayInstrument(string instLabel, Vector3 pos, string layer, int layerOrder)
    {
        player.SetInstrumentSprite(instLabel);
        if (instLabel.Contains("Guitar") || instLabel.Contains("Bass"))
            currState = MovementState.Guitar;
        else if (instLabel.Contains("Drum"))
            currState = MovementState.Drum;

        //Quaternion currentRotation = transform.localRotation;
        //currentRotation.y = 0;
        prevPos = transform.position;
        transform.position = pos;
        prevLayer = player.GetCurrentLayer().sortingLayerName;
        prevLayerOrder = player.GetCurrentLayer().sortingOrder;
        player.MoveToRenderLayer(layer, layerOrder);
        //transform.rotation = currentRotation;
    }

    public void StopPlayingInstrument()
    {
        currState = MovementState.Idle;
        player.HideInstrumentSprite();
        transform.position = prevPos;
        player.MoveToRenderLayer(prevLayer, prevLayerOrder);
    }

    protected void Ollie()
    {
        animator.Play("BaseCharacter_SkateJump");
    }

    private bool HasStateChanged()
    {
        return stateToAnimation[currState] != currAnim;
    }
}

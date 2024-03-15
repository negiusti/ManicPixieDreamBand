using System.Collections.Generic;
using UnityEngine;

public abstract class Movement : MonoBehaviour
{
    protected static float walkMoveSpeed = 8f;
    protected static float skateMoveSpeed = 16f;
    protected Character character;
    protected Animator animator;
    protected float minX, maxX;
    protected GameObject background;
    protected Vector3 prevPos;
    protected string prevLayer;
    protected int prevLayerOrder;
    protected bool isSkating;
    protected bool isRollerSkating;

    public enum MovementState
    {
        Walk,
        Idle,
        Guitar,
        Drum,
        Skate,
        SkateIdle,
        Rollerskate,
        RollerskateIdle
    };

    private static string WalkAnim = "BaseCharacter_Walk";
    private static string IdleAnim = "BaseCharacter_Idle";
    private static string GuitarAnim = "BaseCharacter_Guitar";
    private static string DrumAnim = "BaseCharacter_Drum";
    private static string SkateAnim = "BaseCharacter_Skateboard";
    private static string SkateIdleAnim = "BaseCharacter_SkateboardIdle";
    private static string RollerskateAnim = "BaseCharacter_Rollerskate";
    private static string RollerskateIdleAnim = "BaseCharacter_RollerskateIdle";

    private Dictionary<MovementState, string> stateToAnimation = new Dictionary<MovementState, string> {
        {MovementState.Walk, WalkAnim },
        {MovementState.Idle, IdleAnim },
        {MovementState.Guitar, GuitarAnim },
        {MovementState.Drum, DrumAnim },
        {MovementState.Skate, SkateAnim },
        {MovementState.SkateIdle, SkateIdleAnim },
        {MovementState.Rollerskate, RollerskateAnim },
        {MovementState.RollerskateIdle, RollerskateIdleAnim }
            };
    protected MovementState currState;
    private string currAnim;

    // Use this for initialization
    protected virtual void Start()
    {
        isSkating = false;
        isRollerSkating = false;
        character = GetComponent<Character>();
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

    public bool IsSkating()
    {
        return isSkating || isRollerSkating;
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
            }
            else if (currState == MovementState.Skate)
            {
                currAnim = SkateAnim;
            }
            else if (currState == MovementState.SkateIdle)
            {
                currAnim = SkateIdleAnim;
            }
            else if (currState == MovementState.Rollerskate)
            {
                currAnim = RollerskateAnim;
            }
            else if (currState == MovementState.RollerskateIdle)
            {
                currAnim = RollerskateIdleAnim;
            }
            animator.CrossFade(currAnim, .05f);
        }
    }

    public void PlayInstrument(string instLabel, Vector3 pos, string layer, int layerOrder)
    {
        character.SetInstrumentSprite(instLabel);
        if (instLabel.Contains("Guitar") || instLabel.Contains("Bass"))
            currState = MovementState.Guitar;
        else if (instLabel.Contains("Drum"))
            currState = MovementState.Drum;

        //Quaternion currentRotation = transform.localRotation;
        //currentRotation.y = 0;
        prevPos = transform.position;
        transform.position = pos;
        prevLayer = character.GetCurrentLayer().sortingLayerName;
        prevLayerOrder = character.GetCurrentLayer().sortingOrder;
        character.MoveToRenderLayer(layer, layerOrder);
        //transform.rotation = currentRotation;
    }

    public void StopPlayingInstrument()
    {
        currState = MovementState.Idle;
        character.HideInstrumentSprite();
        transform.position = prevPos;
        character.MoveToRenderLayer(prevLayer, prevLayerOrder);
    }

    protected void Ollie()
    {
        animator.Play("BaseCharacter_SkateJump");
    }

    protected void Rollie()
    {
        animator.Play("BaseCharacter_RollerskateJump");
    }

    protected void EatShit()
    {
        if (isRollerSkating)
           animator.Play("BaseCharacter_RollerskateFall");
        if (isSkating)
           animator.Play("BaseCharacter_SkateboardFall");
    }

    private bool HasStateChanged()
    {
        return stateToAnimation[currState] != currAnim;
    }


    public void RollerskatesOnOff(bool isRollerskating)
    {
        character.RollerskatesOnOff(isRollerskating);
    }
}

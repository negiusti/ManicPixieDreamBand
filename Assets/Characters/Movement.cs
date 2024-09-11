using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
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
    protected Quaternion prevRotation;
    protected string prevLayer;
    protected int prevLayerOrder;
    protected bool isSkating;
    protected bool isRollerSkating;
    protected bool lockAnim;

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
        //Drink
    };

    private static string WalkAnim = "BaseCharacter_Walk";
    private static string IdleAnim = "BaseCharacter_Idle";
    private static string GuitarAnim = "BaseCharacter_Guitar";
    private static string DrumAnim = "BaseCharacter_Drum";
    private static string SkateAnim = "BaseCharacter_Skateboard";
    private static string SkateIdleAnim = "BaseCharacter_SkateboardIdle";
    private static string RollerskateAnim = "BaseCharacter_Rollerskate";
    private static string RollerskateIdleAnim = "BaseCharacter_RollerskateIdle";
    private static string DrinkAnim = "BaseCharacter_Drink";

    private Dictionary<MovementState, string> stateToAnimation = new Dictionary<MovementState, string> {
        {MovementState.Walk, WalkAnim },
        {MovementState.Idle, IdleAnim },
        {MovementState.Guitar, GuitarAnim },
        {MovementState.Drum, DrumAnim },
        {MovementState.Skate, SkateAnim },
        {MovementState.SkateIdle, SkateIdleAnim },
        {MovementState.Rollerskate, RollerskateAnim },
        {MovementState.RollerskateIdle, RollerskateIdleAnim }
        //{MovementState.Drink, DrinkAnim }
            };
    protected MovementState currState;
    private string currAnim;

    // Use this for initialization
    protected virtual void Start()
    {
        lockAnim = false;
        isSkating = false;
        isRollerSkating = false;
        character = GetComponent<Character>();
        animator = GetComponentInChildren<Animator>();
        currState = MovementState.Idle;
        animator.Play(IdleAnim, -1, Random.Range(0f, 1f));
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

    public void LockAnim()
    {
        lockAnim = true;
    }

    public void UnlockAnim()
    {
        lockAnim = false;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (HasStateChanged() && !lockAnim)
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
            //else if (currState == MovementState.Drink)
            //{
            //    currAnim = DrinkAnim;
            //}
            animator.CrossFade(currAnim, .05f, -1, Random.Range(0f, 1f));
        }
    }

    public void Drink(string itemName)
    {
        //stateToAnimation[currState] != currAnim
        //lockAnim = true;
        character.SetHoldingSprite(itemName);
        currAnim = DrinkAnim;
        animator.Play("BaseCharacter_Drink", 2, 0f);
        MainCharacterState.SetFlag("Drank_" + itemName, true);
    }

    public void PlayInstrument(string instLabel, Vector3 pos, string layer, int layerOrder)
    {
        Quaternion currentRotation = transform.localRotation;
        prevRotation = transform.localRotation;

        prevPos = transform.position;
        Debug.Log("Prev pos:" + prevPos);

        transform.position = pos;

        prevLayer = character.GetCurrentLayer().sortingLayerName;
        prevLayerOrder = character.GetCurrentLayer().sortingOrder;
        Debug.Log("Prev order:" + prevLayer + prevLayerOrder);
        character.MoveToRenderLayer(layer, layerOrder);
        currentRotation.y = 180;
        transform.rotation = currentRotation;

        character.SetInstrumentSprite(instLabel);
        if (instLabel.Contains("Guitar") || instLabel.Contains("Bass"))
            currState = MovementState.Guitar;
        else if (instLabel.Contains("Drum"))
            currState = MovementState.Drum;
    }

    public void StopPlayingInstrument()
    {
        currState = MovementState.Idle;
        Debug.Log("Prev pos:" + prevPos);
        transform.position = prevPos;
        transform.rotation = prevRotation;
        character.MoveToRenderLayer(prevLayer, prevLayerOrder);
        character.HideInstrumentSprite();
    }

    protected void Ollie()
    {
        if (!lockAnim && !DialogueManager.IsConversationActive)
        {
            animator.Play("BaseCharacter_SkateJump");
            Tutorial.hasOllied = true;
        }
    }

    protected void Rollie()
    {
        if (!lockAnim && !DialogueManager.IsConversationActive)
        {
            animator.Play("BaseCharacter_RollerskateJump");
            Tutorial.hasOllied = true;
        }
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

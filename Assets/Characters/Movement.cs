﻿using System.Collections.Generic;
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
        Drum
    };

    private static string WalkAnim = "BaseCharacter_Walk";
    private static string IdleAnim = "BaseCharacter_Idle";
    private static string GuitarAnim = "BaseCharacter_Guitar";
    private static string DrumAnim = "BaseCharacter_Drum";

    private Dictionary<MovementState, string> stateToAnimation = new Dictionary<MovementState, string> {
        {MovementState.Walk, WalkAnim },
        {MovementState.Idle, IdleAnim },
        {MovementState.Guitar, GuitarAnim },
        {MovementState.Drum, DrumAnim }
            };
    protected MovementState currState;
    private string currAnim;

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
            animator.CrossFade(currAnim, .05f);
        }
    }

    public void PlayInstrument(string instLabel, Vector3 pos)
    {
        player.SetInstrumentSprite(instLabel);
        if (instLabel.Contains("Guitar") || instLabel.Contains("Bass"))
            currState = MovementState.Guitar;
        else if (instLabel.Contains("Drum"))
            currState = MovementState.Drum;

        Quaternion currentRotation = transform.localRotation;
        currentRotation.y = 0;
        transform.position = pos;// new Vector3(xPos, transform.position.y + 1f, transform.position.z);
        transform.rotation = currentRotation;
    }

    public void StopPlayingInstrument()
    {
        currState = MovementState.Idle;
        player.HideInstrumentSprite();
        transform.position = new Vector3(transform.position.x, prevYPos, transform.position.z);
    }

    private bool HasStateChanged()
    {
        return stateToAnimation[currState] != currAnim;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private float moveSpeed = 4f;
    private float moveInput;
    private Character player;
    private Animator animator;
    private bool isIdle = true;
    private string targetSortingLayer = "Player";
    private float minX, maxX;
    public GameObject background;

    //void Awake()
    //{
    //    DontDestroyOnLoad(gameObject);
    //}

    void Update()
    {
        moveInput = Input.GetAxis("Horizontal");

        if (moveInput == 0f && !isIdle)
        {
            animator.CrossFade("BaseCharacter_Idle", .1f);
            isIdle = true;
        }
        else if (moveInput != 0f && isIdle)
        {
            animator.CrossFade("BaseCharacter_Walk", .1f);
            isIdle = false;
        }


        Vector3 currentScale = player.transform.localScale;
        Quaternion currentRotation = player.transform.localRotation;
        if ((moveInput < 0 && currentScale.x > 0) || (moveInput > 0 && currentScale.x < 0))
        {
            currentScale.x *= -1;
            currentRotation.z *= -1;
            player.transform.localScale = currentScale;
            player.transform.localRotation = currentRotation;
        }

        Vector3 position = transform.position;
        position.x += moveInput * moveSpeed * Time.deltaTime;
        position.x = Mathf.Clamp(position.x, minX, maxX);
        transform.position = position;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Character>();
        animator = GetComponentInChildren<Animator>();
        //player.SetCharacterName("Nicole");
        //player.LoadCharacter();
        ChangeChildSortingLayers(transform);
        animator.Play("BaseCharacter_Idle");
        if (background != null)
        {
            // Calculate the movement bounds based on the background's collider
            Bounds backgroundBounds = background.GetComponent<Collider2D>().bounds;
            float objectHalfWidth = GetComponent<Collider2D>().bounds.extents.x;

            minX = backgroundBounds.min.x + objectHalfWidth;
            maxX = backgroundBounds.max.x - objectHalfWidth;
        }
    }

    private void ChangeChildSortingLayers(Transform parent)
    {
        // Iterate through all child objects
        foreach (Transform child in parent)
        {
            // Get the Sprite Renderer component of the child object
            SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();

            if (spriteRenderer != null)
            {
                // Change the sorting layer to the target sorting layer
                spriteRenderer.sortingLayerName = targetSortingLayer;
            }

            // Recursively call the function for nested child objects
            ChangeChildSortingLayers(child);
        }
    }

}

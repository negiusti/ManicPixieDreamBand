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

    //void Awake()
    //{
    //    DontDestroyOnLoad(gameObject);
    //}

    void FixedUpdate()
    {
        moveInput = Input.GetAxis("Horizontal");

        if (moveInput == 0f && !isIdle)
        {
            animator.CrossFade("Idle", .1f);
            isIdle = true;
        }
        else if (moveInput != 0f && isIdle)
        {
            animator.CrossFade("Walk", .1f);
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
        position.x += moveInput * moveSpeed * Time.fixedDeltaTime;
        transform.position = position;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInChildren<Character>();
        animator = GetComponentInChildren<Animator>();
        player.SetCharacterName("Nicole");
        player.LoadCharacter();
        ChangeChildSortingLayers(transform);
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

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Milk : MonoBehaviour
{
    private Animator animator;
    private Quaternion originalRotation;
    private Quaternion targetRotation;
    private bool isMouseOver = false;
    public float rotationAngle = 25f;
    public float rotationSpeed = 5f;
    private Vector3 origialPos;
    private Vector3 targetPos;
    private LerpPosition lerp;

    void Start()
    {
        origialPos = transform.localPosition;
        targetPos = new Vector3(21.2f, origialPos.y, origialPos.z);
        // Store the original rotation of the GameObject
        originalRotation = transform.rotation;
        // Calculate the target rotation
        targetRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + rotationAngle);
        animator = GetComponent<Animator>();
        lerp = GetComponent<LerpPosition>();
    }

    void Update()
    {
        if (isMouseOver)
        {
            // Rotate towards the target rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
        else
        {
            // Rotate back to the original rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, originalRotation, Time.deltaTime * rotationSpeed);
        }

    }

    private void OnMouseDown()
    {
        StartCoroutine(lerp.Lerp(targetPos, 0.5f));
        animator.Play("Pour");
    }

    void OnMouseEnter()
    {
        isMouseOver = true;
    }

    void OnMouseExit()
    {
        isMouseOver = false;
    }
}

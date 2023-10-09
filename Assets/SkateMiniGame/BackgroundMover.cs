using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMover : MonoBehaviour
{
    public float moveSpeed = 5f;

    private void Start()
    {
    }

    private void Update()
    {
        // Move the GameObject to the left
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
    }
}

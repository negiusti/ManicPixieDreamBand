using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 startRotation;
    private LerpPosition lerp;

    void Start()
    {
        // Store the initial position of the game object
        startPosition = transform.localPosition;
        startRotation = transform.localEulerAngles;
        lerp = GetComponent<LerpPosition>();
        transform.localEulerAngles = GetRandomAngles();
    }

    private void OnEnable()
    {
        if (lerp == null)
            Start();
        lerp.finishedLerp = true;
        lerp.finishedRotationLerp = true;
    }

    private Vector3 GetRandomTarget()
    {
        // Generate a random point within a 1 unit radius
        Vector3 randomPoint = Random.insideUnitSphere * 0.1f;

        randomPoint.z = 0;

        // Apply the random offset to the starting position
        return startPosition + randomPoint;
    }

    private Vector3 GetRandomAngles()
    {
        return startRotation + new Vector3(0f, 0f, Random.Range(0f, 180f));
    }

    private void Update()
    {
        if (lerp.finishedLerp)
        {
            lerp.finishedLerp = false;
            StartCoroutine(lerp.Lerp(GetRandomTarget(), Random.Range(1.5f, 2f)));
        }
        if (lerp.finishedRotationLerp)
        {
            lerp.finishedRotationLerp = false;
            StartCoroutine(lerp.LerpRotation(GetRandomAngles(), Random.Range(7f, 8f)));
        }
    }
}

using System.Collections;
using UnityEngine;

public class DrumstickScript : MonoBehaviour
{
    //public bool isLeftStick;
    //private Vector3 startPos;
    //private Vector3 startScale;
    //private float inSpeed = 20f;
    //private float outSpeed = 2.5f;
    //private bool onDrumPad = false;

    //private float shakeDuration = 0.3f;
    //private float shakeMagnitude = 0.4f;
    //private bool isShaking = false;
    // Start is called before the first frame update
    //void Start()
    //{
    //    startPos = transform.position;
    //    startScale = transform.localScale;
    //}

    // Update is called once per frame
    void Update()
    {
        //float newXPosition = transform.position.x;
        //if (isShaking)
        //    return;
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
            //float adjustment = inSpeed * Time.deltaTime;
        //    if (isLeftStick)
        //        newXPosition += adjustment;
        //    else
        //        newXPosition -= adjustment;
        //}
        //else if (onDrumPad)
        //{
        //    float adjustment = outSpeed * Time.deltaTime;
        //    if (isLeftStick)
        //        newXPosition -= adjustment;
        //    else
        //        newXPosition += adjustment;
        //}
        //transform.position = new Vector3(newXPosition, transform.position.y, transform.position.z);
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.name == "Drumpad")
    //    {
    //        onDrumPad = true;
    //    }
    //    if (collision.gameObject.name.Contains("drumstick"))
    //    {
    //        isShaking = true;
    //        transform.localScale = startScale;
    //        StartCoroutine(Shake());
    //    }
    //}
    //public bool Shaking()
    //{
    //    return isShaking;
    //}
   
    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.gameObject.name == "Drumpad")
    //    {
    //        onDrumPad = false;
    //    }
    //}

    //IEnumerator Shake()
    //{
    //    float elapsedTime = 0.0f;

    //    while (elapsedTime < shakeDuration)
    //    {
    //        Vector3 randomOffset = Random.insideUnitSphere * shakeMagnitude;
    //        transform.position = transform.position + randomOffset;

    //        elapsedTime += Time.deltaTime;

    //        yield return null;
    //    }
    //    transform.position = startPos;
    //    transform.localScale = startScale;
    //    isShaking = false;
    //}
}

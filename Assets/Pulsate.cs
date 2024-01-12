using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class Pulsate : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originalScale;
    //public float scaleFactor;
    private bool isPulsing;
    private float pulseSpeed = 0.2f; // Adjust the speed of the pulsation
    private float pulseMagnitude = 0.1f; // Adjust the magnitude of the pulsation (20% bigger/smaller)

    // Start is called before the first frame update
    void Start()
    {
        originalScale = this.gameObject.transform.localScale;
        //if (scaleFactor < 1f)
        //{
        //    scaleFactor = 1.1f;
        //}
        isPulsing = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPulsing)
        {
            // Calculate the scale factor using Mathf.PingPong
            float scaleFactor = 1.0f + Mathf.PingPong(Time.time * pulseSpeed, pulseMagnitude);

            // Apply the scale to the GameObject
            transform.localScale = new Vector3(originalScale.x * scaleFactor, originalScale.y * scaleFactor, originalScale.z * scaleFactor);
        }
    }

    void OnMouseExit()
    {
        //this.gameObject.transform.localScale = originalScale;
        isPulsing = true;
    }

    void OnMouseEnter()
    {
        //Vector3 newScale = originalScale * scaleFactor;
        //this.gameObject.transform.localScale = newScale;
        isPulsing = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isPulsing = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPulsing = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CarMechanicHand : MonoBehaviour
{
    public LerpPosition lerpPosition;
    // Start is called before the first frame update
    void Start()
    {
        lerpPosition = GetComponent<LerpPosition>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void move(float targetx, float targety, float moveTime)
    {
        StartCoroutine(lerpPosition.Lerp(new Vector3(targetx, targety, 50), moveTime * Time.deltaTime, false));
    }

    public bool handFull()
    {
        return transform.childCount > 0;
    }

    public string getTool()
    {
        if (!handFull()) return null;
        return transform.GetChild(0).name;
    }
}

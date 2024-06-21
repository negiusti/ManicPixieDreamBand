using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Hand : MonoBehaviour
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

    public void move(float targetX, float targetY, float moveTime)
    {
        StartCoroutine(lerpPosition.Lerp(new Vector3(targetX, targetY, 50), moveTime * Time.deltaTime, false));
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

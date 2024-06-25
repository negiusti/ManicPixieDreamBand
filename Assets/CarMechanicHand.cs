using UnityEngine;

public class CarMechanicHand : MonoBehaviour
{
    private LerpPosition lerpPosition;
    // Start is called before the first frame update
    void Start()
    {
        lerpPosition = GetComponent<LerpPosition>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move(float targetx, float targety, float moveTime)
    {
        StartCoroutine(lerpPosition.Lerp(new Vector3(targetx, targety, 50), moveTime * Time.deltaTime, false));
    }

    public bool IsFull()
    {
        return transform.childCount > 0;
    }

    public string GetTool()
    {
        if (!IsFull()) return null;
        return transform.GetChild(0).name;
    }
}

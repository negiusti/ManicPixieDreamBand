using System.Collections;
using UnityEngine;

public class StarMoverScript : MonoBehaviour
{
    private float beatTempo;// = 6f;
    public bool hasStarted = false;
    private float startTime;
    private float runwayDelay;
    private bool passed;
    private StarSpawnerScript spawner;
    private Vector3 destinationPos;

    // Start is called before the first frame update
    void Start()
    {
        hasStarted = false;
        passed = false;
        spawner = GetComponentInParent<StarSpawnerScript>();
        beatTempo = spawner.HighwaySpeed();
        destinationPos = new Vector3(transform.position.x, spawner.GetDestinationY(), transform.position.z);
    }

    //public IEnumerator Lerp(GameObject go, Vector3 targetLocalPosition, float duration)
    //{
    //    Vector3 startPosition = go.transform.localPosition;

    //    for (float timePassed = 0f; timePassed < duration; timePassed += Time.deltaTime)
    //    {
    //        float factor = timePassed / duration;
    //        factor = Mathf.SmoothStep(0, 1, factor);

    //        go.transform.localPosition = Vector3.Lerp(startPosition, targetLocalPosition, factor);

    //        yield return null;
    //    }
    //    go.transform.localPosition = targetLocalPosition;
    //}

    // Update is called once per frame
    void Update()
    {
        if (!hasStarted)
        {
            hasStarted = true;
            startTime = Time.time;
            //transform.position -= new Vector3(0f, beatTempo * Time.deltaTime, 0f);
        } else
        {
            //transform.position -= new Vector3(0f, beatTempo * Time.deltaTime, 0f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Activator"))
        {
            runwayDelay = Time.time - startTime;
            passed = true;
        }
    }

    public bool hasPassed()
    {
        return passed;
    }

    public float GetRunwayDelay()
    {
        return runwayDelay;
    }

}

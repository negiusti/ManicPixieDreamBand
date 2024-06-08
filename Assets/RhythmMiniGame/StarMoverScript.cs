using UnityEngine;

public class StarMoverScript : MonoBehaviour
{
    private float beatTempo;// = 6f;
    public bool hasStarted = false;
    private float startTime;
    private float runwayDelay;
    private bool passed;

    // Start is called before the first frame update
    void Start()
    {
        hasStarted = false;
        passed = false;
        beatTempo = this.gameObject.transform.parent.GetComponent<StarSpawnerScript>().HighwaySpeed();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasStarted)
        {
            hasStarted = true;
            startTime = Time.time;
            transform.position -= new Vector3(0f, beatTempo * Time.deltaTime, 0f);
        } else
        {
            transform.position -= new Vector3(0f, beatTempo * Time.deltaTime, 0f);
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

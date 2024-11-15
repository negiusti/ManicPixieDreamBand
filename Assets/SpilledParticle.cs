using UnityEngine;

public class SpilledParticle : MonoBehaviour
{
    private SweepMicroGame mg;
    private void Start()
    {
        mg = GetComponentInParent<SweepMicroGame>();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (mg == null)
            Start();
        if (other.CompareTag("Activator"))
        {
            mg.IncreaseCount();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (mg == null)
            Start();
        if (other.CompareTag("Activator"))
        {
            mg.DecreaseCount();
        }
    }
}

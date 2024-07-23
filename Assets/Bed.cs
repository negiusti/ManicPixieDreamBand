using UnityEngine;

public class Bed : MonoBehaviour
{
    private Animator animator;
    private SleepingScreenMiniGame sleepingScreen;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("DoneForTheDay", Calendar.DoneForTheDay());
        GetComponent<BiggerWhenHovered>().scaleFactor = Calendar.DoneForTheDay() ? 1.1f : 1f;
        sleepingScreen = GetComponentInChildren<SleepingScreenMiniGame>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        if (Calendar.DoneForTheDay())
        {            
            animator.SetBool("DoneForTheDay", false);
            GetComponent<BiggerWhenHovered>().scaleFactor = 1f;
            sleepingScreen.OpenMiniGame();
        }
    }
}

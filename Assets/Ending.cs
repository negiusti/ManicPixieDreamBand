using UnityEngine;

public class Ending : MonoBehaviour
{
    private Animator animator;
    private bool startedEndingAnim;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();    
        startedEndingAnim = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!startedEndingAnim && !SceneChanger.Instance.IsLoadingScreenOpen()) {
            startedEndingAnim = true;
            animator.Play("Ending", -1, 0f);
        }
    }

    public void Done()
    {
        SceneChanger.Instance.ChangeScene("Credits");
    }
}

using System.Linq;
using UnityEngine;

public class BlackScreen : MonoBehaviour
{
    private MiniGame mg;
    private Animator animator;
    private bool fading;
    private bool closing;

    private void Start()
    {
        mg = GetComponentInParent<MiniGame>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        fading = false;
        closing = false;
        if (mg == null)
            Start();
    }

    public void CloseMiniGames()
    {
        if (mg != null)
        {
            if (closing)
                return;
            closing = true;
            mg.CloseMiniGame();
        }
    }

    public void Fade()
    {
        if (fading)
            return;
        fading = true;
        animator.Play("BlackScreenFade", -1, 0f);
    }

    public void Unfade()
    {
        closing = false;
        fading = false;
        animator.Play("BlackScreenUnfade", -1, 0f);
    }
}

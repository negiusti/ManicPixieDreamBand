using PixelCrushers.DialogueSystem;
using UnityEngine;

public class NPCCharacterController : MonoBehaviour
{
    private Character character;
    private TalkToMeHint hint;
    private bool shouldHint;

    // Start is called before the first frame update
    void Start()
    {
        character = this.GetComponentInChildren<Character>();
        character.SetCharacterName(transform.name);
        character.LoadCharacter();
        hint = GetComponentInChildren<TalkToMeHint>();
        shouldHint = hint != null && GetComponentInChildren<DialogueSystemTrigger>() != null && GetComponentInChildren<DialogueSystemTrigger>().isActiveAndEnabled;
    }

    // Update is called once per frame
    void Update()
    {
        if (DialogueManager.Instance.IsConversationActive)
        {
            if (hint != null)
            {
                hint.gameObject.SetActive(false);
            }
        } else if (shouldHint)
        {
            hint.gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (shouldHint)
            {
                hint.ShowButtonPrompt();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (shouldHint)
            {
                hint.HideButtonPrompt();
            }
        }
    }
}

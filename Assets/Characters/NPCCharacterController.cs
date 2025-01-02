using PixelCrushers.DialogueSystem;
using UnityEngine;

public class NPCCharacterController : MonoBehaviour
{
    private Character character;
    private TalkToMeHint hint;

    // Start is called before the first frame update
    void Start()
    {
        character = gameObject.GetComponent<Character>();
        //character.SetCharacterName(transform.name);
        //character.LoadCharacter();
        hint = gameObject.GetComponentInChildren<TalkToMeHint>(true);
    }

    private bool shouldHint()
    {
        return hint != null && gameObject.GetComponent<DialogueSystemTrigger>() != null && gameObject.GetComponent<DialogueSystemTrigger>().enabled;
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
        } else if (shouldHint())
        {
            hint.gameObject.SetActive(true);
        } else
        {
            hint.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (shouldHint())
            {
                hint.ShowButtonPrompt();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (shouldHint())
            {
                hint.HideButtonPrompt();
            }
        }
    }
}

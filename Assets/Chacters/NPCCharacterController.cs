using UnityEngine;
using PixelCrushers.DialogueSystem;

public class NPCCharacterController : MonoBehaviour
{
    private Player character;
    //private DialogueSystemController dialogueController;

    // Start is called before the first frame update
    void Start()
    {
        character = this.GetComponentInChildren<Player>();
        character.SetPlayerName(transform.name);
        character.LoadPlayer();
        //dialogueController = FindObjectOfType<DialogueSystemController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (DialogueLua.GetActorField(character.PlayerName(), "isLeaving").asBool)
        {
            Debug.Log("HERE HERE HERE");
            character.Disappear();
        }
    }
}

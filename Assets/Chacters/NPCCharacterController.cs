using UnityEngine;
using PixelCrushers.DialogueSystem;

public class NPCCharacterController : MonoBehaviour
{
    private Player character;

    // Start is called before the first frame update
    void Start()
    {
        character = this.GetComponentInChildren<Player>();
        character.SetPlayerName(transform.name);
        character.LoadPlayer();
    }

    // Update is called once per frame
    void Update()
    {
    }
}

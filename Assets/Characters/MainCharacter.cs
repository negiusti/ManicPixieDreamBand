using PixelCrushers.DialogueSystem;
using UnityEngine;
using Rewired;


public class MainCharacter : MonoBehaviour
{
    private ProximitySelector ps;
    private Player player;


    // Start is called before the first frame update
    void Start()
    {
        ps = this.GetComponent<ProximitySelector>();
        player = ReInput.players.GetPlayer(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (DialogueManager.isConversationActive && ps.enabled)
        {
            ps.enabled = false;
        } else if (!DialogueManager.isConversationActive && !ps.enabled)
        {
            ps.enabled = true;
        }
        if (!DialogueManager.isConversationActive && ps.enabled && player.GetButtonDown("Interact"))
        {
            ps.UseCurrentSelection();
        }
    }
}

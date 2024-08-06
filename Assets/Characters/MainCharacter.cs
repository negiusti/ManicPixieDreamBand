using PixelCrushers.DialogueSystem;
using UnityEngine;


public class MainCharacter : MonoBehaviour
{
    private ProximitySelector ps;

    // Start is called before the first frame update
    void Start()
    {
        ps = this.GetComponent<ProximitySelector>();
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
    }
}

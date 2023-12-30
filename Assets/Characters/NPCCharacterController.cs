using UnityEngine;

public class NPCCharacterController : MonoBehaviour
{
    private Character character;

    // Start is called before the first frame update
    void Start()
    {
        character = this.GetComponentInChildren<Character>();
        character.SetCharacterName(transform.name);
        character.LoadCharacter();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void PlayInstrument()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private Player character;

    // Start is called before the first frame update
    void Start()
    {
        character = this.GetComponentInChildren<Player>();
        character.SetPlayerName(gameObject.name);
        character.LoadPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InputUIScript : MonoBehaviour
{
    public Character player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdatePlayerName()
    {
        InputField getName = this.GetComponentInChildren<InputField>();
        string textMessage = getName.text;
        Debug.Log("You just typed " + textMessage);
        player.SetCharacterName(getName.text);
    }
}

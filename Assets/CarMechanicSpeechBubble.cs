using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class CarMechanicSpeechBubble : MonoBehaviour
{
    public Text speechText;
    
    private string[] tools = new string[] { "Wrench", "Socket", "Screwdriver", "Pliers", "Banana"};
    private string[] angryResponses = new string[] { "Do I look like\nI can use this?", "Really?\nThis is what you\nthink I need?", "Are you\ntrying to slow\nme down?", "You gotta be\nkidding me\nwith this.", "Try again,\nand get it right\nthis time." };
    private string[] niceResponses = new string[] { "Perfect timing!", "This should\ndo the trick", "Just what I\nneeded, thanks!" };
    public string currTool;
    public bool angry = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AskForTool(string tool)
    {
        if(tool == null)
        {
            do
            {
                tool = tools[Random.Range(0, tools.Length)];
            } while (tool == currTool);
        }
        currTool = tool;
        speechText.text = "Could you \n hand me the \n" + currTool;
    }

    public void OutputMessage(bool angry)
    {
        this.angry = angry;
        if (angry)
        {
            speechText.text = angryResponses[Random.Range(0, angryResponses.Length)];
        }
        else
        {
            speechText.text = niceResponses[Random.Range(0, niceResponses.Length)];
        }
    }

    public void ToolInHandMessage()
    {
        angry = true;
        speechText.text = "Can't you\n see my hands\n are full!";
    }
    
    public void RemoveBanana()
    {
        string[] newTools = new string[tools.Length-1];
        for(int i = 0; i < newTools.Length; i++)
        {
            newTools[i] = tools[i];
        }
        tools = newTools;
    }
}

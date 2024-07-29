using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarPackingMiniGame : MiniGame
{
    public TrunkDoor trunkDoor;
    public Character maxCloseup;
    public Character rickiCloseup;
    public GameObject maxSpeechBubble;
    public Text maxSpeechText;
    public GameObject rickiSpeechBubble;
    public Text rickiSpeechText;
    private GameObject mainCamera;
    private bool isActive;
    private Coroutine cr;
    private List<MaxRickiDialogue> maxRickiDialogue = new List<MaxRickiDialogue> {
        new MaxRickiDialogue(MaxOrRicki.Max, "Can you hurry up please?"),
        new MaxRickiDialogue(MaxOrRicki.Ricki, "Be careful with my breakables!"),
        new MaxRickiDialogue(MaxOrRicki.Max, "Don't forget my pickles!", eyesEmote:"Angry"),
        new MaxRickiDialogue(MaxOrRicki.Ricki, "Yeah, Max needs her pickles!!"),
        new MaxRickiDialogue(MaxOrRicki.Max, "Lift with your knees, not your back."),
        new MaxRickiDialogue(MaxOrRicki.Ricki, "I would totally help but I just got a manicure.",eyesEmote:"Pathetic")
    };

    private enum MaxOrRicki
    {
        Max,
        Ricki
    }

    private class MaxRickiDialogue
    {
        public MaxOrRicki speaker;
        public string txt;
        public string mouthEmote;
        public string eyesEmote;

        public MaxRickiDialogue(MaxOrRicki speaker, string txt, string mouthEmote = "", string eyesEmote = "")
        {
            this.speaker = speaker;
            this.txt = txt;
            this.mouthEmote = mouthEmote;
            this.eyesEmote = eyesEmote;
        }
    }

    private void Start()
    {
        DisableAllChildren();
        //OpenMiniGame();
    }

    public override bool IsMiniGameActive()
    {
        return isActive;
    }

    public override void OpenMiniGame()
    {
        mainCamera = Camera.main.transform.gameObject;

        mainCamera.SetActive(false);

        EnableAllChildren();

        MiniGameManager.PrepMiniGame();
        isActive = true;
        cr = StartCoroutine(maxAndRickiConvo());
    }

    public override void CloseMiniGame()
    {
        if (cr != null)
            StopCoroutine(cr);
        DisableAllChildren();

        mainCamera.SetActive(true);

        isActive = false;
        MiniGameManager.CleanUpMiniGame();
    }

    public void Win()
    {
        trunkDoor.CloseTrunk();
    }

    private IEnumerator maxAndRickiConvo()
    {
        while (true)
        {
            foreach (MaxRickiDialogue d in maxRickiDialogue)
            {
                switch (d.speaker)
                {
                    case MaxOrRicki.Max:
                        maxBark(d.txt, d.eyesEmote, d.mouthEmote);
                        break;
                    case MaxOrRicki.Ricki:
                        rickiBark(d.txt, d.eyesEmote, d.mouthEmote);
                        break;
                    default:
                        break;
                }
                yield return new WaitForSeconds(5f);
            }
        }
    }

    private void rickiBark(string speechText, string emoteEyes = "", string emoteMouth = "")
    {
        maxSpeechBubble.SetActive(false);
        maxCloseup.EmoteEyes("Default");
        maxCloseup.EmoteMouth("Default");
        if (emoteEyes.Length > 0)
        {
            rickiCloseup.EmoteEyes(emoteEyes);
        }
        if (emoteMouth.Length > 0)
        {
            rickiCloseup.EmoteMouth(emoteMouth);
        }
        if (emoteEyes.Length > 0 || emoteMouth.Length > 0)
        {
            rickiCloseup.FacePop();
        }
        rickiSpeechText.text = speechText;
        rickiSpeechBubble.SetActive(true);
    }

    private void maxBark(string speechText, string emoteEyes="", string emoteMouth="")
    {
        rickiSpeechBubble.SetActive(false);
        rickiCloseup.EmoteEyes("Default");
        rickiCloseup.EmoteMouth("Default");
        if (emoteEyes.Length > 0)
        {
            maxCloseup.EmoteEyes(emoteEyes);
        }
        if (emoteMouth.Length > 0)
        {
            maxCloseup.EmoteMouth(emoteMouth);
        }
        if (emoteEyes.Length > 0 || emoteMouth.Length > 0)
        {
            maxCloseup.FacePop();
        }
        
        maxSpeechText.text = speechText;
        maxSpeechBubble.SetActive(true);
    }
    
}
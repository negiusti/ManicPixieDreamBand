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
    private BlackScreen blackScreen;
    private GameObject mainCamera;
    private bool isActive;
    private Coroutine cr;
    private List<MaxRickiDialogue> maxRickiDialogue = new List<MaxRickiDialogue> {
        new MaxRickiDialogue(MaxOrRicki.Ricki, "What happened to your pedal board dude?"),
        new MaxRickiDialogue(MaxOrRicki.Max, "I don't wanna talk about it, okay?"),
        new MaxRickiDialogue(MaxOrRicki.Ricki, "Hey! Be careful with my breakables!"),
        new MaxRickiDialogue(MaxOrRicki.Max, "And don't forget my pickles!", eyesEmote:"Angry"),
        new MaxRickiDialogue(MaxOrRicki.Ricki, "Yeah, Max needs her pickles!!"),
        new MaxRickiDialogue(MaxOrRicki.Max, "Lift with your knees, not your back.", eyesEmote:"Default"),
        new MaxRickiDialogue(MaxOrRicki.Ricki, "I would totally help but I just got a manicure.")
    };
    private List<MaxRickiDialogue> maxRickiDialogueCouch = new List<MaxRickiDialogue> {
        new MaxRickiDialogue(MaxOrRicki.Ricki, "Oh my God, that couch is rancid!"),
        new MaxRickiDialogue(MaxOrRicki.Max, "It's gonna look perfect in the basement!"),
        new MaxRickiDialogue(MaxOrRicki.Ricki, "I'm glad the couch fit in your car, but I don't think we'll be able to fit the rest of your junk."),
        new MaxRickiDialogue(MaxOrRicki.Max, "These are my precious belongings! We can't leave anything behind!"),
        new MaxRickiDialogue(MaxOrRicki.Ricki, "I think I broke a nail when we lifted that stupid couch.", eyesEmote:"Pathetic"),
        new MaxRickiDialogue(MaxOrRicki.Max, "Don't you lift a finger, Ricki! The new kid can do it!"),
        new MaxRickiDialogue(MaxOrRicki.Ricki, "Geez, why do you have so much junk in your car, Max??", eyesEmote:"Default"),
        new MaxRickiDialogue(MaxOrRicki.Max, "I live in my landlord's closet, so I have to store a few things in here okay?"),
        new MaxRickiDialogue(MaxOrRicki.Ricki, "I think those pickles should be refrigerated."),
        new MaxRickiDialogue(MaxOrRicki.Max, "Hey, make sure you life with your knees, not your back, new kid!"),
        new MaxRickiDialogue(MaxOrRicki.Ricki, "How much longer is this gonna take?"),
        new MaxRickiDialogue(MaxOrRicki.Max, "Yeah, I'm starving! Let's get this junk back in my trunk.")
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
        blackScreen = GetComponentInChildren<BlackScreen>(true);
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
        blackScreen.Unfade();
    }

    public void Fade()
    {
        if (blackScreen == null)
            CloseMiniGame();
        else
            blackScreen.Fade();
    }

    public override void CloseMiniGame()
    {
        if (!isActive)
            return;
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
        List<MaxRickiDialogue> maxRickiConvo = SceneChanger.Instance.GetCurrentScene() == "DowntownNeighborhood" ? maxRickiDialogueCouch : maxRickiDialogue;
        while (true)
        {
            foreach (MaxRickiDialogue d in maxRickiConvo)
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
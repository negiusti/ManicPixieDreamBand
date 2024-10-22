using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emojis : MonoBehaviour
{
    private string npcName;
    public Emoji emojiTemplate;
    private bool romanceable;
    private int romanceLvl;
    private HashSet<string> unlockedEmojis;
    private Emoji romanceEmoji;

    // Start is called before the first frame update
    void Start()
    {
        if (emojiTemplate == null)
            emojiTemplate = GetComponentInChildren<Emoji>(true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadEmojis(string name)
    {
        npcName = name;
        romanceable = (name == "Pixie" || name == "JJ");
        if (romanceable)
            romanceLvl = RomanceManager.GetRelationshipScore(npcName);
        else
            romanceLvl = -1;
        unlockedEmojis = ES3.Load("UnlockedEmojis/" + npcName, new HashSet<string>());
        foreach (string e in unlockedEmojis)
        {
            Emoji newEmoji = Instantiate(emojiTemplate, emojiTemplate.transform.parent);
            newEmoji.gameObject.SetActive(true);
            newEmoji.SetEmoji(e);
        }
        if (romanceable && romanceLvl > 0)
        {
            romanceEmoji = Instantiate(emojiTemplate, emojiTemplate.transform.parent);
            romanceEmoji.gameObject.SetActive(true);
            romanceEmoji.SetRomanceEmoji(romanceLvl);
        }
    }

    public void UnlockEmoji(string emojiName)
    {
        // TO DO: Check for duplicates
        Emoji newEmoji = Instantiate(emojiTemplate, emojiTemplate.transform.parent);
        newEmoji.gameObject.SetActive(true);
        newEmoji.SetEmoji(emojiName);
    }

    public void UpdateRomanceEmoji()
    {
        if (romanceable)
            romanceLvl = RomanceManager.GetRelationshipScore(npcName);
        if (romanceable && romanceLvl > 0)
        {
            if (romanceEmoji == null)
                romanceEmoji = Instantiate(emojiTemplate, emojiTemplate.transform.parent);
            romanceEmoji.SetRomanceEmoji(romanceLvl);
        }
    }

    public void SaveEmojis()
    {
        ES3.Load("UnlockedEmojis/" + npcName, unlockedEmojis);
    }
}
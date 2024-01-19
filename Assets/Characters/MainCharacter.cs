using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{
    // We store MC-specific info here like: 
    // money, unlocked clothes/furniture/instruments, inventory, etc
    private string characterName;
    private float bankBalance;
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

    public void SetCharacterName(string name)
    {
        characterName = name;
    }

    public void ModifyBankBalance(float delta)
    {
        bankBalance += delta;
    }

    public float GetBankBalance()
    {
        return (float)System.Math.Round(bankBalance, 2);
    }
}

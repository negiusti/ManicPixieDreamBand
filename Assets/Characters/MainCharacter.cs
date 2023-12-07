using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{
    // We store MC-specific info here like: 
    // money, unlocked clothes/furniture/instruments, inventory, etc
    private string characterName;
    private float bankBalance;

    // Start is called before the first frame update
    void Start()
    {
        LoadMainCharacter();
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public void SaveMainCharacter()
    {
        Debug.Log("Saving... " + characterName);
        
        SaveSystem.SaveMainCharacter(this);
    }

    public void LoadMainCharacter()
    {
        Debug.Log("Loading main character...");
        MainCharacterData mainCharacterData = SaveSystem.LoadMainCharacter();
        if (mainCharacterData == null)
        {
            // Character does not exist yet...
            bankBalance = 4.20f;
            characterName = "MainCharacter";
            return;
        }

    }
}

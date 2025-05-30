using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class BankApp : PhoneApp
{
    private Phone phone;
    public TextMeshPro balance;
    public TextMeshPro status;
    private SpriteResolver poopResolver;

    // Start is called before the first frame update
    void Start()
    {
        phone = this.GetComponentInParent<Phone>();
        poopResolver = this.GetComponentInChildren<SpriteResolver>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetBankBalance(double value)
    {
        MainCharacterState.SetBankBalance(value);
        UpdateBankBalance();
    }

    public void DebugGiveMeMoney()
    {
        SetBankBalance(100 + MainCharacterState.CurrentBankBalance());
    }

    public void UpdateBankBalance()
    {
        double currBalance = MainCharacterState.CurrentBankBalance();
        balance.text = "$" + currBalance.ToString("F2");
        if (currBalance < 30d) {
            poopResolver.SetCategoryAndLabel("Poop", "Sad");
            status.text = "ur poor lol, maybe try making coffee at home? :)";
        }
        else
        {
            poopResolver.SetCategoryAndLabel("Poop", "Happy");
            status.text = "keep up the good work, girl boss!!";
        }
            
    }

    public override void Save()
    {
        // Nothing to do here
    }

    public override void Load()
    {
        // Nothing to do here
    }
}

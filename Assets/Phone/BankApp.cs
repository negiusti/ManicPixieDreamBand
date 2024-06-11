using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class BankApp : MonoBehaviour
{
    private Phone phone;
    public TextMeshPro balance;
    public TextMeshPro status;
    private SpriteResolver poopResolver;
    public PhoneIcon phoneIcon;

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

    private void OnEnable()
    {
        if (phoneIcon != null)
            phoneIcon.HideNotificationIndicator();
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
}

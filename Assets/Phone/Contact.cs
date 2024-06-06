using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;
using TMPro;

public class Contact : MonoBehaviour
{
    private string contactName;
    private Image img;
    private SpriteRenderer spriteRenderer;
    private SpriteResolver spriteResolver;
    public PhoneMessages messages;
    public Phone phone;
    private TextMeshProUGUI contactNameTM;
    public GameObject notificationIndicator;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetContact(string name)
    {
        spriteResolver = GetComponentInChildren<SpriteResolver>();
        contactName = name;
        contactNameTM = GetComponentInChildren<TextMeshProUGUI>();
        img = GetComponentInChildren<Image>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        contactNameTM.text = contactName;
        spriteResolver.SetCategoryAndLabel("Pic", contactName);
        spriteResolver.ResolveSpriteToSpriteRenderer();
        img.sprite = spriteRenderer.sprite;
    }

    public void ShowNotificationIndicator()
    {
        notificationIndicator.SetActive(true);
    }

    public void HideNotificationIndicator()
    {
        notificationIndicator.SetActive(false);
    }

    public void StartConvo()
    {
        messages.OpenTxtConvoWith(contactName);
    }

    private void OnMouseDown()
    {
        StartConvo();
    }

    
}

using System;
using TMPro;
using UnityEngine;

public class TipJar : MonoBehaviour
{
    public float minY;
    public float maxY;
    private TextMeshPro txt;
    public GameObject moneyLevel;
    private LerpPosition moneyLerp;
    private float dollarIncrements;

    // Start is called before the first frame update
    void Start()
    {
        txt = GetComponentInChildren<TextMeshPro>(true);
        dollarIncrements = (maxY - minY) / 15f;
        moneyLerp = moneyLevel.GetComponent<LerpPosition>();
    }

    private void OnEnable()
    {
        moneyLevel.transform.localPosition = new Vector3(moneyLevel.transform.localPosition.x, minY, moneyLevel.transform.localPosition.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addTip(float amount)
    {
        txt.enabled = true;
        txt.text = "+$" + amount + " tip";
        StartCoroutine(moneyLerp.Lerp(new Vector3(moneyLevel.transform.localPosition.x, Math.Min(moneyLevel.transform.localPosition.y + (dollarIncrements * amount), maxY), moneyLevel.transform.localPosition.z), 0.5f));
        //moneyLevel.transform.localPosition = new Vector3(moneyLevel.transform.localPosition.x, Math.Min(moneyLevel.transform.localPosition.y + (dollarIncrements * amount), maxY), moneyLevel.transform.localPosition.z);
    }

    public void HideTipText()
    {
        txt.enabled = false;
    }
}

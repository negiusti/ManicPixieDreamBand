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
        dollarIncrements = (maxY - minY) / 40f;
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
        if (amount < 0) {
            txt.text = "-$" + -1f*amount + " tip";
        } else {
            txt.text = "+$" + amount + " tip";
        }
        
        if (amount < 1f)
        {
            txt.color = Color.red;
        } else if (amount < 4f)
        {
            txt.color = Color.yellow;
        } else
        {
            txt.color = Color.green;
        }
        StartCoroutine(moneyLerp.Lerp(new Vector3(moneyLevel.transform.localPosition.x, Math.Min(moneyLevel.transform.localPosition.y + (dollarIncrements * amount), maxY), moneyLevel.transform.localPosition.z), 0.5f));
        //moneyLevel.transform.localPosition = new Vector3(moneyLevel.transform.localPosition.x, Math.Min(moneyLevel.transform.localPosition.y + (dollarIncrements * amount), maxY), moneyLevel.transform.localPosition.z);
    }

    public void HideTipText()
    {
        txt.enabled = false;
    }
}

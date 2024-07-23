using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ColorPalette : MonoBehaviour
{
    private CharacterEditor characterEditor;
    public string category;
    private bool showSkinTones;
    private bool coroutineDone;
    // Start is called before the first frame update
    void Start()
    {
        characterEditor = GameObject.FindObjectOfType<CharacterEditor>();
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
       coroutineDone = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public void SetColor(Color c)
    {
        if (category.Equals("Skin"))
            characterEditor.SetSkinColor(c);
        else
            characterEditor.SetCurrentFaceCategoryColor(c);
    }

    private void OnMouseDown()
    {
        Debug.Log("coroutineDone: "+ coroutineDone);
        if (!coroutineDone)
            return;
        if (!category.Equals("Skin"))
            return;
        coroutineDone = false;
        showSkinTones = !showSkinTones;
        if (showSkinTones)
            StartCoroutine(ShowSkinTones());
        else
            StartCoroutine(HideSkinTones());
    }

    private IEnumerator ShowSkinTones()
    {
        for (int i = 0; i < transform.childCount; i++) {
            transform.GetChild(i).gameObject.SetActive(true);
            yield return new WaitForSeconds(0.05f);
        }
        coroutineDone = true;
    }

    private IEnumerator HideSkinTones()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
            yield return new WaitForSeconds(0.05f);
        }
        coroutineDone = true;
    }

    //public void OnPointerDown(PointerEventData eventData)
    //{
    //    Debug.Log("coroutineDone: " + coroutineDone);
    //    if (!coroutineDone)
    //        return;
    //    if (!category.Equals("Skin"))
    //        return;
    //    coroutineDone = false;
    //    showSkinTones = !showSkinTones;
    //    if (showSkinTones)
    //        ShowSkinTones();
    //    else
    //        HideSkinTones();
    //}
}

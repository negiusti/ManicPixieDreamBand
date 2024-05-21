using System.Collections.Generic;
using UnityEngine;

public class Caboodle : MonoBehaviour
{
    private Dictionary<string, CaboodleSection> caboodleSections;
    private string currentCaboodleSection;
    private CharacterEditor characterEditor;

    // Start is called before the first frame update
    void Start()
    {
        caboodleSections = new Dictionary<string, CaboodleSection>();
        characterEditor = GameObject.FindObjectOfType<CharacterEditor>();
        foreach (CaboodleSection section in GetComponentsInChildren<CaboodleSection>(includeInactive:true))
        {
            caboodleSections.Add(section.gameObject.name, section);
        }
        currentCaboodleSection = "Bangs";
        SelectCaboodleSection(currentCaboodleSection);
    }

    public void SelectCaboodleSection(string category)
    {
        currentCaboodleSection = category;
        UnselectCaboodleSections();
        if (!caboodleSections.ContainsKey(category))
            return;
        caboodleSections[category].Select();
        characterEditor.SetCurrentFaceCategory(category);
    }

    private void UnselectCaboodleSections()
    {
        foreach (CaboodleSection section in caboodleSections.Values)
        {
            section.Unselect();
        }
    }

    private void OnEnable()
    {
        if (currentCaboodleSection != null)
            SelectCaboodleSection(currentCaboodleSection);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

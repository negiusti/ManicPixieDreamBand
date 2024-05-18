using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caboodle : MonoBehaviour
{
    private Dictionary<string, CaboodleSection> caboodleSections;

    // Start is called before the first frame update
    void Start()
    {
        caboodleSections = new Dictionary<string, CaboodleSection>();
        foreach (CaboodleSection section in GetComponentsInChildren<CaboodleSection>(includeInactive:true))
        {
            caboodleSections.Add(section.gameObject.name, section);
        }
    }

    public void SelectCaboodleSection(string category)
    {
        UnselectCaboodleSections();
        if (!caboodleSections.ContainsKey(category))
            return;
        caboodleSections[category].Select();
    }

    private void UnselectCaboodleSections()
    {
        foreach (CaboodleSection section in caboodleSections.Values)
        {
            section.Unselect();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

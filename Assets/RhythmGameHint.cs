using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmGameHint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SetChildrenActive(!Tutorial.playedBass);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void SetChildrenActive(bool active)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(active);
        }
    }
}

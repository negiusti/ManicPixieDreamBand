using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class CrowdSpawner : MonoBehaviour
{
    public List<CrowdMember> audience;
    private SpriteLibraryAsset spriteLib;
    private HashSet<string> heads;
    //private string[] allHeads;

    // Start is called before the first frame update
    void Start()
    {
        spriteLib = this.GetComponent<SpriteLibrary>().spriteLibraryAsset;
        heads = new HashSet<string>(spriteLib.GetCategoryLabelNames("Head"));



        foreach(CrowdMember member in audience)
        {
            if (heads.Count == 0)
                break;
            string head = heads.First();
            member.SetHeadSprite(head);
            heads.Remove(head);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}

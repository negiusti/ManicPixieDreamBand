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
        audience.ForEach(member => member.gameObject.SetActive(false));

    }

    public void SpawnCrowd(Band band)
    {
        //int ticketSales = BandsManager.GetBandTixSales();
        int ticketSales = BandsManager.GetBandTixSales(band.AvgTixSales);

        foreach (CrowdMember member in audience)
        {
            member.gameObject.SetActive(ticketSales-- > 0);
            if (heads.Count > 0)
            {
                string head = heads.First();
                member.SetHeadSprite(head);
                heads.Remove(head);
            }
            if (member.IsForegroundCrowdMember() && band.Name != "LEMON BOY")
                member.gameObject.SetActive(false);
        }
    }

    public void DespawnCrowd()
    {
        foreach (CrowdMember member in audience)
        {
            if (member.IsForegroundCrowdMember())
            {
                member.gameObject.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}

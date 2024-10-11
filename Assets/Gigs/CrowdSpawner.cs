using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class CrowdSpawner : MonoBehaviour
{
    public List<CrowdMember> audience;
    private SpriteLibraryAsset spriteLib;
    private HashSet<string> heads;
    public AudioClip smallClapClip;
    public AudioClip medClapClip;
    public AudioClip bigClapClip;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        spriteLib = GetComponent<SpriteLibrary>().spriteLibraryAsset;
        heads = new HashSet<string>(spriteLib.GetCategoryLabelNames("Head"));
        audience.ForEach(member => member.gameObject.SetActive(false));
        audioSource = GetComponent<AudioSource>();
    }

    public void Clap(int ticketSales)
    {
        if (ticketSales <= 0 || audioSource == null)
        {
            return;
        }
        if (ticketSales < 5)
        {
            audioSource.clip = smallClapClip;
            
        } else if (ticketSales < 10)
        {
            audioSource.clip = medClapClip;
        } else
        {
            audioSource.clip = bigClapClip;
        }
        audioSource.Play();
    }

    public void SpawnCrowd(Band band)
    {
        int ticketSales = BandsManager.GetBandTixSales(band.AvgTixSales);
        
        foreach (CrowdMember member in audience)
        {
            if (heads.Count > 0)
            {
                string head = heads.First();
                member.SetHeadSprite(head);
                heads.Remove(head);
            }
            member.gameObject.SetActive(ticketSales-- > 0);
            if (member.IsForegroundCrowdMember() && band.Name != "LEMON BOY")
                member.gameObject.SetActive(false);
        }
    }

    public void SpawnCrowd(int ticketSales)
    {
        foreach (CrowdMember member in audience)
        {
            if (heads.Count > 0)
            {
                string head = heads.First();
                member.SetHeadSprite(head);
                heads.Remove(head);
            }
            member.gameObject.SetActive(ticketSales-- > 0);
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

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class CrowdMember : MonoBehaviour
{
    private Dictionary<string, SpriteResolver> spriteResolvers;
    private SpriteLibraryAsset spriteLib;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        spriteResolvers = new Dictionary<string, SpriteResolver>();
        foreach (SpriteResolver sr in this.GetComponentsInChildren<SpriteResolver>()) {
            spriteResolvers.Add(sr.gameObject.name, sr);
        }
        spriteLib = this.GetComponent<SpriteLibrary>().spriteLibraryAsset;
        RandomizeAppearance();
        animator = this.GetComponent<Animator>();
        animator.CrossFade("Chill_Crowd_Anim", Random.Range(0f, 0.5f), 0, Random.Range(0f, 0.5f));
        //animator.Play("Chill_Crown_Anim", -1, Random.Range(0f, 0.5f));  
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsForegroundCrowdMember()
    {
        return gameObject.name.StartsWith("FG");
    }

    void RandomizeAppearance()
    {
        foreach (string category in spriteResolvers.Keys)
        {
            if (category.StartsWith("R_"))
            {
                continue;
            }
            string[] labels = spriteLib.GetCategoryLabelNames(category).ToArray();
            string label = labels[Random.Range(0, labels.Length)];
            spriteResolvers[category].SetCategoryAndLabel(category, label);
            if (category.StartsWith("L_"))
            {
                string rightCategory = "R_" + category.Substring(2);
                spriteResolvers[rightCategory].SetCategoryAndLabel(rightCategory, label);
            }
        }
    }

    public void SetHeadSprite(string label)
    {
        spriteResolvers["Head"].SetCategoryAndLabel("Head", label);
    }
}

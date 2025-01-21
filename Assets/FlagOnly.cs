using System.Linq;
using UnityEngine;

public class FlagOnly : MonoBehaviour
{
    public string[] TrueFlags;

    // Start is called before the first frame update
    void Start()
    {
        if (TrueFlags != null)
        {
            gameObject.SetActive(TrueFlags.All(f => MainCharacterState.CheckFlag(f)));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

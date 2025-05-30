using UnityEngine;
using PixelCrushers.DialogueSystem;
using Rewired;

public class ExitLocation : MonoBehaviour
{
    public Vector3 exitSpawnPos;
    private bool inRange;
    private string locationName;
    private string neighborhoodName;
    public bool isBusiness;
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        locationName = SceneChanger.Instance.GetCurrentScene();
        neighborhoodName = MapsApp.locationToNeighborhood[MapsApp.sceneNameToLocation[locationName]].ToString();
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        player = ReInput.players.GetPlayer(0);
    }

    // Boba shop: -33.51, -2.4, 0
    // Tattoo shop: 43.4, -2.4, 0
    // Thrift shop: -44.6, -2.64, 0
    // Music shop: 45.1, -2.64, 0
    // Bar: 11.3, -2.64, 0
    // Bedroom: -43.3, -2.4, 0
    // Basement: 46, -2.4, 0
    // Coffee Shop: 7.9, -2.4, 0


    // Update is called once per frame
    void Update()
    {
        //if (inRange && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) && InteractionEnabled())
        if (inRange && player.GetButtonDown("Interact") && InteractionEnabled())
        {
            if (!Tutorial.changedSkin && locationName.Equals("Bedroom"))
            {
                DialogueManager.StartConversation("Tutorial/IShouldGetChanged");
            }
            else
            {
                if (isBusiness)
                    GameManager.miscSoundEffects.Play("businessdoor");
                else
                    GameManager.miscSoundEffects.Play("door");

                if (exitSpawnPos == null || exitSpawnPos == Vector3.zero)
                    SceneChanger.Instance.ChangeScene(neighborhoodName);
                else
                    SceneChanger.Instance.ChangeScene(new SceneChanger.SceneInfo(neighborhoodName, exitSpawnPos));
            }
        }   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inRange = true;
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inRange = false;
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }


    private bool InteractionEnabled()
    {
        return !SceneChanger.Instance.IsLoadingScreenOpen() &&
            Phone.Instance.IsLocked() &&
            !GameManager.Instance.GetComponent<MenuToggleScript>().IsMenuOpen() &&
            !DialogueManager.IsConversationActive &&
            !MiniGameManager.AnyActiveMiniGames();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayGuitar : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private bool withinRange;
    public KeyCode keyToTrigger;
    private Character character;
    private PlayerMovement playerMovement;
    public GameObject minigame;
    private bool isPlayingGuitar;
    // Start is called before the first frame update
    void Start()
    {
        withinRange = false;
        spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlayingGuitar && withinRange && Input.GetKey(keyToTrigger))
        {
            spriteRenderer.enabled = false;
            isPlayingGuitar = true;
            character.SetHoldingSprite("Bass");
            playerMovement.PlayGuitar();
        }
        if (isPlayingGuitar && !minigame.activeSelf)
        {
            spriteRenderer.enabled = true;
            isPlayingGuitar = false;
            playerMovement.StopPlayingGuitar();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            character = other.gameObject.GetComponent<Character>();
            playerMovement = other.gameObject.GetComponent<PlayerMovement>();
            withinRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            withinRange = false;
        }
    }
}

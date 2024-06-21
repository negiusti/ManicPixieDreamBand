using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public class ClickAndDrag : MonoBehaviour
{
    Vector3 mousePositionOffset;
    private Vector3 startposition;
    private Vector3 startSize;
    private bool inHand = false;
    public float underCarX = -2f, underCarY = 2f;
    public float outsideCarX = -5f, outsideCarY = -1.5f;
    public float moveTime = 30;
    public float workTime = 2;
    private bool underCar = false;
    private float timer = 0;
    private bool inToolBox = true;
    private string currTool;
    public LerpPosition lerpPosition;
    public Hand hand;
    public CarMechanicSpeechBubble sb;
    public Slider slider;
    public float addedAmount = 0.00015f;
    public GameObject gameOverScreen;
    public CarMechanicMiniGameManager game;
    public Sprite bananaPeel;
    // Start is called before the first frame update
    void Start()
    {
        startposition = transform.position;
        startSize = transform.localScale;
        lerpPosition = GetComponent<LerpPosition>();
        sb.AskForTool(null);

    }

    // Update is called once per frame
    void Update()
    {
        if (underCar)
        {
            if(timer < workTime)
            {
                timer += Time.deltaTime;
                slider.value += addedAmount;
            }
            else
            {
                askForNewTool();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Hand>() != null)
        {
            inHand = true;
        }
        if (collision.gameObject.GetComponent<Toolbox>() != null)
        {
            inToolBox = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<Hand>() != null)
        {
            inHand = false;
        }
        if (collision.gameObject.GetComponent<Toolbox>() != null)
        {
            inToolBox = false;
        }

    }

    private void OnMouseUp()
    {
        gameObject.transform.localScale = startSize;
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
        if (inHand && !game.gameOver)
        {
            if (!hand.handFull())
            {
                // puts the tool in the hand
                transform.position = new Vector3(hand.transform.position.x - 0.3f, hand.transform.position.y - 0.3f, hand.transform.position.z - 5);
                transform.parent = hand.transform;

                // if correct, take tool under car and output positive message
                if (hand.getTool() == sb.currTool)
                {
                    gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
                    hand.move(underCarX, underCarY, moveTime);
                    if(hand.getTool() == "Banana")
                    {
                        sb.speechText.text = "Yummmmm!";
                    }
                    else
                    {
                        sb.OutputMessage(false);
                    }
                    underCar = true;
                }
                // if incorrect tool output negative message
                else
                {
                    sb.OutputMessage(true);
                }
            }
            else
            {
                sb.ToolInHandMessage();
                transform.position = startposition;
            }
            
            


        }
        else
        {
            transform.position = startposition;
        }


    }

    private void OnMouseDown()
    {
        mousePositionOffset = gameObject.transform.position - GetMouseWorldPosition();
        gameObject.transform.localScale = new Vector3(transform.localScale.x * 1.5f, transform.localScale.y * 1.5f, 0);
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 3;
        transform.parent = null;
        if (sb.angry && !hand.handFull())
        {
            sb.AskForTool(sb.currTool);
        }

    }

    private void OnMouseDrag()
    {

        Vector3 position = GetMouseWorldPosition() + mousePositionOffset;

        // Update the GameObject's position to the snappedPosition.
        transform.position = position;
    }
    // Places the dragged object wherever the mouse is released, shifts the object if needed, and resets the position if the object is overlapping too much with another object.

    private Vector3 GetMouseWorldPosition()
    {
        // Get the mousePosition in world coordinates.
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void askForNewTool()
    {
        hand.move(outsideCarX, outsideCarY, moveTime);
        if (hand.getTool() == "Banana")
        {
            throwBanana();
        }

        timer = 0;
        underCar = false;
        if (slider.value == 1)
        {
            gameOverScreen.SetActive(true);
            game.gameOver = true;
        }
        else if (slider.value >= 0.6f && game.bananaEaten == false)
        {
            sb.AskForTool("Banana");
        }
        else
        {
            sb.AskForTool(null);
        }
    }

    private void throwBanana()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = bananaPeel;
        transform.localScale = new Vector3(1.2f, 1.2f, 0);
        transform.Rotate(0, 0, -90);
        transform.parent = null;
        StartCoroutine(lerpPosition.Lerp(new Vector3(-12, -7, 50), moveTime * Time.deltaTime * 10, true));
        game.bananaEaten = true;
        sb.RemoveBanana();
    }
}

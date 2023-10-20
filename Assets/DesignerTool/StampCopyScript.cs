using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StampCopyScript : MonoBehaviour
{
    private bool canDuplicate;
    private SpriteRenderer sr;
    private SpriteMaskInteraction smi;
    private GameObject designCanvas;
    private Collider2D coll;
    // Start is called before the first frame update
    void Start()
    {
        canDuplicate = true;
        sr = GetComponent<SpriteRenderer>();
        sr.maskInteraction = SpriteMaskInteraction.None;
        designCanvas = GameObject.FindWithTag("DesignCanvas");
        coll = GetComponent<Collider2D>();
        //GetComponent<ClickAndDrag>().enabled = false;
        //gameObject.AddComponent<ClickAndDrag>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        if (canDuplicate)
        {
            DuplicateSelf();
        }
    }

    private void OnMouseUp()
    {
        sr.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        if (!sr.isVisible || !coll.IsTouching(designCanvas.GetComponent<Collider2D>()))
        {
            Destroy(gameObject);
        }
    }

    //public void SetDuplicate(bool canDup)
    //{
    //    this.canDuplicate = canDup;
    //}

    private void DuplicateSelf()
    {
        canDuplicate = false;
        //GetComponent<ClickAndDrag>().enabled = true;
        //Destroy(GetComponent<StampCopyScript>());
        //tag = "Design";
        GameObject clone = Instantiate(gameObject, transform.position, transform.rotation, transform.parent);
        //GameObject clone = Instantiate(gameObject);
        //clone.transform.parent = transform.parent;
        //clone.tag = "Stamp";
        //clone.AddComponent<StampCopyScript>();
        //clone.GetComponent<StampCopyScript>().enabled = false;
        //Destroy(clone.GetComponent<ClickAndDrag>());
    }
}

/*
using UnityEngine;

public class FindTouchingObjects2D : MonoBehaviour
{
    public LayerMask objectsToCheck; // Assign the layers of objects you want to check in the Inspector
    private Collider2D[] touchingObjects;

    void Update()
    {
        // Find the "designcanvas" game object
        GameObject designCanvas = GameObject.FindWithTag("designcanvas");
        if (designCanvas == null)
        {
            Debug.LogWarning("No 'designcanvas' object found.");
            return;
        }

        // Get the collider of the "designcanvas" object
        Collider2D designCanvasCollider = designCanvas.GetComponent<Collider2D>();

        // Check for overlaps with the specified layers
        touchingObjects = Physics2D.OverlapBoxAll(designCanvasCollider.bounds.center, designCanvasCollider.bounds.extents, 0, objectsToCheck);

        // Process the overlapping objects
        foreach (Collider2D objCollider in touchingObjects)
        {
            GameObject obj = objCollider.gameObject;
            // Do something with the touching objects
            Debug.Log("Object with tag '" + obj.tag + "' is touching the 'designcanvas'.");
        }
    }
}
*/
using UnityEngine;
using System.Collections.Generic;

public class TattooMiniGame : MonoBehaviour
{
    public int outOfGuidelines;

    public GameObject linePrefab;
    private Line line;

    public GameObject guideline; // The current guideline
    private PolygonCollider2D guidelineCollider;

    // A list of all of the points connected by the guideline's collider
    public List<Vector2> guidelineColliderPoints = new List<Vector2>();

    public GameObject guidelineCheck;

    private void Start()
    {
        guidelineCollider = guideline.GetComponent<PolygonCollider2D>();

        // Looping through the paths of the guideline's collider, which contain the points
        for (int i = 0; i < guidelineCollider.pathCount; i++)
        {
            // Looping through the points in the current path and adding them to the list
            for (int j = 0; j < guidelineCollider.GetPath(i).Length; j++)
            {
                guidelineColliderPoints.Add(guidelineCollider.GetPath(i)[j]);
            }
        }

        for (int i = 0; i < guidelineColliderPoints.Count; i++)
        {
            // Multiplying the position being checked by the guideline's local scale to transpose it from local to world position
            Vector2 checkPosition = new Vector2(guidelineColliderPoints[i].x * guideline.transform.localScale.x, guidelineColliderPoints[i].y * guideline.transform.localScale.y);

            // Spawn a check object at each of the positions stored in the list
            Instantiate(guidelineCheck, checkPosition, Quaternion.identity, guideline.transform);
        }
    }

    private void Update()
    {
        // Spawn a new line and set it as the current line
        if (Input.GetMouseButtonDown(0))
        {
            GameObject newLine = Instantiate(linePrefab);
            line = newLine.GetComponent<Line>();
        }

        if (Input.GetMouseButtonUp(0))
        {
            line = null;

            // If all of the guideline's checks have been deleted, delete the guideline, lerp the arm offscreen, delete that arm, and spawn a new arm
            if (guideline.transform.childCount == 0)
            {
                Destroy(guideline);
            }
        }

        // If you are drawing a line now, pass in the current mouse position into the line script
        if (line != null)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            line.UpdateLine(mousePosition);
        }
    }  
}
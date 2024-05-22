using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class Line : MonoBehaviour
{
    // LineRenderer is a default Unity component
    public LineRenderer lineRenderer;

    // A list of all of the positions connected by this line
    private List<Vector2> positions;

    // The smaller this value is, faster points are updated on the lines
    public float minimumDistance = 0.025f;

    public GameObject guidelineCheck;

    public void UpdateLine(Vector2 position)
    {
        // If the positions list has not been initialized, initialize it and set the passed in position
        if (positions == null)
        {
            positions = new List<Vector2>();
            SetPoint(position);

            return;
        }
        // The minimumDistance is used to limit the minimum distance between points
        if (Vector2.Distance(positions.Last(), position) > minimumDistance)
        {
            SetPoint(position);
        }
    }

    private void SetPoint(Vector2 position)
    {
        // Add the passed in position to the list
        positions.Add(position);

        // Updating the LineRenderer's positions
        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPosition(positions.Count - 1, position);

        // Spawn a lineCheck at the passed in position
        Instantiate(guidelineCheck, position, Quaternion.identity, transform);
    }
}

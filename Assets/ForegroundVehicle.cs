using UnityEngine;

public class ForegroundVehicle : MonoBehaviour
{
    private GameObject background;
    private float minX, maxX;
    public bool left;
    public float moveSpeed = 30f;
    public Color[] colors;
    public SpriteRenderer car;

    // Start is called before the first frame update
    void Start()
    {
        background = GameObject.FindGameObjectWithTag("Background");

        if (background != null)
        {
            // Calculate the movement bounds based on the background's collider
            Bounds backgroundBounds = background.GetComponent<Collider2D>().bounds;
            //float objectHalfWidth = GetComponent<Collider2D>().bounds.extents.x;

            minX = backgroundBounds.min.x - 100f;
            maxX = backgroundBounds.max.x + 100f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void SwitchDirection()
    {
        left = !left;
        Quaternion currentRotation = transform.rotation;
        currentRotation.y = left ? 180 : 0;
        currentRotation.z = 0f;
        transform.rotation = currentRotation;
        if (colors != null && colors.Length > 0)
            car.color = colors[Random.Range(0, colors.Length)];
    }

    private void Move()
    {
        Vector3 position = transform.position;
        //float moveSpeed = Random.Range(27f, 28f);
        float moveInput = left ? -1f : 1f;
        position.x += moveInput * moveSpeed * Time.deltaTime;
        transform.position = position;
        if (position.x > maxX && !left)
            SwitchDirection();
        if (position.x < minX && left)
            SwitchDirection();
    }
}

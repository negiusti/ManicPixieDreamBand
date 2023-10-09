using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSpawnerScript : MonoBehaviour
{
    public GameObject background;
    private float respawnXPosition = 18.5f;
    private Queue<GameObject> bgs = new Queue<GameObject>();
    private int size;

    // Start is called before the first frame update
    void Start()
    {
        bgs.Enqueue(Instantiate(background, new Vector3(respawnXPosition, transform.position.y, transform.position.z), Quaternion.identity));
        size = 1;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the GameObject is outside of the camera view
        GameObject bg = bgs.Peek();
        if (bg.transform.position.x < -2.6f && size < 3)
        {
            Respawn();
        }
        if (bg.transform.position.x < -19f) {
            bgs.Dequeue();
            Destroy(bg);
            size--;
        }
    }

    private void Respawn()
    {
        bgs.Enqueue(Instantiate(background, new Vector3(respawnXPosition, transform.position.y, transform.position.z), Quaternion.identity));
        size++;
    }
}

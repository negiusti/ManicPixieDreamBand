using System.Threading;
using UnityEngine;

public class Shine : MonoBehaviour
{
    public Material mat;
    public float speed = 3f;
    private float value;
    private float t = 0f;


    void Update()
    {
        if (t < 1f)
        {
            t += speed * Time.deltaTime;
            value = Mathf.Lerp(0f, 1f, t);
            mat.SetFloat("_ShineLocation", value);
        } else {
            t = 0f;
        }
    }
}

using UnityEngine;
using UnityEngine.EventSystems;

public class GoToPreviousSceneButton : MonoBehaviour, IPointerDownHandler
{
    private GameManager gm;
    private SceneChanger sc;
    private bool done;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;
        sc = gm.gameObject.GetComponent<SceneChanger>();
        done = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        if (!done)
        {
            sc.GoToPreviousScene();
            done = true;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnMouseDown();
    }
}

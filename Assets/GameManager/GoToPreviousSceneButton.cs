using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GoToPreviousSceneButton : MonoBehaviour, IPointerDownHandler
{
    private GameManager gm;
    private SceneChanger sc;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;
        sc = gm.gameObject.GetComponent<SceneChanger>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        sc.GoToPreviousScene();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnMouseDown();
    }
}

using UnityEngine;

public class DemoGoToPortrait : MonoBehaviour
{
    private GameManager gm;
    private SceneChanger sc;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;
        sc = gm.GetComponent<SceneChanger>();
    }

    public void GoToDemoPortrait()
    {
        sc.ChangeScene("DemoPortrait");
    }

    private void OnMouseDown()
    {
        GoToDemoPortrait();
    }

    void Update()
    {
    }
}

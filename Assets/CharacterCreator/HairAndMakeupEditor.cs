using UnityEngine;
using UnityEngine.U2D.Animation;
using Rewired;
using UnityEngine.EventSystems;

public class HairAndMakeupEditor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private SpriteResolver targetResolver;
    private SpriteRenderer spriteRenderer;
    private bool makeupView = false;
    public DrawerScript[] otherDrawers;
    public GameObject makeupMenu;
    public GameObject skinPalette;
    public GameObject faceColorPalettes;
    private Camera mainCamera;
    private float panDuration = 0.5f;
    bool panToCloseup = false;
    bool panToDefault = false;
    private CapsuleCollider2D capCollider;
    public GameObject wardrobe;

    private Vector3 defaultPosition;
    private Vector3 closeupPosition;
    private float closeupSize = 2.9f;
    private float startTime;
    private float defaultSize;
    private Player player;
    // size 2.7
    // position x 2.44 y 2.56


    // Start is called before the first frame update
    void Start()
    {
        //SpriteLibrary fuck = this.GetComponentInParent<SpriteLibrary>();
        //LibraryAsset = fuck.spriteLibraryAsset;
        targetResolver = GetComponent<SpriteResolver>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        capCollider = GetComponent<CapsuleCollider2D>();
        makeupMenu.SetActive(false);
        faceColorPalettes.SetActive(false);

        mainCamera = Camera.main;
        // Save the initial and target positions of the camera
        defaultSize = mainCamera.orthographicSize;
        defaultPosition = mainCamera.transform.position;
        closeupPosition = new Vector3(4f, 2.56f, mainCamera.transform.position.z);
        player = ReInput.players.GetPlayer(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (panToCloseup)
        {
            PanCameraToCloseup();
        }
        else if (panToDefault)
        {
            PanCameraToDefault();
        }
        if (player.GetButtonDown("Back"))
        {
            ExitMakeupView();
        }
    }

    void OnMouseDown()
    {
       EnterMakeupView();
    }

    void OnMouseEnter()
    {
        targetResolver.SetCategoryAndLabel("Makeup", "Open");
    }

    void OnMouseExit()
    {
        targetResolver.SetCategoryAndLabel("Makeup", "Closed");
    }

    void EnterMakeupView()
    {
        if (makeupView)
            return;
        spriteRenderer.enabled = false;
        capCollider.enabled = false;
        wardrobe.SetActive(false);
        makeupView = true;
        makeupMenu.SetActive(true);
        faceColorPalettes.SetActive(true);
        startTime = Time.time;
        panToCloseup = true;
        skinPalette.SetActive(false);
    }

    public void ExitMakeupView()
    {
        if (!makeupView)
            return;
        spriteRenderer.enabled = true;
        capCollider.enabled = true;
        wardrobe.SetActive(true);
        makeupView = false;
        makeupMenu.SetActive(false);
        faceColorPalettes.SetActive(false);
        startTime = Time.time;
        panToDefault = true;
        skinPalette.SetActive(true);
    }


    void PanCameraToCloseup()
    {
        // Calculate the lerp parameter based on elapsed time
        float t = (Time.time - startTime) / panDuration;

        // Interpolate between the start and target positions
        mainCamera.transform.position = Vector3.Lerp(defaultPosition, closeupPosition, t);
        // Interpolate between the start and target sizes for zoom
        mainCamera.orthographicSize = Mathf.Lerp(defaultSize, closeupSize, t);

        // If the lerp parameter reaches 1, the pan is complete
        if (t >= 1.0f)
        {
            // Optionally, you may want to perform some action when the pan is complete
            Debug.Log("Pan complete!");
            panToCloseup = false;
        }
    }

    void PanCameraToDefault()
    {
        // Calculate the lerp parameter based on elapsed time
        float t = (Time.time - startTime) / panDuration;

        // Interpolate between the start and target positions
        mainCamera.transform.position = Vector3.Lerp(closeupPosition, defaultPosition, t);
        // Interpolate between the start and target sizes for zoom
        mainCamera.orthographicSize = Mathf.Lerp(closeupSize, defaultSize, t);

        // If the lerp parameter reaches 1, the pan is complete
        if (t >= 1.0f)
        {
            // Optionally, you may want to perform some action when the pan is complete
            Debug.Log("Pan complete!");
            panToDefault = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnMouseEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnMouseExit();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnMouseDown();
    }
}

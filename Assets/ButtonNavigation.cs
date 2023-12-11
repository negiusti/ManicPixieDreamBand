using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonNavigation : MonoBehaviour
{
    private Button[] selectables;
    private int currentIndex = 0;
    public GameObject UpArrow;
    public GameObject DownArrow;
    //private int topIdx;
    //private int bottomIdx;

    void Start()
    {
        // Get all Selectable components in the GameObject's children
        selectables = GetComponentsInChildren<Button>().Where(b => b.enabled).ToArray();

        // Ensure there is at least one selectable
        if (selectables.Length > 0)
        {
            // Set the initial selection
            Select(currentIndex);
        }
    }

    private void OnEnable()
    {
        selectables = GetComponentsInChildren<Button>().Where(b => b.enabled).ToArray();
        currentIndex = 0;
        Select(currentIndex);
    }

    void Update()
    {
        // Check for up and down arrow key presses
        //float verticalInput = Input.GetAxis("Vertical");

        //if (verticalInput > 0)
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // Move selection up
            SelectNext(-1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // Move selection down
            SelectNext(1);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            selectables[currentIndex].OnSubmit(null);
        }
    }

    void SelectNext(int direction)
    {
        // Deselect the current button
        Unselect(currentIndex);

        // Update the current index
        //currentIndex = (currentIndex + direction + selectables.Length) % selectables.Length;
        int proposedIdx = currentIndex + direction;
        currentIndex = proposedIdx < selectables.Length && proposedIdx >= 0 ? proposedIdx : currentIndex;
        // Select the new button
        Select(currentIndex);
    }

    private void Select(int idx)
    {
        HideAll();
        Show2Options(idx);
        EventSystem.current.SetSelectedGameObject(selectables[idx].gameObject);
        selectables[idx].OnSelect(null);
        selectables[idx].Select();
    }

    private void Unselect(int idx)
    {
        selectables[idx].OnDeselect(null);
    }

    private void HideAll()
    {
        foreach (Button b in selectables)
        {
            b.gameObject.SetActive(false);
        }
    }

    private void ShowArrows(int topIdx, int bottomIdx)
    {
        UpArrow.SetActive(false);
        DownArrow.SetActive(false);

        // if not at the bottom, show down arrow
        if (bottomIdx < selectables.Length - 1)
        {
            DownArrow.SetActive(true);
        }
        //if not at the top, show up arrow
        if (topIdx > 0)
        {
            UpArrow.SetActive(true);
        }
    }

    private void Show2Options(int idx)
    {
        // not at the bottom
        if (idx < selectables.Length - 1)
        {
            selectables[idx].gameObject.SetActive(true);
            selectables[idx + 1].gameObject.SetActive(true);
            ShowArrows(idx, idx + 1);
        }
        else
        {
            selectables[idx].gameObject.SetActive(true);
            selectables[idx - 1].gameObject.SetActive(true);
            ShowArrows(idx - 1, idx);
        }
    }
}

using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonNavigation : MonoBehaviour
{
    private Button[] selectables;
    private int currentIndex = 0;
    private int topIdx;
    private int bottomIdx;

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
        currentIndex = (currentIndex + direction + selectables.Length) % selectables.Length;

        // Select the new button
        Select(currentIndex);
    }

    private void Select(int idx)
    {
        EventSystem.current.SetSelectedGameObject(selectables[idx].gameObject);
        selectables[idx].OnSelect(null);
        selectables[idx].Select();
    }

    private void Unselect(int idx)
    {
        selectables[idx].OnDeselect(null);
    }
}

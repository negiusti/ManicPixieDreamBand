using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Rewired;
using PixelCrushers.DialogueSystem;

public class ButtonNavigation : MonoBehaviour
{
    private Button[] selectables;
    private DialogueResponseOption[] responses;
    private int currentIndex = 0;
    private RectTransform rect;
    private Player player;
    private CustomDialogueScript customDialogueScript;
    private float previousAxisValue = 0f;

    void Start()
    {
        // Get all Selectable components in the GameObject's children
        selectables = GetComponentsInChildren<Button>().Where(b => b.enabled).ToArray();
        responses = selectables.Select(b => b.gameObject.GetComponent<DialogueResponseOption>()).ToArray();
        rect = GetComponent<RectTransform>();
        player = ReInput.players.GetPlayer(0);
        customDialogueScript = DialogueManager.Instance.gameObject.GetComponent<CustomDialogueScript>();
        UnselectAll();
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
        responses = selectables.Select(b => b.gameObject.GetComponent<DialogueResponseOption>()).ToArray();
        currentIndex = 0;
        UnselectAll();
        Select(currentIndex);
        if (rect == null)
            rect = GetComponent<RectTransform>();
        if (rect != null)
            LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
    }

    void Update()
    {
        float axisValue = player.GetAxis("Navigate Dialogue Responses");

        if (axisValue > 0 && Mathf.Approximately(previousAxisValue, 0f))
        {
            SelectNext(-1);
        }
        else if (axisValue < 0 && Mathf.Approximately(previousAxisValue, 0f))
        {
            SelectNext(1);
        }

        if (player.GetButtonDown("Advance Conversation") && !customDialogueScript.isCoolDown)
        {
            selectables[currentIndex].OnSubmit(null);
        }

        previousAxisValue = axisValue;
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

    public void Select(int idx)
    {
        currentIndex = idx;
        ShowUpTo3Options(idx);
        EventSystem.current.SetSelectedGameObject(selectables[idx].gameObject);
        selectables[idx].OnSelect(null);
        selectables[idx].Select();
        responses[idx].Select();
        if (idx > 0)
        {
            responses[idx].ShowUpArrow();
        }
        if (idx < selectables.Length - 1)
        {
            responses[idx].ShowDownArrow();
        }
        if (rect == null)
            rect = GetComponent<RectTransform>();
        if (rect != null)
            LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
    }

    public void Unselect(int idx)
    {
        if (idx < 0 || idx >= selectables.Length)
        {
            return;
        }
        responses[idx].Deselect();
        selectables[idx].OnDeselect(null);
    }

    private void UnselectAll()
    {
        foreach (DialogueResponseOption r in responses) {
            r.Deselect();
        }
        foreach (Button b in selectables)
        {
            b.OnDeselect(null);
        }
    }

    private void HideAbove(int idx)
    {
        idx--;
        while (idx >= 0)
        {
            selectables[idx].gameObject.SetActive(false);
            idx--;
        }
    }

    private void HideBelow(int idx)
    {
        idx++;
        while (idx < selectables.Length)
        {
            selectables[idx].gameObject.SetActive(false);
            idx++;
        }
    }

    private void ShowUpTo3Options(int idx)
    {
        if (selectables.Length == 2)
        {
            selectables[0].gameObject.SetActive(true);
            responses[0].SetTop();

            selectables[1].gameObject.SetActive(true);
            responses[1].SetBottom();
            return;
        }

        // not at the bottom
        if (idx < selectables.Length - 2)
        {
            HideAbove(idx);
            selectables[idx].gameObject.SetActive(true);
            responses[idx].SetTop();

            selectables[idx + 1].gameObject.SetActive(true);
            responses[idx + 1].SetMiddle();

            selectables[idx + 2].gameObject.SetActive(true);
            responses[idx + 2].SetBottom();
            HideBelow(idx + 2);
        } else if (idx < selectables.Length - 1)
        {
            HideAbove(idx - 1);
            selectables[idx - 1].gameObject.SetActive(true);
            responses[idx - 1].SetTop();

            selectables[idx].gameObject.SetActive(true);
            responses[idx].SetMiddle();

            selectables[idx + 1].gameObject.SetActive(true);
            responses[idx + 1].SetBottom();
            HideBelow(idx + 1);
        }
        else
        {
            HideAbove(idx - 2);
            selectables[idx].gameObject.SetActive(true);
            responses[idx].SetBottom();

            selectables[idx - 1].gameObject.SetActive(true);
            responses[idx - 1].SetMiddle();

            selectables[idx - 2].gameObject.SetActive(true);
            responses[idx - 2].SetTop();
            HideBelow(idx);
        }
    }
}

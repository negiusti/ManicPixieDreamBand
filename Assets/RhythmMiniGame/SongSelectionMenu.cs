using System.Collections.Generic;
using UnityEngine;

public class SongSelectionMenu : MonoBehaviour
{
    private List<SongSelection> songSelections;
    private int idx;
    private BassMiniGame minigame;

    void Start()
    {
        minigame = GetComponentInParent<BassMiniGame>();
        songSelections = new List<SongSelection>();
    }

    private void OnEnable()
    {
        if (songSelections == null)
            Start();
        idx = 0;
        foreach (SongSelection songSelection in GetComponentsInChildren<SongSelection>(includeInactive:true))
        {
            songSelection.gameObject.SetActive(JamCoordinator.UnlockedSongs().Contains(songSelection.gameObject.name));
            if (JamCoordinator.UnlockedSongs().Contains(songSelection.gameObject.name) && !songSelections.Exists(s => s.gameObject.name == songSelection.gameObject.name))
                songSelections.Add(songSelection);
        }
        Highlight(0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            idx++;
            if (idx >= songSelections.Count)
            {
                idx = 0;
            }

            Highlight(idx);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            idx--;
            if (idx < 0)
            {
                idx = songSelections.Count - 1;
            }

            Highlight(idx);
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            Select(idx);
        }
    }

    public void Select(int i)
    {
        if (i < 0)
            return;
        idx = i;
        minigame.SelectSong(songSelections[idx].gameObject.name);
    }

    public void Highlight(int i)
    {
        if (i < 0)
            return;
        idx = i;
        for (int x = 0; x < songSelections.Count; x++)
        {
            if (x == idx)
                songSelections[x].Highlight();
            else
                songSelections[x].Unhighlight();
        }
    }

    public void Select(string song)
    {
        Select(songSelections.FindIndex(s => s.gameObject.name == song));
    }

    public void Highlight(string song)
    {
        Highlight(songSelections.FindIndex(s => s.gameObject.name == song));
    }
}

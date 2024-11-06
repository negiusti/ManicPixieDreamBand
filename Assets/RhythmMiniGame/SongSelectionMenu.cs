using System.Collections.Generic;
using UnityEngine;

public class SongSelectionMenu : MonoBehaviour
{
    private List<SongSelection> songSelections;
    private HashSet<string> unlockedSongs;
    private int idx;

    void Start()
    {
        unlockedSongs = ES3.Load("UnlockedSongs", new HashSet<string> { "UISS", "BodyHorror" });
        songSelections = new List<SongSelection>();
        idx = 0;
        foreach (SongSelection songSelection in GetComponentsInChildren<SongSelection>())
        {
            songSelection.gameObject.SetActive(unlockedSongs.Contains(songSelection.gameObject.name));
            if (unlockedSongs.Contains(songSelection.gameObject.name))
                songSelections.Add(songSelection);
        }
        Highlight(0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.S))
        {
            idx++;
            if (idx >= songSelections.Count)
            {
                idx = 0;
            }

            Highlight(idx);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.A))
        {
            idx--;
            if (idx < 0)
            {
                idx = songSelections.Count - 1;
            }

            Highlight(idx);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Select(idx);
        }
    }

    public void Select(int i)
    {
        if (i < 0)
            return;
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

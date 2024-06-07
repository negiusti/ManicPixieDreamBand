using System.IO;
using System.Text;
using UnityEngine;

public class MusicChartCreator : MonoBehaviour
{
    private AudioSource song;
    private bool hasStarted;
    private StringBuilder notes;
    private StringBuilder times;
    private string timesFilePath;
    private string notesFilePath;
    public string songname;

    // Start is called before the first frame update
    void Start()
    {
        song = GetComponent<AudioSource>();
        string directoryPath = Path.Combine(Application.dataPath, "RhythmGameNotes/BodyHorror");
        timesFilePath = Path.Combine(directoryPath, songname+".txt");
        notesFilePath = Path.Combine(directoryPath, songname+"_notes.txt");

        Debug.Log(timesFilePath + " " + notesFilePath);
        
        // Ensure the directory exists
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
            Debug.Log("Created directory: " + directoryPath);
        }

        notes = new StringBuilder();
        times = new StringBuilder();

        // Append the header
        //csvBuilder.AppendLine("FirstName,LastName,Age");

        // Data to write
        //string data = "This is an example of writing to a file in Unity.";

        // Write data to the file
        //WriteToFile(filePath, data);
    }

    private void Complete()
    {
        WriteToFile(timesFilePath, times.ToString());
        WriteToFile(notesFilePath, notes.ToString());
    }

    private void WriteToFile(string path, string content)
    {
        try
        {
            File.WriteAllText(path, content);
            Debug.Log("Data written to file: " + path);
        }
        catch (IOException e)
        {
            Debug.LogError("An error occurred while writing to the file: " + e.Message);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasStarted && Input.GetKeyDown(KeyCode.Return))
        {
            hasStarted = true;
            song.Play();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            notes.AppendLine("1");
            times.AppendLine(song.time.ToString());
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            notes.AppendLine("2");
            times.AppendLine(song.time.ToString());
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            notes.AppendLine("3");
            times.AppendLine(song.time.ToString());
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            notes.AppendLine("4");
            times.AppendLine(song.time.ToString());
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Complete();
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    //public enum StagePosition
    //{
    //    Left,
    //    Right,
    //    Center
    //};

    public GameObject leftAmp;
    public GameObject rightAmp;
    public PlayInstrument leftInst;
    public PlayInstrument rightInst;
    public PlayInstrument centerInst;
    public CrowdSpawner crowdSpawner;
    private AudioSource audioSource;
    public List<AudioClip> CloudyKingsClips;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartPerformance(Band band, int ticketSales)
    {
        if (crowdSpawner != null)
        {
            if (ticketSales >= 0)
                crowdSpawner.SpawnCrowd(ticketSales);
            else
                crowdSpawner.SpawnCrowd(band);
        }
            
        if (audioSource != null && band.Name.Equals("Cloudy Kings"))
        {
            if (CloudyKingsClips.Count == 0)
                return;
            GameManager.Instance.PauseBGMusic();
            int clipIndex = Random.Range(0, CloudyKingsClips.Count);
            Debug.Log("Playing: " + CloudyKingsClips[clipIndex].name);
            audioSource.clip = CloudyKingsClips[clipIndex];
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void StartPerformance(Band band)
    {
        if (crowdSpawner != null)
            crowdSpawner.SpawnCrowd(band);
        if (audioSource != null && band.Name.Equals("Cloudy Kings"))
        {
            if (CloudyKingsClips.Count == 0)
                return;
            GameManager.Instance.PauseBGMusic();
            int clipIndex = Random.Range(0, CloudyKingsClips.Count);
            Debug.Log("Playing: " + CloudyKingsClips[clipIndex].name);
            audioSource.clip = CloudyKingsClips[clipIndex];
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void StopPerformance()
    {
        if (crowdSpawner != null)
        {
            crowdSpawner.DespawnCrowd();
        }

        leftInst.Stop();
        rightInst.Stop();
        centerInst.Stop();
        if (audioSource != null)
        {
            audioSource.Stop();
            GameManager.Instance.UnpauseBGMusic();
        }
    }

    public PlayInstrument GetInstrument(string position)
    {
        switch (position)
        {
            case "Left":
                return leftInst;
            case "Right":
                return rightInst;
            case "Center":
                return centerInst;
            default:
                return centerInst;
        }
    }
}
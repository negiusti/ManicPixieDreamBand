using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class StarSpawnerScript : MonoBehaviour
{
    //private int[] stringz = { 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 3, 2, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1 };
    //private int[] stringz = { 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 4, 4, 4, 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 4, 4, 4, 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 1, 4, 4, 4, 3, 2, 1, 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 1 };
    public ParticleSystem starParticles;
    public float lagCorrection;
    private bool hasStarted = false;
    public GameObject pinkStar;
    public GameObject blackStar;
    public GameObject purpleStar;
    public GameObject redStar;
    public GameObject buttons;
    public GameObject starter;
    public Vector3 pinkSpawnPosition;
    public Vector3 blackSpawnPosition;
    public Vector3 purpleSpawnPosition;
    public Vector3 redSpawnPosition;
    public float highwaySpeed = 2.5f;
    private int i;
    private float delay;
    private int note;
    private AudioSource hamster;
    private Queue<GameObject> spawnedStars;
    private int hitNotes;
    private int totalNotes;
    private BassMiniGame miniGame;
    private string[] notes;
    private string[] times;
    private bool ready;
    public TextMeshPro scoreTxtTop;
    public TextMeshPro scoreTxtBottom;
    private Coroutine spawnStarCoroutine;

    private void OnLoadCompleted1(AsyncOperationHandle<TextAsset> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            string songNotes = obj.Result.text;
            notes = songNotes.Split(new char[] { '\n', '\r', '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            totalNotes = notes.Length;
        }
        else
        {
            Debug.LogError("Failed to load file. Error: " + obj.OperationException);
        }
        Addressables.Release(obj);
    }

    private void OnLoadCompleted2(AsyncOperationHandle<TextAsset> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            string songTimes = obj.Result.text;
            times = songTimes.Split(new char[] { '\n', '\r', '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            try
            {
                delay = float.Parse(times[i]);
            }
            catch (FormatException)
            {
                Debug.LogError("Invalid float format.");
            }
        }
        else
        {
            Debug.LogError("Failed to load file. Error: " + obj.OperationException);
        }
        Addressables.Release(obj);
        ready = true;
    }

    public float HighwaySpeed()
    {
        return highwaySpeed;
    }

    private void OnEnable()
    {
        i = 0;
        ready = false;
        hasStarted = false;
        hitNotes = 0;
        totalNotes = 0;
        scoreTxtTop.text = "";
        scoreTxtBottom.text = "";
        hamster = this.GetComponent<AudioSource>();
        // Specify the addressable path (use the address you set in the Addressables Group)
        //string addressablePath1 = "Assets/RhythmGameNotes/BodyHorror/BodyHorror_notes.txt";
        //string addressablePath2 = "Assets/RhythmGameNotes/BodyHorror/BodyHorror.txt";
        string addressablePath1 = "Assets/RhythmGameNotes/UISS/UISS_notes.txt";
        string addressablePath2 = "Assets/RhythmGameNotes/UISS/UISS.txt";

        // Load the text file asynchronously
        AsyncOperationHandle<TextAsset> asyncOperation1 = Addressables.LoadAssetAsync<TextAsset>(addressablePath1);
        AsyncOperationHandle<TextAsset> asyncOperation2 = Addressables.LoadAssetAsync<TextAsset>(addressablePath2);

        // Register a callback for when the load operation completes
        asyncOperation1.Completed += OnLoadCompleted1;
        asyncOperation2.Completed += OnLoadCompleted2;

        miniGame = this.transform.parent.gameObject.GetComponent<BassMiniGame>();
    }

    private void OnDisable()
    {
        hamster.Stop();
        if (spawnedStars != null)
        {
            GameObject star;
            while (spawnedStars.TryDequeue(out star))
            {
                Destroy(star);
            }
        }
        if (spawnStarCoroutine != null)
            StopCoroutine(spawnStarCoroutine);
    }


    // Start is called before the first frame update
    void Start()
    {
        spawnedStars = new Queue<GameObject>();
        pinkSpawnPosition = pinkStar.transform.position;
        pinkSpawnPosition.y = pinkStar.transform.position.y;
        blackSpawnPosition = blackStar.transform.position;
        blackSpawnPosition.y = blackStar.transform.position.y;
        purpleSpawnPosition = purpleStar.transform.position;
        purpleSpawnPosition.y = purpleStar.transform.position.y;
        redSpawnPosition = redStar.transform.position;
        redSpawnPosition.y = redStar.transform.position.y;
    }

    private IEnumerator DelayedActions()
    {
        lagCorrection = ES3.Load("LagCalibration", 0f);
        Debug.Log("LAG COORECTION" + lagCorrection);
        while (i < times.Length)
        {
            while (hamster.time < (delay - highwaySpeed + lagCorrection)) 
            {
                // Wait until the desired delay time has passed
                yield return null;
            }

            // Perform the action or event here
            SpawnStar();
            if (++i >= times.Length)
                break;
            try
            {                
                delay = float.Parse(times[i]);
            }
            catch (FormatException)
            {
                Debug.LogError("Invalid float format.");
            }
        }

        AudioController.Clap(12);

        if (GetScore() > 75f && !starParticles.isPlaying)
        {
            Debug.Log("Score: " + GetScore() + "start playing");
            starParticles.Play();
            miniGame.PlayGoodSound();
        }
        else if (GetScore() < 75f && starParticles.isPlaying)
        {
            Debug.Log("Score: " + GetScore() + "stop playing");
            starParticles.Stop();
        }

        // wait until last note passes
        while (hamster.time < (delay + lagCorrection + 0.5f))
            yield return null;

        // Display score here!!!
        if (GetScore() > 80f && !starParticles.isPlaying)
        {
            Debug.Log("Score: " + GetScore() + "start playing" + i);
            starParticles.Play();
        }
        else if (GetScore() < 80f && starParticles.isPlaying)
        {
            Debug.Log("Score: " + GetScore() + "stop playing");
            starParticles.Stop();
        }

        scoreTxtTop.text = GetScore().ToString("F2") + "%";
        
        if (GetScore() > 90f)
        {
            scoreTxtBottom.text += "Well done, rock star!";
        } else if (GetScore() > 70f)
        {
            scoreTxtBottom.text += "Not bad, punk!";
        } else if (GetScore() > 50f)
        {
            scoreTxtBottom.text += "Keep practicing!!";
        } else
        {
            scoreTxtBottom.text += "Be honest, did u even try? :)";
        }
        while (hamster.time < hamster.clip.length - 1f && hamster.isPlaying)
            yield return null;

        //miniGame.Fade();
        miniGame.CloseMiniGame();
        yield return null;
    }

    public void HitNote()
    {
        Debug.Log("HIT NOTE");
        hitNotes++;
    }

    public void WrongNote()
    {
        Debug.Log("WRONG NOTE");
        if (hitNotes > 0)
            hitNotes--;
    }

    void Update()
    {
        if (!hasStarted && ready)
        {
            //if (starter.hasPassed())
            //{
                hamster.Play();
                //runwayDelay = starter.GetRunwayDelay();
                //Debug.Log("runway delay is " + runwayDelay);
                spawnStarCoroutine = StartCoroutine(DelayedActions());
                hasStarted = true;
            //}
        } //else
        //{
        //    if (spawnedStars.Count > 0)
        //    {
        //        // This means the note was "hit"
        //        if (spawnedStars.Peek() == null)
        //        {
        //            spawnedStars.Dequeue();
        //            Debug.Log("HIT NOTE" + hitNotes);
        //            hitNotes++;
        //        }
        //        // This means the note was "missed"
        //        else if (spawnedStars.Peek().transform.localPosition.y < -20f)
        //        {
        //            GameObject starToDestroy = spawnedStars.Dequeue();
        //            //runwayDelay = starToDestroy.GetComponent<StarMoverScript>().GetRunwayDelay();
        //            Destroy(starToDestroy);
        //            Debug.Log("MISS NOTE" + missedNotes);
        //            missedNotes++;
        //        }
                    
        //    }
        //}

        //if (Input.GetKeyDown(KeyCode.Backspace))
        //{
        //    if (spawnStarCoroutine != null)
        //        StopCoroutine(spawnStarCoroutine);
        //    miniGame.CloseMiniGame();
        //}
    }

    public float GetDestinationY()
    {
        return starter.transform.localPosition.y;
    }

    public float GetScore()
    {
        return ((float)hitNotes / (float)(totalNotes)) * 100f;
    }

    public float GetScoreSoFar()
    {
        if (i == 0)
            return 0f;
        return ((float)hitNotes / (float)i) * 100f;
    }

    public IEnumerator Lerp(GameObject go, Vector3 targetLocalPosition, float duration)
    {
        Vector3 startPosition = go.transform.localPosition;

        for (float timePassed = 0f; timePassed < duration; timePassed += Time.deltaTime)
        {
            float factor = timePassed / duration;
            factor = Mathf.Lerp(0, 1, factor);

            if (go != null)
                go.transform.localPosition = Vector3.Lerp(startPosition, targetLocalPosition, factor);

            yield return null;
        }
        if (go != null)
            go.transform.localPosition = targetLocalPosition;
    }

    private void SpawnStar()
    {
        if (i >= 5 && GetScoreSoFar() > 70f && !starParticles.isPlaying)
        {
            Debug.Log("Score so far: " + GetScoreSoFar() + "start playing" + i);
            starParticles.Play();
            miniGame.PlayGoodSound();
        } else if ((i < 5 || GetScoreSoFar() < 70f) && starParticles.isPlaying)
        {
            Debug.Log("Score so far: " + GetScoreSoFar() + "stop playing" + i);
            starParticles.Stop();
        }

        if (i < notes.Length)
        {
            note = int.Parse(notes[i]);
        }

        if (note == 1)
        {
            GameObject p = Instantiate(pinkStar, pinkSpawnPosition, Quaternion.identity);
            p.SetActive(true);
            p.transform.parent = gameObject.transform;
            //p.transform.localScale = pinkStar.transform.localScale;
            spawnedStars.Enqueue(p);
            Vector3 destinationPos = new Vector3(p.transform.localPosition.x, GetDestinationY(), p.transform.localPosition.z);
            StartCoroutine(Lerp(p, destinationPos, highwaySpeed));
        }
        else if (note == 2)
        {
            GameObject b = Instantiate(blackStar, blackSpawnPosition, Quaternion.identity);
            b.SetActive(true);
            b.transform.parent = gameObject.transform;
            //b.transform.localScale = pinkStar.transform.localScale;
            spawnedStars.Enqueue(b);
            Vector3 destinationPos = new Vector3(b.transform.localPosition.x, GetDestinationY(), b.transform.localPosition.z);
            StartCoroutine(Lerp(b, destinationPos, highwaySpeed));
        }
        else if (note == 3)
        {
            GameObject p = Instantiate(purpleStar, purpleSpawnPosition, Quaternion.identity);
            p.SetActive(true);
            p.transform.parent = gameObject.transform;
            //p.transform.localScale = pinkStar.transform.localScale;
            spawnedStars.Enqueue(p);
            Vector3 destinationPos = new Vector3(p.transform.localPosition.x, GetDestinationY(), p.transform.localPosition.z);
            
            StartCoroutine(Lerp(p, destinationPos, highwaySpeed));
        }
        else if (note == 4)
        {
            GameObject x = Instantiate(redStar, redSpawnPosition, Quaternion.identity);
            x.SetActive(true);
            x.transform.parent = gameObject.transform;
            //x.transform.localScale = pinkStar.transform.localScale;
            spawnedStars.Enqueue(x);
            Vector3 destinationPos = new Vector3(x.transform.localPosition.x, GetDestinationY(), x.transform.localPosition.z);
            StartCoroutine(Lerp(x, destinationPos, highwaySpeed));
        }
    }

}

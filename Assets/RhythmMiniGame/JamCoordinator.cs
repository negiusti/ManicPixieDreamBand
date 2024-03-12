using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "JamCoordinator", menuName = "Custom/JamCoordinator")]
public class JamCoordinator : ScriptableObject
{
    private static Stage stage;
    private static Dictionary<string,Movement> musicians;
    private static Camera mainCam;
    private static Camera jamCamera;
    private static bool isJamInSession;
    private static Dictionary<string, JamCamera> jamCameras = new Dictionary<string, JamCamera>{
        {"BandPracticeRoom", new JamCamera(8f, new Vector3(1.63f, 0f, -10f))},
        {"SmallBar", new JamCamera(11.9f, new Vector3(0f, 0f, -10f))}
    };

    private class JamCamera
    {
        public float orthographicSize;
        public Vector3 position;
        public JamCamera (float o, Vector3 p)
        {
            orthographicSize = o;
            position = p;
        }
    }

    // Start is called before the first frame update
    public static void StartJam(string bandname)
    {
        if (isJamInSession)
            return;

        GameManager gm = GameManager.Instance;
        Band band = BandsManager.GetBandByName(bandname);
        stage = FindFirstObjectByType<Stage>();
        stage.StartPerformance(band);

        SpawnCharacters.SpawnBandMembers(band.members);

        musicians = FindObjectsOfType<Character>()
            .Where(x => band.members.Any(m => m.name == x.CharacterName()))
            .ToDictionary(c => c.CharacterName(), c => c.gameObject.GetComponent<Movement>());

        if (musicians.Count() == 0)
            return;

        isJamInSession = true;
        // SwitchToJamCamera();
        foreach(BandMember member in band.members)
        {
            stage.GetInstrument(member.position).Play(musicians[member.name]);
        }
    }

    public static void EndJam()
    {
        //if (!isJamInSession)
        //    return;

        //if (musicians.Count() == 0)
        //    return;

        isJamInSession = false;
        //SwitchToMainCamera();
        FindObjectsOfType<PlayInstrument>().ToList().ForEach(x => x.Stop());
        stage?.StopPerformance();
    }

    private static void SwitchToJamCamera()
    {
        string currLocation = SceneChanger.Instance.GetCurrentScene();
        mainCam = Camera.main;
        jamCamera = Instantiate(mainCam);
        jamCamera.GetComponent<AudioListener>().enabled = false;
        Debug.Log("currLocation: " + currLocation);
        if (jamCameras.ContainsKey(currLocation))
        {
            Debug.Log("contains currLocation: " + currLocation);
            jamCamera.orthographicSize = jamCameras[currLocation].orthographicSize;
            jamCamera.transform.position = jamCameras[currLocation].position;
        }
        mainCam.enabled = false;
    }

    private static void SwitchToMainCamera() {
        mainCam.enabled = true;
        jamCamera.enabled = false;
        Destroy(jamCamera.gameObject);
    }
}

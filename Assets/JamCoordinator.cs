using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "JamCoordinator", menuName = "Custom/JamCoordinator")]
public class JamCoordinator : ScriptableObject
{
    private static PlayInstrument[] playableInstruments;
    private static NPCMovement[] musicians;
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
    public static void StartJam()
    {
        GameManager gm = GameManager.Instance;
        SceneChanger sc = gm.gameObject.GetComponent<SceneChanger>();
        string currLocation = sc.GetCurrentScene();

        if (isJamInSession)
            return;

        playableInstruments = FindObjectsOfType<PlayInstrument>().Where(i => !i.IsBeingPlayed()).ToArray();
        musicians = FindObjectsOfType<Character>().Where(x => x.isMusician).Select(x => x.gameObject.GetComponent<NPCMovement>()).Where(npc => npc != null).ToArray();

        if (musicians.Count() == 0)
            return;

        isJamInSession = true;
        mainCam = Camera.main;
        jamCamera = Instantiate(mainCam);
        jamCamera.GetComponent<AudioListener>().enabled = false;
        if (jamCameras.ContainsKey(currLocation))
        {
            jamCamera.orthographicSize = jamCameras[currLocation].orthographicSize;
            jamCamera.transform.position = jamCameras[currLocation].position;
        }
        mainCam.enabled = false;
        
        int i = 0;
        foreach (PlayInstrument instrument in playableInstruments)
        {
            if (i > musicians.Length - 1)
                return;
            instrument.Play(musicians[i++]);
        }
    }

    public static void EndJam()
    {
        if (!isJamInSession)
            return;

        if (musicians.Count() == 0)
            return;

        isJamInSession = false;
        mainCam.enabled = true;
        jamCamera.enabled = false;
        Destroy(jamCamera.gameObject);
        foreach (PlayInstrument instrument in playableInstruments)
        {
            instrument.Stop();
        }
    }
}

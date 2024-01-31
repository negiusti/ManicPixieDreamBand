using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "JamCoordinator", menuName = "Custom/JamCoordinator")]
public class JamCoordinator : ScriptableObject
{
    private static PlayInstrument[] playableInstruments;
    private static NPCMovement[] musicians;
    private static Camera jamCamera;
    private static Camera mainCam;
    private static bool isJamInSession;

    // Start is called before the first frame update
    public static void StartJam()
    {
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
        jamCamera.orthographicSize = 8f;
        jamCamera.transform.position = new Vector3(1.63f, 0f, -10f);
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

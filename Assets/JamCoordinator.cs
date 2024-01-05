using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "JamCoordinator", menuName = "Custom/JamCoordinator")]
public class JamCoordinator : ScriptableObject
{
    private static PlayInstrument[] playableInstruments;
    private static NPCMovement[] musicians;
    private static Camera jamCamera;
    private static Camera mainCam;

    // Start is called before the first frame update
    public static void StartJam()
    {
        mainCam = Camera.main;
        jamCamera = Instantiate(mainCam);
        jamCamera.orthographicSize = 7f;
        jamCamera.transform.position = new Vector3(1.63f, 0f, -10f);
        mainCam.enabled = false;
        
        playableInstruments = FindObjectsOfType<PlayInstrument>().Where(i => !i.IsBeingPlayed()).ToArray();
        musicians = FindObjectsOfType<Character>().Where(x => x.isMusician).Select(x => x.gameObject.GetComponent<NPCMovement>()).Where(npc => npc != null).ToArray();
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
        mainCam.enabled = true;
        jamCamera.enabled = false;
        Destroy(jamCamera);
        foreach (PlayInstrument instrument in playableInstruments)
        {
            instrument.Stop();
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D.Animation;

[CreateAssetMenu(fileName = "JamCoordinator", menuName = "Custom/JamCoordinator")]
public class JamCoordinator : ScriptableObject
{
    private static Stage stage;
    private static Dictionary<string,Movement> musicians;
    private static readonly List<string> songsToUnlock = new()
    {
            "UISS",
            "BodyHorror",
            "OhNo",
            "BiteMe",
            "GuitarCenter",
            "SugarDaddy",
            "PuzzlePieces",
            "PBaby",
            "ImFine" };
    //private static Camera mainCam;
    //private static Camera jamCamera;
    private static bool isJamInSession;
    //private static Dictionary<string, JamCamera> jamCameras = new Dictionary<string, JamCamera>{
    //    {"BandPracticeRoom", new JamCamera(8f, new Vector3(1.63f, 0f, -10f))},
    //    {"SmallBar", new JamCamera(11.9f, new Vector3(0f, 0f, -10f))}
    //};

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

    public static void Save()
    {
    }

    public static HashSet<string> UnlockedSongs()
    {
        return songsToUnlock.GetRange(0, System.Math.Min(Calendar.Date() + 2, songsToUnlock.Count)).ToHashSet();
    }

    //public static void UnlockSong(string songName)
    //{
    //    unlockedSongs.Add(songName);
    //    Save();
    //}

    public static void Load()
    {
    }

    public static void StartJam(string bandname, double ticketSales = -1)
    {
        if (isJamInSession)
            return;

        isJamInSession = true;
        GameManager gm = GameManager.Instance;
        Band band = BandsManager.GetBandByName(bandname);
        stage = FindFirstObjectByType<Stage>();
        if (stage != null)
            stage.StartPerformance(band, (int)ticketSales);

        SpawnCharacters.SpawnBandMembers(band.members);

        musicians = FindObjectsOfType<Character>()
            .Where(x => band.members.Any(m => m.name == x.CharacterName()))
            .ToDictionary(c => c.CharacterName(), c => c.gameObject.GetComponent<Movement>());

        if (musicians.Count() == 0)
            return;

        //SwitchToJamCamera();
        foreach (BandMember member in band.members)
        {
            stage.GetInstrument(member.position).SetInstrument(member.instrument, (member.instrumentName == null || member.instrumentName.Length == 0 ? Gear.GetGearLabel(member.instrument) : member.instrumentName));
            //Debug.Log("FUCK YUOU THE CATEGORY IS NOW" + stage.GetInstrument(member.position).GetComponent<SpriteResolver>().GetCategory());
            if (member.position == "Left")
            {
                string amp = (member.instrument == "Bass" ? "B_Amp" : "G_Amp");
                //Debug.Log("FUCK YOU setting category and label: " + amp + " " + (band.leftAmp == null || band.leftAmp.Length == 0 ? Gear.GetGearLabel(amp) : band.leftAmp));
                stage.leftAmp.GetComponent<SpriteResolver>().SetCategoryAndLabel(amp, (band.leftAmp == null || band.leftAmp.Length == 0 ? Gear.GetGearLabel(amp) : band.leftAmp));
                //Debug.Log("FUCK YUOU THE CATEGORY IS NOW" + stage.leftAmp.GetComponent<SpriteResolver>().GetCategory());
            }
            if (member.position == "Right")
            {
                string amp = (member.instrument == "Bass" ? "B_Amp" : "G_Amp");
                //Debug.Log("FUCK YOU setting category and label: " + amp + " " + (band.rightAmp == null || band.rightAmp.Length == 0 ? Gear.GetGearLabel(amp) : band.rightAmp));
                stage.rightAmp.GetComponent<SpriteResolver>().SetCategoryAndLabel(amp, (band.rightAmp == null || band.rightAmp.Length == 0 ? Gear.GetGearLabel(amp) : band.rightAmp));
                //Debug.Log("FUCK YUOU THE CATEGORY IS NOW" + stage.rightAmp.GetComponent<SpriteResolver>().GetCategory());
            }
            stage.GetInstrument(member.position).Play(musicians[member.name]);
        }
    }

    //public static void StartJam(string bandname)
    //{
    //    if (isJamInSession)
    //        return;

    //    isJamInSession = true;
    //    GameManager gm = GameManager.Instance;
    //    Band band = BandsManager.GetBandByName(bandname);
    //    stage = FindFirstObjectByType<Stage>();
    //    if (stage != null)
    //        stage.StartPerformance(band);

    //    SpawnCharacters.SpawnBandMembers(band.members);

    //    musicians = FindObjectsOfType<Character>()
    //        .Where(x => band.members.Any(m => m.name == x.CharacterName()))
    //        .ToDictionary(c => c.CharacterName(), c => c.gameObject.GetComponent<Movement>());

    //    if (musicians.Count() == 0)
    //        return;

    //    //SwitchToJamCamera();
    //    foreach(BandMember member in band.members)
    //    {
    //        stage.GetInstrument(member.position).SetInstrument(member.instrument, (member.instrumentName == null || member.instrumentName.Length == 0 ? Gear.GetGearLabel(member.instrument) : member.instrumentName));
    //        //Debug.Log("FUCK YUOU THE CATEGORY IS NOW" + stage.GetInstrument(member.position).GetComponent<SpriteResolver>().GetCategory());
    //        if (member.position == "Left")
    //        {
    //            string amp = (member.instrument == "Bass" ? "B_Amp" : "G_Amp");
    //            //Debug.Log("FUCK YOU setting category and label: " + amp + " " + (band.leftAmp == null || band.leftAmp.Length == 0 ? Gear.GetGearLabel(amp) : band.leftAmp));
    //            stage.leftAmp.GetComponent<SpriteResolver>().SetCategoryAndLabel(amp, (band.leftAmp == null || band.leftAmp.Length == 0 ? Gear.GetGearLabel(amp) : band.leftAmp));
    //            //Debug.Log("FUCK YUOU THE CATEGORY IS NOW" + stage.leftAmp.GetComponent<SpriteResolver>().GetCategory());
    //        }
    //        if (member.position == "Right")
    //        {
    //            string amp = (member.instrument == "Bass" ? "B_Amp" : "G_Amp");
    //            //Debug.Log("FUCK YOU setting category and label: " + amp + " " + (band.rightAmp == null || band.rightAmp.Length == 0 ? Gear.GetGearLabel(amp) : band.rightAmp));
    //            stage.rightAmp.GetComponent<SpriteResolver>().SetCategoryAndLabel(amp, (band.rightAmp == null || band.rightAmp.Length == 0 ? Gear.GetGearLabel(amp) : band.rightAmp));
    //            //Debug.Log("FUCK YUOU THE CATEGORY IS NOW" + stage.rightAmp.GetComponent<SpriteResolver>().GetCategory());
    //        }
    //        stage.GetInstrument(member.position).Play(musicians[member.name]);
    //    }
    //}

    public static void EndJam()
    {
        //if (!isJamInSession)
        //    return;

        //if (musicians.Count() == 0)
        //    return;

        isJamInSession = false;
        //SwitchToMainCamera();
        
        if (stage != null)
            stage.StopPerformance();
        else
            FindObjectsOfType<PlayInstrument>().ToList().ForEach(x => x.Stop());
    }

    //public static void SwitchToJamCamera()
    //{
    //    string currLocation = SceneChanger.Instance.GetCurrentScene();
    //    mainCam = Camera.main;
    //    jamCamera = Instantiate(mainCam);
    //    jamCamera.GetComponent<AudioListener>().enabled = false;
    //    jamCamera.GetComponent<CameraFollow>().enabled = false;
    //    Debug.Log("currLocation: " + currLocation);
    //    if (jamCameras.ContainsKey(currLocation))
    //    {
    //        Debug.Log("contains currLocation: " + currLocation);
    //        jamCamera.orthographicSize = jamCameras[currLocation].orthographicSize;
    //        jamCamera.transform.position = jamCameras[currLocation].position;
    //    }
    //    mainCam.enabled = false;
    //}

    //public static void SwitchToMainCamera() {
    //    if (mainCam == null || jamCamera == null)
    //        return;
    //    mainCam.enabled = true;
    //    jamCamera.enabled = false;
    //    Destroy(jamCamera.gameObject);
    //}
}

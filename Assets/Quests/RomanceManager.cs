using System;
using System.Linq;
using PixelCrushers.DialogueSystem;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RomanceManager", menuName = "Custom/RomanceManager")]
public class RomanceManager : ScriptableObject
{
    private static Dictionary<string, Romance> hornySingles;
    private static HashSet<string> completedRomanceConvos;
    private static Dictionary<string, int> relationshipScores;
    private static string prevScene;

    public static void OnConversationComplete(string convoName)
    {
        // If the convo that was just completed was a romance convo, add it to the list of convos completed
        if (hornySingles.Any(h => h.Value.conversationsData.Any(c => c.conversation == convoName)))
            completedRomanceConvos.Add(convoName);
    }

    private static bool HasSceneChanged() {
        return prevScene != SceneChanger.Instance.GetCurrentScene();
    }

    public static void CheckForRomanceConvo()
    {
        foreach (string single in hornySingles.Keys)
        {
            HashSet<ConversationData> availableConvos = hornySingles[single].conversationsData.Where(c => !completedRomanceConvos.Contains(c.conversation)).ToHashSet();
            foreach (ConversationData romanceConvo in availableConvos)
            {
                bool requirementsMet = ConvoRequirements.RequirementsMet(romanceConvo.requirements) &&
                    (!SceneChanger.Instance.IsLoadingScreenOpen() || !DialogueManager.Instance.gameObject.GetComponent<CustomDialogueScript>().IsTxtConvo(romanceConvo.conversation));
                if (requirementsMet)
                {
                    if ((romanceConvo.trigger == null || romanceConvo.trigger == "Start") && !DialogueManager.IsConversationActive)
                    {
                        DialogueManager.Instance.gameObject.GetComponent<CustomDialogueScript>().StartConversation(romanceConvo);
                        //completedRomanceConvos.Add(romanceConvo.conversation);
                    } else // if (HasSceneChanged()) // Only try to do this once per scene?? 
                    {
                        // this is for spawning participants with triggers for optional dialogues
                        SpawnCharacters.SpawnParticipants(romanceConvo.participants, romanceConvo.conversation);
                    }
                    //prevScene = SceneChanger.Instance.GetCurrentScene();
                }
            }
        }
        //prevScene = SceneChanger.Instance.GetCurrentScene();
    }

    public static int GetRelationshipScore(string npc)
    {
        if (relationshipScores == null)
            Load();
        if (!relationshipScores.ContainsKey(npc))
        {
            relationshipScores.Add(npc, 0);
        }
        return relationshipScores[npc];
    }

    public static void ChangeRelationshipScore(string npc, double delta)
    {
        if (relationshipScores.ContainsKey(npc))
            relationshipScores[npc] += (int)delta;
        else
            relationshipScores.Add(npc, (int)delta);

        // Make sure this updates contact emojis
        Phone.Instance.UpdateRomanceEmoji(npc);
    }

    public static void Save()
    {
        ES3.Save("CompletedRomanceConvos", completedRomanceConvos);
        ES3.Save("RelationshipScores", relationshipScores);
    }

    public static void Load()
    {
        hornySingles = ConversationJson.GetRomancesData().romances.ToDictionary(r => r.npcName, r => r);
        completedRomanceConvos = ES3.Load("CompletedRomanceConvos", new HashSet<string>());
        relationshipScores = ES3.Load("RelationshipScores", new Dictionary<string, int>());
    }
}

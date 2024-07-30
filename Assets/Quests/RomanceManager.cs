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

    public static bool  CheckForRomanceConvo()
    {
        foreach (string single in hornySingles.Keys)
        {
            HashSet<ConversationData> availableConvos = hornySingles[single].conversationsData.Where(c => !completedRomanceConvos.Contains(c.conversation)).ToHashSet();
            foreach (ConversationData romanceConvo in availableConvos)
            {
                bool requirementsMet = ConvoRequirements.RequirementsMet(romanceConvo.requirements);
                if (requirementsMet)
                {
                    DialogueManager.Instance.gameObject.GetComponent<CustomDialogueScript>().StartConversation(romanceConvo);
                    completedRomanceConvos.Add(romanceConvo.conversation);
                    return requirementsMet;
                }
            }
        }
        return false;
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

#if USE_ARCWEAVE

using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace PixelCrushers.DialogueSystem.ArcweaveSupport
{

    #region Helper Types

    [Serializable]
    public class ArcweaveConversationInfo
    {
        public string boardGuid;
        public int startIndex; // Index into board's sorted elements list.
        public int actorIndex; // Index into sorted components list.
        public int conversantIndex; // Index into sorted components list.
        public ArcweaveConversationInfo() { }
        public ArcweaveConversationInfo(string boardGuid) { this.boardGuid = boardGuid; }
    }

    public enum ArcweaveBoardType { Ignore, Conversation, Quest }
    public enum ArcweaveComponentType { Ignore, Player, NPC, Item, Location }

    // Temporary tree structure for boards so we only show leaf nodes as possible conversations, and can prepend parents to conversation titles.
    public class ArcweaveBoardNode
    {
        public string guid;
        public Board board;
        public List<ArcweaveBoardNode> children = new List<ArcweaveBoardNode>();
        public ArcweaveBoardNode parent;

        public ArcweaveBoardNode(string guid, Board board, ArcweaveBoardNode parent)
        {
            this.guid = guid;
            this.board = board;
            this.parent = parent;
        }
    }

    [Serializable]
    public class ArcweaveImportPrefs
    {
        public string sourceFilename = string.Empty;
        public string outputFolder = "Assets";
        public string databaseFilename = "Dialogue Database";
        public bool overwrite = false;
        public bool merge = false;

        public string arcweaveProjectPath;
        public string contentJson;

        public List<string> questBoardGuids = new List<string>();

        public List<ArcweaveConversationInfo> conversationInfo = new List<ArcweaveConversationInfo>();

        public List<string> playerComponentGuids = new List<string>();
        public List<string> npcComponentGuids = new List<string>();
        public List<string> itemComponentGuids = new List<string>();
        public List<string> locationComponentGuids = new List<string>();

        public bool boardsFoldout = true;
        public bool componentsFoldout = true;

        public bool importPortraits = true;
        public bool importDialogueEntryAttributes = false;
        public bool importGuids = false;
        public int numPlayers = 1;
        public string globalVariables;

        public string prefsPath;
    }

    #endregion

    public class ArcweaveImporter
    {

        #region Variables

        public bool merge;

        public string arcweaveProjectPath;
        public string contentJson;

        public List<string> questBoardGuids = new List<string>();

        public List<ArcweaveConversationInfo> conversationInfo = new List<ArcweaveConversationInfo>();

        public List<string> playerComponentGuids = new List<string>();
        public List<string> npcComponentGuids = new List<string>();
        public List<string> itemComponentGuids = new List<string>();
        public List<string> locationComponentGuids = new List<string>();

        public bool boardsFoldout = true;
        public bool componentsFoldout = true;
        public int numPlayers = 1;
        public List<string> globalVariables = new List<string>();

        public bool importPortraits = true;
        public bool importDialogueEntryAttributes = false;
        public bool importGuids = false;

        public Template template = Template.FromDefault();

        public DialogueDatabase database;

        public ArcweaveProject arcweaveProject { get; protected set; } = null;

        public Dictionary<string, string[]> conversationElements { get; protected set; } = new Dictionary<string, string[]>();
        public Dictionary<string, string> elementNames = new Dictionary<string, string>();
        public string[] componentNames = new string[0];

        public ArcweaveBoardNode rootBoardNode = null;
        public Dictionary<string, Board> leafBoards { get; protected set; } = new Dictionary<string, Board>();

        protected Dictionary<string, ArcweaveType> arcweaveLookup = new Dictionary<string, ArcweaveType>();
        protected Dictionary<string, DialogueEntry> dialogueEntryLookup = new Dictionary<string, DialogueEntry>();
        protected Dictionary<string, Actor> actorLookup = new Dictionary<string, Actor>();
        protected int currentPlayerID;
        protected int currentNpcID;
        protected const string NoneSequence = "None()";
        protected const string ContinueSequence = "Continue()";
        private const string DeleteTag = "$$Delete$$";

        #endregion

        #region Setup

        /// <summary>
        /// Prepares the Arcweave importer with parameters ready to perform an import.
        /// </summary>
        /// <param name="arcweaveProjectPath">Path to Arcweave project files. This path should contain project_settings.json exported from Arcweave.</param>
        /// <param name="contentJson">Arcweave project_settings.json text. Can be blank. If non-blank, import uses this instead of reading JSON file from arcweaveProjectPath.</param>
        /// <param name="questBoardGuids">Settings from Arcweave importer window's prefs.</param>
        /// <param name="conversationInfo">Settings from Arcweave importer window's prefs.</param>
        /// <param name="playerComponentGuids">Settings from Arcweave importer window's prefs.</param>
        /// <param name="npcComponentGuids">Settings from Arcweave importer window's prefs.</param>
        /// <param name="itemComponentGuids">Settings from Arcweave importer window's prefs.</param>
        /// <param name="locationComponentGuids">Settings from Arcweave importer window's prefs.</param>
        /// <param name="boardsFoldout">Settings from Arcweave importer window's prefs.</param>
        /// <param name="componentsFoldout">Settings from Arcweave importer window's prefs.</param>
        /// <param name="importPortraits">Assign portrait images to actors. (Editor only)</param>
        /// <param name="importDialogueEntryAttributes">Import dialogue element attributes as custom fields in dialogue entry.</param>
        /// <param name="importGuids">In actors, locations, conversations, and quests, add a field containing Arcweave GUID.</param>
        /// <param name="merge">Merge into existing database, keeping/overwriting existing assets, instead of clearing database first.</param>
        /// <param name="numPlayers">Set to value greater than 1 to import set of variables for each player.</param>
        /// <param name="globalVariables">If numPlayers > 1, this is a comma-separated list of global variables that aren't player-specific.</param>
        /// <param name="template">Template to use to create new actors, conversations, etc.</param>
        public virtual void Setup(string arcweaveProjectPath,
            string contentJson,
            List<string> questBoardGuids,
            List<ArcweaveConversationInfo> conversationInfo,
            List<string> playerComponentGuids,
            List<string> npcComponentGuids,
            List<string> itemComponentGuids,
            List<string> locationComponentGuids,
            bool boardsFoldout,
            bool componentsFoldout,
            bool importPortraits,
            bool importDialogueEntryAttributes,
            bool importGuids,
            int numPlayers,
            string globalVariables,
            bool merge,
            Template template)
        {
            this.arcweaveProjectPath = arcweaveProjectPath;
            this.contentJson = contentJson;
            this.questBoardGuids = questBoardGuids;
            this.conversationInfo = conversationInfo;
            this.playerComponentGuids = playerComponentGuids;
            this.npcComponentGuids = npcComponentGuids;
            this.itemComponentGuids = itemComponentGuids;
            this.locationComponentGuids = locationComponentGuids;
            this.boardsFoldout = boardsFoldout;
            this.componentsFoldout = componentsFoldout;
            this.importPortraits = importPortraits;
            this.importDialogueEntryAttributes = importDialogueEntryAttributes;
            this.importGuids = importGuids;
            this.numPlayers = numPlayers;
            this.globalVariables = ParseGlobalVariables(globalVariables);
            this.merge = merge;
            this.template = (template != null) ? template : Template.FromDefault();
        }

        /// <summary>
        /// Prepares the Arcweave importer with parameters ready to perform an import.
        /// </summary>
        /// <param name="arcweaveProjectPath">Path to Arcweave project files. This path should contain project_settings.json exported from Arcweave.</param>
        /// <param name="contentJson">Arcweave project_settings.json text. Can be blank. If non-blank, import uses this instead of reading JSON file from arcweaveProjectPath.</param>
        /// <param name="questBoardGuids">Settings from Arcweave importer window's prefs.</param>
        /// <param name="conversationInfo">Settings from Arcweave importer window's prefs.</param>
        /// <param name="playerComponentGuids">Settings from Arcweave importer window's prefs.</param>
        /// <param name="npcComponentGuids">Settings from Arcweave importer window's prefs.</param>
        /// <param name="itemComponentGuids">Settings from Arcweave importer window's prefs.</param>
        /// <param name="locationComponentGuids">Settings from Arcweave importer window's prefs.</param>
        /// <param name="boardsFoldout">Settings from Arcweave importer window's prefs.</param>
        /// <param name="componentsFoldout">Settings from Arcweave importer window's prefs.</param>
        /// <param name="importPortraits">Assign portrait images to actors. (Editor only)</param>
        /// <param name="importGuids">In actors, locations, conversations, and quests, add a field containing Arcweave GUID.</param>
        /// <param name="merge">Merge into existing database, keeping/overwriting existing assets, instead of clearing database first.</param>
        /// <param name="numPlayers">Set to value greater than 1 to import set of variables for each player.</param>
        /// <param name="globalVariables">If numPlayers > 1, this is a comma-separated list of global variables that aren't player-specific.</param>
        /// <param name="template">Template to use to create new actors, conversations, etc.</param>
        public virtual void Setup(string arcweaveProjectPath,
            string contentJson,
            List<string> questBoardGuids,
            List<ArcweaveConversationInfo> conversationInfo,
            List<string> playerComponentGuids,
            List<string> npcComponentGuids,
            List<string> itemComponentGuids,
            List<string> locationComponentGuids,
            bool boardsFoldout,
            bool componentsFoldout,
            bool importPortraits,
            bool importGuids,
            int numPlayers,
            string globalVariables,
            bool merge,
            Template template)
        {
            Setup(arcweaveProjectPath, contentJson, questBoardGuids, conversationInfo, playerComponentGuids, npcComponentGuids,
                itemComponentGuids, locationComponentGuids, boardsFoldout, componentsFoldout, importPortraits, false,
                importGuids, numPlayers, globalVariables, merge, template);
        }

        /// <summary>
        /// Prepares the Arcweave importer with parameters ready to perform an import.
        /// </summary>
        /// <param name="jsonPrefs">JSON text of prefs from Arcweave importer window, with optional contentJson to use instead of reading project_settings.json file.</param>
        /// <param name="database">Database in which to add the imported content.</param>
        /// <param name="template">Template to use to create new actors, conversations, etc.</param>
        public virtual void Setup(string jsonPrefs, DialogueDatabase database, Template template)
        {
            var prefs = JsonUtility.FromJson<ArcweaveImportPrefs>(jsonPrefs);
            if (prefs != null)
            {
                Setup(prefs.arcweaveProjectPath,
                    prefs.contentJson,
                    prefs.questBoardGuids,
                    prefs.conversationInfo,
                    prefs.playerComponentGuids,
                    prefs.npcComponentGuids,
                    prefs.itemComponentGuids,
                    prefs.locationComponentGuids,
                    prefs.boardsFoldout,
                    prefs.componentsFoldout,
                    prefs.importPortraits,
                    prefs.importDialogueEntryAttributes,
                    prefs.importGuids,
                    prefs.numPlayers,
                    prefs.globalVariables,
                    prefs.merge,
                    template);
                this.database = database;
            }
        }

        public virtual void Clear()
        {
            arcweaveProject = null;
        }

        protected virtual List<string> ParseGlobalVariables(string s)
        {
            var list = new List<string>();
            if (!string.IsNullOrEmpty(s))
            {
                foreach (var v in s.Split(','))
                {
                    list.Add(v.Trim());
                }
            }
            return list;
        }

        #endregion

        #region Load JSON

        /// <summary>
        /// Imports Arcweave exported content into a dialogue database.
        /// </summary>
        /// <param name="jsonPrefs">JSON text of prefs from Arcweave importer window, with optional contentJson to use instead of reading project_settings.json file.</param>
        /// <param name="database">Database in which to add the imported content.</param>
        /// <param name="template">Template to use to create new actors, conversations, etc.</param>
        public virtual void Import(string jsonPrefs, DialogueDatabase database, Template template)
        {
            Setup(jsonPrefs, database, template);
            LoadAndConvert();
        }

        public virtual void LoadAndConvert()
        {
            LoadJson();
            Convert();
        }

        public virtual bool IsJsonLoaded()
        {
            return arcweaveProject != null && arcweaveProject.boards != null && arcweaveProject.boards.Count > 0;
        }

        public virtual bool LoadJson()
        {
            try
            {
                arcweaveProject = null;
                var pathInProject = (!string.IsNullOrEmpty(arcweaveProjectPath) && arcweaveProjectPath.StartsWith("Assets"))
                    ? arcweaveProjectPath.Substring("Assets".Length) : string.Empty;
                var jsonFilename = Application.dataPath + pathInProject + "/project_settings.json";

                //Check if file exists, if not check for content json. If both are not provided an import is not possible
                var json = "";
                if (!String.IsNullOrEmpty(contentJson))
                {
                    json = contentJson;
                }
                else if (System.IO.File.Exists(jsonFilename))
                {
                    json = System.IO.File.ReadAllText(jsonFilename);
                }
                else
                {
                    Debug.LogError($"Dialogue System: Arcweave JSON file '{jsonFilename}' doesn't exist.");
                    return false;
                }
                if (string.IsNullOrEmpty(json))
                {
                    Debug.LogError($"Dialogue System: Unable to read '{jsonFilename}'.");
                    return false;
                }
                arcweaveProject = Newtonsoft.Json.JsonConvert.DeserializeObject<ArcweaveProject>(json);
                if (!IsJsonLoaded())
                {
                    Debug.LogError($"Dialogue System: Arcweave project '{jsonFilename}' is empty or could not be loaded.");
                    return false;
                }
                Debug.Log($"Dialogue System: Successfully loaded {jsonFilename} containing {arcweaveProject.boards.Count} boards.");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Dialogue System: Arcweave Project could not be deserialized: {e.Message}");
            }
            try
            {
                CatalogAllArcweaveTypes();
                RecordElementNames();
                RecordComponentNames();
                RecordBoardHierarchy();
                SortBoardElements();
                conversationElements.Clear();
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Dialogue System: Unable to cache element names in deserialized Arcweave Project: {e.Message}");
                arcweaveProject = null;
                return false;
            }
        }

        protected virtual void SortBoardElements()
        {
            foreach (var board in leafBoards.Values)
            {
                board.elements.Sort((a, b) => elementNames[a].CompareTo(elementNames[b]));
            }
        }

        protected virtual void RecordBoardHierarchy()
        {
            // Record hierarchy so we can identify leaf nodes and path for conversation titles.
            rootBoardNode = null;
            foreach (var kvp in arcweaveProject.boards)
            {
                string guid = kvp.Key;
                Board board = kvp.Value;
                if (board.root)
                {
                    rootBoardNode = new ArcweaveBoardNode(guid, board, null);
                    break;
                }
            }
            if (rootBoardNode == null)
            {
                Debug.LogError("Dialogue System: Can't find root board in Arcweave Project.");
                arcweaveProject = null;
                return;
            }
            leafBoards.Clear();
            RecordBoardChildren(rootBoardNode);
        }

        protected virtual void RecordBoardChildren(ArcweaveBoardNode boardNode)
        {
            if (boardNode == null || boardNode.board == null) return;
            if (boardNode.board.children == null)
            {
                if (!leafBoards.ContainsKey(boardNode.guid))
                {
                    leafBoards.Add(boardNode.guid, boardNode.board);
                }
            }
            else
            {
                foreach (var childGuid in boardNode.board.children)
                {
                    var childBoard = arcweaveLookup[childGuid] as Board;
                    var childNode = new ArcweaveBoardNode(childGuid, childBoard, boardNode);
                    boardNode.children.Add(childNode);
                    RecordBoardChildren(childNode);
                }
            }
        }

        protected virtual void RecordElementNames()
        {
            // Sort elements:
            var elementList = arcweaveProject.elements.ToList();
            elementList.Sort((kvp1, kvp2) => GetElementName(kvp1.Value).CompareTo(GetElementName(kvp2.Value)));
            arcweaveProject.elements.Clear();
            elementList.ForEach(x => arcweaveProject.elements.Add(x.Key, x.Value));

            // Record elements:
            elementNames.Clear();
            foreach (var kvp in arcweaveProject.elements)
            {
                var name = GetElementName(kvp.Value);
                elementNames[kvp.Key] = name;
            }
        }

        protected string GetElementName(Element element)
        {
            if (element == null) return string.Empty;
            var name = TouchUpRichText(element.title);
            if (string.IsNullOrEmpty(name)) name = TouchUpRichText(element.content);
            return name.Replace("/", "\u2215");
        }

        protected virtual void RecordComponentNames()
        {
            // Add default Player actor:
            var list = new List<string>();
            list.Add("Player");

            // Sort components:
            var componentList = arcweaveProject.components.ToList();
            componentList.Sort((kvp1, kvp2) => TouchUpRichText(kvp1.Value.name).CompareTo(TouchUpRichText(kvp2.Value.name)));
            arcweaveProject.components.Clear();
            componentList.ForEach(x => arcweaveProject.components.Add(x.Key, x.Value));

            // Add components:
            foreach (var kvp in arcweaveProject.components)
            {
                list.Add(TouchUpRichText(kvp.Value.name));
            }
            componentNames = list.ToArray();
        }

        #endregion

        #region Import

        public virtual void Convert()
        {
            if (IsJsonLoaded())
            {
                CopySourceToDialogueDatabase(database);
                TouchUpDialogueDatabase(database);
            }
        }

        public virtual void CopySourceToDialogueDatabase(DialogueDatabase database)
        {
            this.database = database;
            database.description = arcweaveProject.name;
            AddVariables();
            AddLocations();
            AddActors(playerComponentGuids, "Player", true);
            AddActors(npcComponentGuids, "NPC", false);
            AddConversations();
            AddQuests();
        }

        protected virtual void CatalogAllArcweaveTypes()
        {
            arcweaveLookup.Clear();
            CatalogDictionary(arcweaveProject.boards);
            CatalogDictionary(arcweaveProject.notes);
            CatalogDictionary(arcweaveProject.elements);
            CatalogDictionary(arcweaveProject.jumpers);
            CatalogDictionary(arcweaveProject.connections);
            CatalogDictionary(arcweaveProject.branches);
            CatalogDictionary(arcweaveProject.components);
            CatalogDictionary(arcweaveProject.attributes);
            CatalogDictionary(arcweaveProject.assets);
            CatalogDictionary(arcweaveProject.variables);
            CatalogDictionary(arcweaveProject.conditions);
        }

        protected virtual void CatalogDictionary<T>(Dictionary<string, T> dict) where T : ArcweaveType
        {
            foreach (var kvp in dict)
            {
                arcweaveLookup[kvp.Key] = kvp.Value;
            }
        }

        protected T LookupArcweave<T>(string guid) where T : ArcweaveType
        {
            ArcweaveType value;
            return arcweaveLookup.TryGetValue(guid, out value) ? (value as T) : null;
        }

        protected virtual void AddVariables()
        {
            AddVariables("");
            if (numPlayers > 1)
            {
                for (var i = 0; i < numPlayers; i++)
                {
                    AddVariables("Player" + i + "_");
                }
            }
        }

        protected virtual void AddVariables(string prefix)
        {
            foreach (var kvp in arcweaveProject.variables)
            {
                ArcweaveVariable arcweaveVariable = kvp.Value;
                if (!string.IsNullOrEmpty(arcweaveVariable.name))
                {
                    if (merge) database.variables.RemoveAll(x => x.Name == arcweaveVariable.name);
                    var isGlobalVariable = globalVariables.Contains(arcweaveVariable.name);
                    if ((isGlobalVariable && !string.IsNullOrEmpty(prefix))) // Is global so don't need player-specific version.
                    {
                        continue;
                    }
                    var variable = template.CreateVariable(template.GetNextVariableID(database), prefix + arcweaveVariable.name, string.Empty);
                    database.variables.Add(variable);
                    switch (arcweaveVariable.type)
                    {
                        case "boolean":
                            variable.Type = FieldType.Boolean;
                            variable.InitialBoolValue = (bool)(arcweaveVariable.value as Newtonsoft.Json.Linq.JValue).Value;
                            break;
                        case "float":
                            variable.Type = FieldType.Number;
                            variable.InitialFloatValue = (float)(arcweaveVariable.value as Newtonsoft.Json.Linq.JValue).Value;
                            break;
                        case "integer":
                            variable.Type = FieldType.Number;
                            if ((arcweaveVariable.value as Newtonsoft.Json.Linq.JValue).Value.GetType() == typeof(Int64))
                            {
                                variable.InitialFloatValue = (Int64)(arcweaveVariable.value as Newtonsoft.Json.Linq.JValue).Value;
                            }
                            else if ((arcweaveVariable.value as Newtonsoft.Json.Linq.JValue).Value.GetType() == typeof(int))
                            {
                                variable.InitialFloatValue = (int)(arcweaveVariable.value as Newtonsoft.Json.Linq.JValue).Value;
                            }
                            else
                            {
                                Debug.Log($"Variable {kvp.Key} '{arcweaveVariable.name}' is not an int, type is: {(arcweaveVariable.value as Newtonsoft.Json.Linq.JValue).Value.GetType().Name}");
                            }
                            break;
                        case "string":
                            variable.Type = FieldType.Text;
                            variable.InitialValue = (string)(arcweaveVariable.value as Newtonsoft.Json.Linq.JValue).Value;
                            break;
                        default:
                            Debug.LogWarning($"Dialogue System: Can't import variable {variable.Name} type '{arcweaveVariable.type}'.");
                            break;
                    }
                }
            }

            // Sort variables alphabetically:
            database.variables.Sort((a, b) => a.Name.CompareTo(b.Name));
        }

        protected virtual void AddLocations()
        {
            foreach (string guid in locationComponentGuids)
            {
                ArcweaveComponent component;
                if (arcweaveProject.components.TryGetValue(guid, out component))
                {
                    if (merge) database.locations.RemoveAll(x => x.Name == component.name);
                    var location = template.CreateLocation(template.GetNextLocationID(database), component.name);
                    database.locations.Add(location);
                    AddAttributes(location.fields, component.attributes);
                }
            }
        }

        protected static string[] ImageExtensions = new string[] { ".png", ".jpg", ".tga", ".tif", ".psd", ".psb" };

        protected virtual void AddActors(List<string> guids, string defaultActorName, bool isPlayer)
        {
            Actor actor = null;
            foreach (var guid in guids)
            {
                ArcweaveComponent component;
                if (arcweaveProject.components.TryGetValue(guid, out component))
                {
                    if (merge) database.actors.RemoveAll(x => x.Name == component.name);
                    actor = template.CreateActor(template.GetNextActorID(database), component.name, isPlayer);
                    actorLookup[guid] = actor;
                    database.actors.Add(actor);

                    AddAttributes(actor.fields, component.attributes);

#if UNITY_EDITOR
                    // Portrait:
                    if (importPortraits && component.assets != null && component.assets.cover != null)
                    {
                        ArcweaveAsset asset;
                        if (arcweaveProject.assets.TryGetValue(component.assets.cover.id, out asset))
                        {
                            Sprite sprite;
                            Texture2D texture;
                            TryLoadPortrait(asset.name, out sprite, out texture);
                            if (sprite != null)
                            {
                                actor.spritePortrait = sprite;
                            }
                            else if (texture != null)
                            {
                                actor.portrait = texture;
                            }
                            else
                            {
                                Debug.LogWarning($"Dialogue System: Can't find portrait image '{GetAssetPath(asset.name)}' for actor {actor.Name}.");
                            }
                        }

                        // Alternate portraits:
                        foreach (var attributeId in component.attributes)
                        {
                            Attribute attribute;
                            if (arcweaveProject.attributes.TryGetValue(attributeId, out attribute))
                            {
                                if (attribute != null && attribute.name != null &&
                                    attribute.name.StartsWith("Portrait "))
                                {
                                    int portraitNum = SafeConvert.ToInt(attribute.name.Substring("Portrait ".Length).Trim());
                                    var assetName = (string)attribute.value.data;
                                    Sprite sprite = null;
                                    Texture2D texture = null;
                                    foreach (var extension in ImageExtensions)
                                    {
                                        TryLoadPortrait(assetName + extension, out sprite, out texture);
                                        if (sprite != null || texture != null) break;
                                    }
                                    if (sprite != null)
                                    {
                                        for (int i = actor.spritePortraits.Count; i < (portraitNum - 1); i++)
                                        {
                                            actor.spritePortraits.Add(null);
                                        }
                                        actor.spritePortraits[portraitNum - 2] = sprite;
                                    }
                                    else if (texture != null)
                                    {
                                        for (int i = actor.alternatePortraits.Count; i < portraitNum; i++)
                                        {
                                            actor.alternatePortraits.Add(null);
                                        }
                                        actor.alternatePortraits[portraitNum - 2] = texture;
                                    }
                                    else
                                    {
                                        Debug.LogWarning($"Dialogue System: Can't find portrait {portraitNum} image '{GetAssetPath(assetName)}' for actor {actor.Name}.");
                                    }
                                }
                            }
                        }
                    }
#endif
                }
            }
            if (actor == null)
            {
                if (merge) database.actors.RemoveAll(x => x.Name == defaultActorName);
                actor = template.CreateActor(template.GetNextActorID(database), defaultActorName, isPlayer);
                database.actors.Add(actor);
            }
        }

        protected virtual void TryLoadPortrait(string assetName, out Sprite sprite, out Texture2D texture)
        {
#if UNITY_EDITOR
            string assetPath = GetAssetPath(assetName);
            texture = null;
            sprite = UnityEditor.AssetDatabase.LoadAssetAtPath(assetPath, typeof(Sprite)) as Sprite;
            if (sprite == null)
            {
                texture = UnityEditor.AssetDatabase.LoadAssetAtPath(assetPath, typeof(Texture2D)) as Texture2D;
            }
#else
            sprite = null;
            texture = null;
#endif
        }

        protected virtual string GetAssetPath(string assetName)
        {
            var pathInProject = (!string.IsNullOrEmpty(arcweaveProjectPath) && arcweaveProjectPath.StartsWith("Assets"))
                                ? arcweaveProjectPath : "Assets";
            return pathInProject + "/assets/" + assetName;
        }

        protected virtual void AddAttributes(List<Field> fields, List<string> attributes)
        {
            foreach (string attributeGuid in attributes)
            {
                Attribute attribute;
                if (arcweaveProject.attributes.TryGetValue(attributeGuid, out attribute))
                {
                    switch (attribute.value.type)
                    {
                        case "string":
                            var text = TouchUpRichText((string)(attribute.value.data as Newtonsoft.Json.Linq.JValue).Value);
                            Field.SetValue(fields, attribute.name, text);
                            break;
                    }
                }
            }
        }

        protected virtual Actor FindActorByComponentIndex(int index)
        {
            return (0 <= index && index < componentNames.Length) ? database.actors.Find(x => x.Name == componentNames[index]) : null;
        }

        protected virtual void AddConversations()
        {
            dialogueEntryLookup.Clear();

            foreach (var conversationInfo in conversationInfo)
            {
                Board board;
                var boardGuid = conversationInfo.boardGuid;
                if (arcweaveProject.boards.TryGetValue(boardGuid, out board))
                {
                    // Determine current conversation participants:
                    var player = FindActorByComponentIndex(conversationInfo.actorIndex) ?? database.actors.Find(x => x.IsPlayer);
                    var npc = FindActorByComponentIndex(conversationInfo.conversantIndex) ?? database.actors.Find(x => !x.IsPlayer);
                    currentPlayerID = (player != null) ? player.id : 0;
                    currentNpcID = (npc != null) ? npc.id : 1;

                    // Create conversation:
                    var conversationTitle = GetConversationTitle(board);
                    if (merge) database.conversations.RemoveAll(x => x.Title == conversationTitle);
                    var conversation = template.CreateConversation(template.GetNextConversationID(database), conversationTitle);
                    database.conversations.Add(conversation);
                    conversation.ActorID = currentPlayerID;
                    conversation.ConversantID = currentNpcID;

                    // Create <START> node:
                    var startEntry = template.CreateDialogueEntry(0, conversation.id, "START");
                    dialogueEntryLookup[boardGuid] = startEntry;
                    conversation.dialogueEntries.Add(startEntry);
                    startEntry.ActorID = currentPlayerID;
                    startEntry.ConversantID = currentNpcID;

                    // Add Elements:
                    foreach (var elementGuid in board.elements)
                    {
                        var element = LookupArcweave<Element>(elementGuid);
                        if (element == null) continue;
                        var entry = GetOrCreateDialogueEntry(conversation, elementGuid);
                        entry.Title = StripHtmlCodes(element.title);
                        entry.ActorID = GetActorIDFromTitle(element, currentNpcID);
                        entry.ConversantID = currentPlayerID;
                        SetActorIDsFromComponents(entry, element);
                        ProcessContent(entry, element.content);
                    }

                    // Add Connections:
                    foreach (var connectionGuid in board.connections)
                    {
                        var connection = LookupArcweave<Connection>(connectionGuid);
                        if (connection == null) continue;
                        string code;
                        var connectionLabel = ExtractCode(connection.label, out code);
                        var isBlank = string.IsNullOrEmpty(connectionLabel);
                        var entry = GetOrCreateDialogueEntry(conversation, connectionGuid);
                        entry.ActorID = isBlank ? currentNpcID : currentPlayerID;
                        entry.ConversantID = currentPlayerID;
                        entry.isGroup = isBlank;
                        entry.DialogueText = TouchUpRichText(connectionLabel);
                        //--- Added later: entry.userScript = string.IsNullOrEmpty(code) ? string.Empty : ConvertArcscriptToLua(code);
                        if (isBlank && string.IsNullOrEmpty(code)) entry.Title = DeleteTag;
                    }

                    // Add Branches:
                    foreach (var branchGuid in board.branches)
                    {
                        var branch = LookupArcweave<Branch>(branchGuid);
                        if (branch == null) continue;
                        var entry = GetOrCreateDialogueEntry(conversation, branchGuid);
                        entry.ActorID = currentNpcID;
                        entry.ConversantID = currentPlayerID;
                        entry.isGroup = true;
                    }

                    // Add Jumpers:
                    foreach (var jumperGuid in board.jumpers)
                    {
                        var jumper = LookupArcweave<Jumper>(jumperGuid);
                        if (jumper == null) continue;
                        var entry = GetOrCreateDialogueEntry(conversation, jumperGuid);
                        entry.ActorID = currentNpcID;
                        entry.ConversantID = currentPlayerID;
                        entry.Title = "Jumper." + jumper.elementId; // Will connect jumpers after creating all conversations and nodes.
                        entry.isGroup = true;
                    }

                    // Add Conditions:
                    foreach (var branchGuid in board.branches)
                    {
                        var branch = LookupArcweave<Branch>(branchGuid);
                        if (branch == null) continue;
                        var currentCumulativeCondition = string.Empty;
                        var ifEntry = CreateConditionEntry(conversation, branch.conditions.ifCondition, ref currentCumulativeCondition);
                        if (ifEntry != null)
                        {
                            ifEntry.Title = "if " + (arcweaveLookup[branch.conditions.ifCondition] as Condition).script;
                        }
                        if (branch.conditions.elseIfConditions != null)
                        {
                            var jArray = (Newtonsoft.Json.Linq.JArray)(branch.conditions.elseIfConditions);
                            foreach (var jToken in jArray)
                            {
                                var elseIfConditionGuid = (string)(jToken as Newtonsoft.Json.Linq.JValue).Value;
                                var elseIfEntry = CreateConditionEntry(conversation, elseIfConditionGuid, ref currentCumulativeCondition);
                                if (elseIfEntry != null)
                                {
                                    elseIfEntry.Title = "elseif " + (arcweaveLookup[elseIfConditionGuid] as Condition).script;
                                }
                            }
                        }
                        var elseEntry = CreateConditionEntry(conversation, branch.conditions.elseCondition, ref currentCumulativeCondition);
                        if (elseEntry != null)
                        {
                            elseEntry.Title = "else";
                        }
                    }

                    // Connect <START> node to starting element:
                    var startIndex = conversationInfo.startIndex;
                    if (!(0 <= startIndex && startIndex < board.elements.Count)) continue;
                    DialogueEntry firstRealEntry;
                    if (dialogueEntryLookup.TryGetValue(board.elements[startIndex], out firstRealEntry))
                    {
                        startEntry.outgoingLinks.Add(new Link(conversation.id, startEntry.id, conversation.id, firstRealEntry.id));
                    }

                    // Add Connections:
                    foreach (var connectionGuid in board.connections)
                    {
                        var connection = LookupArcweave<Connection>(connectionGuid);
                        if (connection == null) continue;
                        var entry = GetOrCreateDialogueEntry(conversation, connectionGuid);
                        entry.ActorID = currentPlayerID;
                        entry.ConversantID = currentNpcID;
                        ProcessContent(entry, connection.label);
                        if (string.IsNullOrEmpty(connection.label))
                        {
                            entry.ActorID = currentNpcID;
                            entry.isGroup = true;
                        }
                    }

                    //=======================================================================

                    // Connections are processed after adding all elements in all conversations.

                    // Add Condition links:
                    foreach (var branchGuid in board.branches)
                    {
                        var branch = LookupArcweave<Branch>(branchGuid);
                        if (branch == null) continue;
                        if (!string.IsNullOrEmpty(branch.conditions.ifCondition))
                        {
                            AddLink(branchGuid, branch.conditions.ifCondition);
                        }
                        if (branch.conditions.elseIfConditions != null)
                        {
                            var jArray = (Newtonsoft.Json.Linq.JArray)(branch.conditions.elseIfConditions);
                            foreach (var jToken in jArray)
                            {
                                var elseIfConditionGuid = (string)(jToken as Newtonsoft.Json.Linq.JValue).Value;
                                AddLink(branchGuid, elseIfConditionGuid);
                            }
                        }
                        if (!string.IsNullOrEmpty(branch.conditions.elseCondition))
                        {
                            AddLink(branchGuid, branch.conditions.elseCondition);
                        }
                    }
                }
            }

            // Handle connections here to handle cross-conversation links too:
            foreach (var conversationInfo in conversationInfo)
            {
                Board board;
                var boardGuid = conversationInfo.boardGuid;
                if (arcweaveProject.boards.TryGetValue(boardGuid, out board))
                {
                    // Add Connection links:
                    foreach (var connectionGuid in board.connections)
                    {
                        var connection = LookupArcweave<Connection>(connectionGuid);
                        if (connection == null) continue;
                        var a = AddLink(connection.sourceid, connectionGuid);
                        var b = AddLink(connectionGuid, connection.targetid);
                    }
                }
            }

            // Note: Jumpers are handled separately to handle cross-conversation jumps.

            SetElementOrderByOutputs();

            DeleteUnnecessaryConnectionEntries();
        }

        /// <summary>
        /// Extracts code tags and code (if present) from a connection label.
        /// It appears that a connection can only have label text or code, not both.
        /// </summary>
        /// <param name="label">Original label.</param>
        /// <param name="code">Extracted code.</param>
        /// <returns>Full label if no code tag, or empty string if extracted code.</returns>
        protected virtual string ExtractCode(string label, out string code)
        {
            code = string.Empty;
            if (string.IsNullOrEmpty(label) || !label.Contains("<code>")) return label;
            var codeOpenTagPos = label.IndexOf("<code>");
            var codeCloseTagPos = label.IndexOf("</code>");
            var codePos = codeOpenTagPos + "<code>".Length;
            code = label.Substring(codePos, codeCloseTagPos - codePos);
            return string.Empty;
        }

        protected virtual void SetElementOrderByOutputs()
        {
            foreach (var conversationInfo in conversationInfo)
            {
                Board board;
                var boardGuid = conversationInfo.boardGuid;
                if (arcweaveProject.boards.TryGetValue(boardGuid, out board))
                {
                    foreach (var elementGuid in board.elements)
                    {
                        var element = LookupArcweave<Element>(elementGuid);
                        var entry = dialogueEntryLookup[elementGuid];
                        if (element == null || entry == null) continue;
                        entry.outgoingLinks.Sort((x, y) => CompareOutputsPosition(x, y, element.outputs));
                    }
                }
            }
        }

        protected virtual void DeleteUnnecessaryConnectionEntries()
        {
            foreach (var conversation in database.conversations)
            {
                var deletionList = new List<DialogueEntry>();
                foreach (var entry in conversation.dialogueEntries)
                {
                    foreach (var link in entry.outgoingLinks)
                    {
                        if (link.destinationConversationID != entry.conversationID) continue;
                        var linkedEntry = conversation.GetDialogueEntry(link.destinationDialogueID);
                        if (linkedEntry == null) continue;
                        if (linkedEntry.isGroup && linkedEntry.outgoingLinks.Count > 0 && linkedEntry.Title == DeleteTag)
                        {
                            link.destinationDialogueID = linkedEntry.outgoingLinks[0].destinationDialogueID;
                            deletionList.Add(linkedEntry);
                        }
                    }
                }
                deletionList.ForEach(entryToDelete => conversation.dialogueEntries.Remove(entryToDelete));
            }
        }

        protected virtual int CompareOutputsPosition(Link x, Link y, List<string> outputs)
        {
            if (outputs == null) return 0;
            var destinationX = database.GetDialogueEntry(x);
            var destinationY = database.GetDialogueEntry(y);
            if (destinationX == null || destinationY == null) return 0;
            var guidX = dialogueEntryLookup.FirstOrDefault(e => e.Value == destinationX).Key;
            var guidY = dialogueEntryLookup.FirstOrDefault(e => e.Value == destinationY).Key;
            if (string.IsNullOrEmpty(guidX) || string.IsNullOrEmpty(guidY)) return 0;
            var indexX = outputs.IndexOf(guidX);
            var indexY = outputs.IndexOf(guidY);
            if (indexX == -1 || indexY == -1 || indexX == indexY) return 0;
            return (indexX < indexY) ? -1 : 1;
        }

        protected virtual string GetConversationTitle(Board conversationBoard)
        {
            string title;
            foreach (var boardNode in rootBoardNode.children)
            {
                if (TryGetConversationTitle(conversationBoard, boardNode, out title)) return title;
            }
            return conversationBoard.name;
        }

        protected virtual bool TryGetConversationTitle(Board conversationBoard, ArcweaveBoardNode boardNode, out string title)
        {
            if (boardNode.board == conversationBoard)
            {
                title = boardNode.board.name;
                return true;
            }
            else
            {
                foreach (var childBoardNode in boardNode.children)
                {
                    if (TryGetConversationTitle(conversationBoard, childBoardNode, out title))
                    {
                        title = boardNode.board.name + "/" + title;
                        return true;
                    }
                }
                title = string.Empty;
                return false;
            }
        }

        protected virtual int GetActorIDFromTitle(Element element, int currentNpcID)
        {
            if (element != null && element.title != null && element.title.Contains("Speaker:"))
            {
                var strippedTitle = Tools.StripRichTextCodes(element.title);
                var actorName = strippedTitle.Substring("Speaker:".Length).Trim();
                var actor = database.GetActor(actorName);
                if (actor != null) return actor.id;
            }
            return currentNpcID;
        }

        protected virtual void SetActorIDsFromComponents(DialogueEntry entry, Element element)
        {
            if (entry == null || element == null || element.components == null) return;
            if (element.components.Count >= 1)
            {
                var firstComponentGuid = element.components[0];
                Actor actor;
                if (actorLookup.TryGetValue(firstComponentGuid, out actor))
                {
                    entry.ActorID = actor.id;
                }
            }
            if (element.components.Count >= 2)
            {
                var firstComponentGuid = element.components[1];
                Actor actor;
                if (actorLookup.TryGetValue(firstComponentGuid, out actor))
                {
                    entry.ConversantID = actor.id;
                }
            }
        }

        protected virtual bool AddLink(string sourceGuid, string targetGuid)
        {
            DialogueEntry sourceEntry, targetEntry;
            if (!(dialogueEntryLookup.TryGetValue(sourceGuid, out sourceEntry) &&
                  dialogueEntryLookup.TryGetValue(targetGuid, out targetEntry))) return false;
            sourceEntry.outgoingLinks.Add(new Link(sourceEntry.conversationID, sourceEntry.id, targetEntry.conversationID, targetEntry.id));
            return true;
        }

        protected virtual DialogueEntry GetOrCreateDialogueEntry(Conversation conversation, string guid)
        {
            DialogueEntry entry;
            if (!dialogueEntryLookup.TryGetValue(guid, out entry))
            {
                entry = CreateAndInitDialogueEntry(conversation, string.Empty);
                dialogueEntryLookup[guid] = entry;
                if (importGuids)
                {
                    entry.fields.Add(new Field("Guid", guid, FieldType.Text));
                }
                if (importDialogueEntryAttributes)
                {
                    ArcweaveType arcweaveType;
                    if (arcweaveLookup.TryGetValue(guid, out arcweaveType))
                    {
                        var element = arcweaveType as Element;
                        if (element != null && element.attributes != null)
                        {
                            foreach (var attributeId in element.attributes)
                            {
                                var attribute = LookupArcweave<Attribute>(attributeId);
                                var templateField = template.dialogueEntryFields.Find(x => x.title == attribute.name);
                                var fieldType = (templateField != null) ? templateField.type : FieldType.Text;
                                Field.SetValue(entry.fields, attribute.name, attribute.value.data.ToString(), fieldType);
                            }
                        }
                    }
                }
            }
            return entry;
        }

        protected DialogueEntry CreateAndInitDialogueEntry(Conversation conversation, string title)
        {
            var entry = template.CreateDialogueEntry(template.GetNextDialogueEntryID(conversation), conversation.id, title);
            conversation.dialogueEntries.Add(entry);
            entry.DialogueText = string.Empty;
            entry.MenuText = string.Empty;
            entry.Sequence = string.Empty;
            return entry;
        }

        protected void CopyActors(DialogueEntry source, DialogueEntry destination)
        {
            if (source == null || destination == null) return;
            destination.ActorID = source.ActorID;
            destination.ConversantID = source.ConversantID;
        }

        protected virtual DialogueEntry CreateConditionEntry(Conversation conversation, string conditionGuid, ref string currentCumulativeCondition)
        {
            if (string.IsNullOrEmpty(conditionGuid)) return null;
            ArcweaveType value;
            if (!arcweaveLookup.TryGetValue(conditionGuid, out value)) return null;
            var condition = value as Condition;
            var entry = GetOrCreateDialogueEntry(conversation, conditionGuid);
            entry.ActorID = currentNpcID;
            entry.ConversantID = currentPlayerID;
            entry.isGroup = true;
            var doesThisNodeHaveCondition = !string.IsNullOrEmpty(condition.script);
            var havePreviousConditions = !string.IsNullOrEmpty(currentCumulativeCondition);
            var sanitizedConditionScript = doesThisNodeHaveCondition ? ConvertArcscriptToLua(condition.script) : string.Empty;
            if (doesThisNodeHaveCondition && (sanitizedConditionScript.Contains(" and ") || sanitizedConditionScript.Contains(" or ")))
            {
                sanitizedConditionScript = $"({sanitizedConditionScript})";
            }
            var completeConditions = sanitizedConditionScript;
            if (havePreviousConditions)
            {
                // We have previous conditions in this if..elseif..else block, so prepend them:
                if (!doesThisNodeHaveCondition)
                {
                    // This node doesn't have conditions, so just require that it's not the previous conditions:
                    completeConditions = $"not ({currentCumulativeCondition})";
                }
                else
                {
                    // This node has conditions, so require its conditions and that it's not the previous conditions:
                    if (currentCumulativeCondition.StartsWith("("))
                    {
                        completeConditions = $"(not {currentCumulativeCondition}) and {sanitizedConditionScript}";
                    }
                    else
                    {
                        completeConditions = $"(not ({currentCumulativeCondition})) and {sanitizedConditionScript}";
                    }
                }
            }
            if (doesThisNodeHaveCondition)
            {
                // This node has conditions, so add then to the cumulative conditions for the next node:
                if (!havePreviousConditions)
                {
                    // This is our first cumulative condition, so just set it:
                    currentCumulativeCondition = sanitizedConditionScript;
                }
                else
                {
                    // We have previous cumulative conditions, so add this one:
                    currentCumulativeCondition = $"{currentCumulativeCondition} or {sanitizedConditionScript}";
                }
            }
            entry.conditionsString = completeConditions;
            return entry;
        }

        protected virtual void ConnectJumpers()
        {
            // Handle jumpers here to handle cross-conversation links too:
            foreach (var conversationInfo in conversationInfo)
            {
                Board board;
                var boardGuid = conversationInfo.boardGuid;
                if (arcweaveProject.boards.TryGetValue(boardGuid, out board))
                {
                    foreach (var jumperGuid in board.jumpers)
                    {
                        var jumper = LookupArcweave<Jumper>(jumperGuid);
                        var jumperEntry = dialogueEntryLookup[jumperGuid];
                        if (jumper == null || jumper.elementId == null || jumperEntry == null) continue;
                        DialogueEntry destinationEntry;
                        if (!dialogueEntryLookup.TryGetValue(jumper.elementId, out destinationEntry)) continue;
                        if (destinationEntry == null) continue;
                        var destinationText = destinationEntry.Title;
                        if (string.IsNullOrEmpty(destinationText)) destinationText = destinationEntry.DialogueText;
                        jumperEntry.Title = string.IsNullOrEmpty(destinationText) ? "Jumper" : "Jumper: " + StripHtmlCodes(destinationText);
                        jumperEntry.outgoingLinks.Clear();
                        AddLink(jumperGuid, jumper.elementId);
                    }
                }
            }
        }

        protected virtual void AddQuests()
        {
            foreach (var questBoardGuid in questBoardGuids)
            {
                try
                {
                    var questInfo = LookupArcweave<Board>(questBoardGuid);
                    if (questInfo == null) continue;
                    var questID = template.GetNextQuestID(database);
                    var quest = template.CreateQuest(questID, questInfo.name);
                    int highestEntryNumber = 0;
                    foreach (var elementId in questInfo.elements)
                    {
                        var element = LookupArcweave<Element>(elementId);
                        var title = StripHtmlCodes(element.title);
                        if (string.Equals("Main", title, StringComparison.OrdinalIgnoreCase))
                        {
                            Field.SetValue(quest.fields, "Description", StripHtmlCodes(element.content));
                            foreach (var attributeId in element.attributes)
                            {
                                var attribute = LookupArcweave<Attribute>(attributeId);
                                switch (attribute.name)
                                {
                                    case "Trackable":
                                    case "Track":
                                        Field.SetValue(quest.fields, attribute.name, attribute.value.plain);
                                        break;
                                    case "State":
                                        Field.SetValue(quest.fields, attribute.name, attribute.value.data.ToString().ToLower());
                                        break;
                                    default:
                                        //case "Group":
                                        //case "Description":
                                        //case "Success Description":
                                        //case "Failure Description":
                                        Field.SetValue(quest.fields, attribute.name, attribute.value.data.ToString());
                                        break;
                                }
                            }
                        }
                        else if (title.StartsWith("Entry "))
                        {
                            int entryNumber;
                            if (int.TryParse(title.Substring("Entry ".Length), out entryNumber))
                            {
                                highestEntryNumber = Mathf.Max(highestEntryNumber, entryNumber);
                                Field.SetValue(quest.fields, title, StripHtmlCodes(element.content));
                                foreach (var attributeId in element.attributes)
                                {
                                    var attribute = LookupArcweave<Attribute>(attributeId);
                                    var value = attribute.value.data.ToString();
                                    if (attribute.name == "State") value = value.ToLower();
                                    Field.SetValue(quest.fields, attribute.name, value);
                                }
                            }
                        }
                    }
                    Field.SetValue(quest.fields, "Entry Count", highestEntryNumber);
                    database.items.Add(quest);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Failed to create quest for Arcweave GUID {questBoardGuid}. {e.Message}");
                }
            }
        }

        #endregion

        #region Touch Up Database

        public virtual void TouchUpDialogueDatabase(DialogueDatabase database)
        {
            SetStartCutscenesToNone(database);
            ConnectJumpers();
            ProcessCodeNodes();
            ExtractSequences();
            TouchUpRichText();
            SplitPipes();
        }

        protected virtual void SetStartCutscenesToNone(DialogueDatabase database)
        {
            foreach (var conversation in database.conversations)
            {
                SetConversationStartCutsceneToNone(conversation);
            }
        }

        protected virtual void SetConversationStartCutsceneToNone(Conversation conversation)
        {
            DialogueEntry entry = conversation.GetFirstDialogueEntry();
            if (entry == null)
            {
                Debug.LogWarning($"Dialogue System: Conversation '{conversation.Title}' doesn't have a START dialogue entry.");
            }
            else
            {
                if (string.IsNullOrEmpty(entry.currentSequence))
                {
                    Field.SetValue(entry.fields, "Sequence", "None()");
                }
            }
        }

        protected virtual string StripHtmlCodes(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            return Tools.StripRichTextCodes(text.Replace("\n", "").Replace("\r", "").Replace("<p>", "").Replace("</p>", ""));
        }

        protected virtual void TouchUpRichText()
        {
            foreach (var conversation in database.conversations)
            {
                foreach (var entry in conversation.dialogueEntries)
                {
                    entry.Title = TouchUpRichText(entry.Title);
                    entry.DialogueText = TouchUpRichText(entry.DialogueText);
                    if (!string.IsNullOrEmpty(entry.userScript)) entry.userScript = ConvertArcscriptToLua(entry.userScript, true);
                    if (!string.IsNullOrEmpty(entry.conditionsString)) entry.conditionsString = ConvertArcscriptToLua(entry.conditionsString);
                    if (!entry.isGroup && string.IsNullOrEmpty(entry.DialogueText) && string.IsNullOrEmpty(entry.Sequence))
                    {
                        entry.Sequence = (entry.id == 0) ? NoneSequence
                            : string.IsNullOrEmpty(entry.DialogueText) ? ContinueSequence : string.Empty;
                    }
                }
            }
        }

        protected virtual void SplitPipes()
        {
            foreach (var conversation in database.conversations)
            {
                conversation.SplitPipesIntoEntries(true, false, "Guid");
                foreach (var entry in conversation.dialogueEntries)
                {
                    entry.DialogueText = entry.DialogueText.Trim();
                }
            }
        }

        protected static Regex SequenceRegex = new Regex(@"\[SEQUENCE:[^\]]*\]");
        protected static Regex BlockRegex = new Regex(@"<p[^>]*>|</p>|<blockquote[^>]*>|</blockquote>|<span[^>]*>|</span>");
        protected static Regex CodeStartRegex = new Regex(@"<pre[^>]*><code>");
        protected static Regex CodeEndRegex = new Regex(@"</code></pre>");
        protected static Regex IdentifierRegex = new Regex(@"(?<![^\s+,(!*/-])\w+(?![^\s)+,*/-])");
        protected static Regex IncrementorRegex = new Regex(@"\+=|\-=");
        protected static Regex VisitsRegex = new Regex(@"visits\(<[^\)]+\)");
        protected static List<string> ReservedKeywords = new List<string>("if|elseif|else|endif|is|not|and|or|true|false|abs|sqr|sqrt|random|reset|resetAll|roll|show|visits".Split('|'));
        //protected static string[] CodeFieldPrefixes = new string[] { "_IF", "_ELSEIF", "_ELSE" };

        protected enum CodeState { None, InIf, InElseIf, InElse }

        protected virtual void ExtractSequences()
        {
            foreach (var conversation in database.conversations)
            {
                foreach (var entry in conversation.dialogueEntries)
                {
                    ExtractSequence(entry);
                }
            }
        }

        protected virtual void ExtractSequence(DialogueEntry entry)
        {
            var text = entry.DialogueText;
            if (string.IsNullOrEmpty(text)) return;
            var match = SequenceRegex.Match(text);
            if (match.Success)
            {
                if (!string.IsNullOrEmpty(entry.Sequence)) entry.Sequence += ";\n";
                entry.Sequence = text.Substring(match.Index + "[SEQUENCE:".Length, match.Length - "[SEQUENCE:]".Length).Trim();
                entry.DialogueText = text.Remove(match.Index, match.Length);
            }
        }

        protected virtual string TouchUpRichText(string s)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;

            // Replace emphasis tags:
            if (s.Contains("<em>"))
            {
                s = s.Replace("<em>", "<i>").Replace("</em>", "</i>");
            }

            var matches = BlockRegex.Matches(s);

            // Insert newlines for blocks containing <p>..</p>:
            foreach (var match in matches.Cast<Match>().Reverse())
            {
                if (match.Value.StartsWith("<p>") || match.Value.StartsWith("</p>"))
                {
                    s = s.Insert(match.Index, "\n");
                }
            }

            s = BlockRegex.Replace(s, string.Empty);
            return Tools.RemoveHtml(s).Trim();
        }

        protected enum ContentPieceType { Text, Code }
        protected class ContentPiece
        {
            public ContentPieceType type;
            public string text;
            public ContentPiece(ContentPieceType type, string text)
            {
                this.type = type;
                this.text = text;
            }
        }

        protected virtual void ProcessContent(DialogueEntry entry, string content)
        {
            // Put conditional code in temporary fields. They will be processed in the
            // touch-up phase as new child nodes. 
            // Put code outside of conditional blocks in the userScript variable.
            if (string.IsNullOrEmpty(content))
            {
                entry.DialogueText = string.Empty;
            }
            else if (!content.Contains("<code"))
            {
                entry.DialogueText = TouchUpRichText(content);
            }
            else
            {
                entry.DialogueText = content;
                //// Split the content into pieces: text, code, text, code, text, etc.
                //string s = content;
                //var pieces = new List<ContentPiece>();
                //var match = CodeStartRegex.Match(s);
                //while (match.Success)
                //{
                //    // Create a piece for the text before the begin code tag:
                //    var text = TouchUpRichText(s.Substring(0, match.Index));
                //    if (!string.IsNullOrEmpty(text.Trim()))
                //    {
                //        pieces.Add(new ContentPiece(ContentPieceType.Text, text));
                //    }

                //    // Strip the text and begin code tag:
                //    s = s.Substring(match.Index + match.Length);

                //    // Locate the end code tag:
                //    match = CodeEndRegex.Match(s);
                //    if (match.Success)
                //    {
                //        // Create a piece for the code inside the code tags:
                //        var code = s.Substring(0, match.Index);
                //        pieces.Add(new ContentPiece(ContentPieceType.Code, code));

                //        // Strip the code and end code tag:
                //        s = s.Substring(match.Index + match.Length);
                //    }

                //    // Look for the next begin code tag:
                //    match = CodeStartRegex.Match(s);
                //}

                //// Create a piece for the text after all of the code tags:
                //s = TouchUpRichText(s);
                //if (!string.IsNullOrEmpty(s.Trim()))
                //{
                //    pieces.Add(new ContentPiece(ContentPieceType.Text, s));
                //}

                //// Process pieces:
                //entry.DialogueText = string.Empty;
                //var hasIfStatement = false;
                //var codeState = CodeState.None;
                //var codeFieldPrefix = string.Empty;
                //int numElseIf = 0;
                //foreach (ContentPiece piece in pieces)
                //{
                //    switch (piece.type)
                //    {
                //        case ContentPieceType.Text:
                //            switch (codeState)
                //            {
                //                case CodeState.None:
                //                    if (hasIfStatement)
                //                    {
                //                        entry.fields.Add(new Field($"_POST_IF_TEXT", piece.text, FieldType.Text));
                //                    }
                //                    else
                //                    {
                //                        entry.DialogueText += piece.text;
                //                    }
                //                    break;
                //                case CodeState.InIf:
                //                case CodeState.InElseIf:
                //                case CodeState.InElse:
                //                    entry.fields.Add(new Field($"{codeFieldPrefix}_TEXT", piece.text, FieldType.Text));
                //                    break;
                //            }
                //            break;
                //        case ContentPieceType.Code:
                //            var code = piece.text;
                //            if (code.StartsWith("if "))
                //            {
                //                hasIfStatement = true;
                //                codeState = CodeState.InIf;
                //                codeFieldPrefix = "_IF";
                //                code = code.Substring("if ".Length);
                //            }
                //            else if (code.StartsWith("elseif "))
                //            {
                //                codeState = CodeState.InElseIf;
                //                codeFieldPrefix = $"_ELSEIF.{numElseIf}";
                //                code = code.Substring("elseif ".Length);
                //                numElseIf++;
                //            }
                //            else if (code.StartsWith("else"))
                //            {
                //                codeState = CodeState.InElse;
                //                codeFieldPrefix = "_ELSE";
                //                code = string.Empty;
                //            }
                //            else if (code.StartsWith("endif"))
                //            {
                //                codeState = CodeState.None;
                //                codeFieldPrefix = string.Empty;
                //            }
                //            else
                //            {
                //                if (codeState == CodeState.None)
                //                {
                //                    if (!string.IsNullOrEmpty(entry.userScript)) entry.userScript += "\n";
                //                    entry.userScript += code;
                //                }
                //                else
                //                {
                //                    entry.fields.Add(new Field($"{codeFieldPrefix}_INNER_CODE", code, FieldType.Text));
                //                }
                //            }
                //            if (!string.IsNullOrEmpty(codeFieldPrefix))
                //            {
                //                entry.fields.Add(new Field($"{codeFieldPrefix}_CODE", code, FieldType.Text));
                //            }
                //            break;
                //    }
                //}
                //entry.fields.Add(new Field("_NUM_ELSEIF", numElseIf.ToString(), FieldType.Number));
            }
        }

        protected virtual void ProcessCodeNodes()
        {
            foreach (var conversation in database.conversations)
            {
                int numEntries = conversation.dialogueEntries.Count;
                for (int i = numEntries - 1; i >= 0; i--)
                {
                    var entry = conversation.dialogueEntries[i];
                    ProcessCodeInNode(conversation, entry);
                }
            }
        }

        protected enum NodeTokenType { Text, If, Elseif, Else, Endif, Code }
        protected class NodeToken
        {
            public NodeTokenType type;
            public string content;
            public NodeToken(NodeTokenType type, string content)
            {
                this.type = type;
                this.content = content;
            }
        }

        protected virtual void ProcessCodeInNode(Conversation conversation, DialogueEntry entry)
        {
            if (entry.DialogueText == null) entry.DialogueText = string.Empty;
            if (!entry.DialogueText.Contains("<code")) return;
            var tokens = TokenizeCodeNode(entry);
            var originalOutgoingLinks = PrepareNodeForTokenConversion(entry);
            ConvertTokensToNodes(conversation, entry, tokens);
            AddOriginalOutgoingLinksToLeafNodes(entry, originalOutgoingLinks, new List<DialogueEntry>());
        }

        private List<Link> PrepareNodeForTokenConversion(DialogueEntry entry)
        {
            entry.DialogueText = string.Empty;
            var originalLinks = entry.outgoingLinks;
            entry.outgoingLinks = new List<Link>();
            return originalLinks;
        }

        protected virtual Queue<NodeToken> TokenizeCodeNode(DialogueEntry entry)
        {
            var tokens = new Queue<NodeToken>();
            var s = entry.DialogueText.Replace("<pre>", "").Replace("</pre>", "");
            int safeguard = 0;
            while (!string.IsNullOrEmpty(s) && safeguard++ < 100)
            {
                var codePos = s.IndexOf("<code");
                if (codePos == -1)
                {
                    tokens.Enqueue(new NodeToken(NodeTokenType.Text, s));
                    s = string.Empty;
                }
                else
                {
                    if (codePos > 0)
                    {
                        tokens.Enqueue(new NodeToken(NodeTokenType.Text, s.Substring(0, codePos)));
                        s = s.Substring(codePos);
                    }
                    var codeTagEndPos = s.IndexOf(">");
                    if (codeTagEndPos == -1 || codeTagEndPos > s.Length - 1) break;
                    s = s.Substring(codeTagEndPos + 1);
                    var endCodePos = s.IndexOf("</code>");
                    if (endCodePos == -1) endCodePos = s.Length - 1;
                    var content = s.Substring(0, endCodePos);
                    s = s.Substring(endCodePos + "</code>".Length);
                    NodeTokenType type = NodeTokenType.Code;
                    if (content.StartsWith("if "))
                    {
                        type = NodeTokenType.If;
                        content = content.Substring("if ".Length);
                    }
                    else if (content.StartsWith("elseif "))
                    {
                        type = NodeTokenType.Elseif;
                        content = content.Substring("elseif ".Length);
                    }
                    else if (content == "else")
                    {
                        type = NodeTokenType.Else;
                        content = string.Empty;
                    }
                    else if (content == "endif")
                    {
                        type = NodeTokenType.Endif;
                        content = string.Empty;
                    }
                    tokens.Enqueue(new NodeToken(type, content));
                }
            }
            return tokens;
        }

        protected virtual void ConvertTokensToNodes(Conversation conversation, DialogueEntry entry, 
            Queue<NodeToken> tokens)
        {
            while (tokens.Count > 0)
            {
                var nextTokenType = tokens.Peek().type;
                if (nextTokenType == NodeTokenType.Elseif || 
                    nextTokenType == NodeTokenType.Else ||
                    nextTokenType == NodeTokenType.Endif)
                {
                    // We need to return control to the parent node to handle it.
                    return;
                }
                var token = tokens.Dequeue();
                switch (token.type)
                {
                    case NodeTokenType.Text:
                        AddContentToLeafNodes(entry, token.content, null, new List<DialogueEntry>());
                        break;
                    case NodeTokenType.Code:
                        AddContentToLeafNodes(entry, null, token.content, new List<DialogueEntry>());
                        break;
                    case NodeTokenType.If:
                        HandleIfStatement(conversation, entry, token.content, tokens);
                        break;
                    default:
                        Debug.LogError($"ArcweaveImporter: Internal error processing {token.type} token");
                        break;
                }
            }
        }

        protected void HandleIfStatement(Conversation conversation, DialogueEntry entry, 
            string condition, Queue<NodeToken> tokens)
        {
            var child = CreateAndInitDialogueEntry(conversation, $"if {condition}");
            CopyActors(entry, child);
            entry.outgoingLinks.Add(new Link(conversation.id, entry.id, conversation.id, child.id));
            child.conditionsString = ConvertArcscriptToLua(condition);
            while (tokens.Count > 0)
            {
                ConvertTokensToNodes(conversation, child, tokens);
                if (tokens.Count == 0) return;
                var token = tokens.Dequeue();
                switch (token.type)
                {
                    case NodeTokenType.Elseif:
                        HandleElseifStatement(conversation, entry, token.content, tokens);
                        break;
                    case NodeTokenType.Else:
                        HandleElseStatement(conversation, entry, tokens);
                        break;
                    case NodeTokenType.Endif:
                        HandleEndifStatement(conversation, entry);
                        return;
                    default:
                        Debug.LogError($"ArcweaveImporter: Internal error processing {token.type} token in 'if' statement.");
                        break;
                }
            }
        }

        protected void HandleElseifStatement(Conversation conversation, DialogueEntry entry, 
            string condition, Queue<NodeToken> tokens)
        {
            var child = CreateAndInitDialogueEntry(conversation, $"elseif {condition}");
            CopyActors(entry, child);
            entry.outgoingLinks.Add(new Link(conversation.id, entry.id, conversation.id, child.id));
            child.conditionsString = ConvertArcscriptToLua(condition);
            ConvertTokensToNodes(conversation, child, tokens);
        }

        protected void HandleElseStatement(Conversation conversation, DialogueEntry entry,
            Queue<NodeToken> tokens)
        {
            var child = CreateAndInitDialogueEntry(conversation, $"else");
            CopyActors(entry, child);
            entry.outgoingLinks.Add(new Link(conversation.id, entry.id, conversation.id, child.id));
            ConvertTokensToNodes(conversation, child, tokens);
        }

        protected void HandleEndifStatement(Conversation conversation, DialogueEntry entry)
        {
            // Check if we need to add an else node:
            var lastChild = conversation.GetDialogueEntry(entry.outgoingLinks[entry.outgoingLinks.Count - 1].destinationDialogueID);
            if (lastChild.Title != "else")
            {
                var child = CreateAndInitDialogueEntry(conversation, $"(else)");
                CopyActors(entry, child);
                entry.outgoingLinks.Add(new Link(conversation.id, entry.id, conversation.id, child.id));
            }

            // Make conditions mutually exclusive:
            var newConditions = new List<string>();
            var allConditions = string.Empty;
            // Make updated list of conditions: node's conditions and not otherConditions.
            for (int i = 0; i < entry.outgoingLinks.Count - 1; i++)
            {
                var conditionNode = conversation.GetDialogueEntry(entry.outgoingLinks[i].destinationDialogueID);
                if (!string.IsNullOrEmpty(allConditions)) allConditions += " or ";
                allConditions += conditionNode.conditionsString;
                string otherConditions = string.Empty;
                for (int j = 0; j < entry.outgoingLinks.Count - 1; j++)
                {
                    if (i == j) continue;
                    var otherNode = conversation.GetDialogueEntry(entry.outgoingLinks[j].destinationDialogueID);
                    if (!string.IsNullOrEmpty(otherConditions)) otherConditions += " or ";
                    otherConditions += otherNode.conditionsString;
                }
                if (string.IsNullOrEmpty(otherConditions))
                {
                    newConditions.Add(conditionNode.conditionsString);
                }   
                else
                {
                    newConditions.Add($"{conditionNode.conditionsString} and not ({otherConditions})");
                }
            }
            // Assign updated list of conditions to nodes:
            for (int i = 0; i < entry.outgoingLinks.Count - 1; i++)
            {
                var conditionNode = conversation.GetDialogueEntry(entry.outgoingLinks[i].destinationDialogueID);
                conditionNode.conditionsString = newConditions[i];
            }
            // Assign else node: not allConditions.
            var elseNode = conversation.GetDialogueEntry(entry.outgoingLinks[entry.outgoingLinks.Count - 1].destinationDialogueID);
            elseNode.conditionsString = $"not ({allConditions})";
        }

        protected void AddContentToLeafNodes(DialogueEntry entry, string text, string code, 
            List<DialogueEntry> processed)
        {
            if (entry == null || processed.Contains(entry)) return;
            processed.Add(entry);
            var isLeaf = entry.outgoingLinks.Count == 0;
            if (isLeaf)
            {
                if (!string.IsNullOrEmpty(text))
                {
                    entry.DialogueText = AppendWithNewline(entry.DialogueText, text);
                }
                if (!string.IsNullOrEmpty(code))
                {
                    entry.userScript = AppendWithNewline(entry.userScript, ConvertArcscriptToLua(code, true));
                }
            }
            else
            {
                foreach (var link in entry.outgoingLinks)
                {
                    var child = database.GetDialogueEntry(link);
                    AddContentToLeafNodes(child, text, code, processed);
                }
            }
        }

        protected string AppendWithNewline(string original, string toAdd)
        {
            if (string.IsNullOrEmpty(original) || original.EndsWith("\n"))
            {
                return original + toAdd;
            }
            else
            {
                return original + "\n" + toAdd;
            }
        }

        protected void AddOriginalOutgoingLinksToLeafNodes(DialogueEntry entry,
            List<Link> originalOutgoingLinks, List<DialogueEntry> processed)
        {
            if (entry == null || processed.Contains(entry)) return;
            processed.Add(entry);
            var isLeaf = entry.outgoingLinks.Count == 0;
            if (isLeaf)
            {
                AddOutgoingLinksToNode(entry, originalOutgoingLinks);
            }
            else
            {
                foreach (var link in entry.outgoingLinks)
                {
                    var child = database.GetDialogueEntry(link);
                    AddOriginalOutgoingLinksToLeafNodes(child, originalOutgoingLinks, processed);
                }
            }
        }

        private void AddOutgoingLinksToNode(DialogueEntry entry, List<Link> outgoingLinks)
        {
            foreach (var link in outgoingLinks)
            {
                entry.outgoingLinks.Add(new Link(link));
            }
        }

        // Returns code entry.
        protected virtual DialogueEntry InsertCodeEntry(Conversation conversation, DialogueEntry entry, DialogueEntry postIfEntry, string prefix, ref string cumulativeConditions)
        {
            var codeField = Field.Lookup(entry.fields, prefix + "_CODE");
            if (codeField == null && !prefix.EndsWith("else")) return null;
            var textField = Field.Lookup(entry.fields, prefix + "_TEXT");
            var innerCodeField = Field.Lookup(entry.fields, prefix + "_INNER_CODE");
            entry.fields.Remove(codeField);
            entry.fields.Remove(textField);
            entry.fields.Remove(innerCodeField);
            var codeEntry = CreateAndInitDialogueEntry(conversation, string.Empty);
            CopyActors(entry, codeEntry);
            codeEntry.Title = (codeField == null) ? "else" : codeField.value;
            codeEntry.outgoingLinks.Add(new Link(conversation.id, codeEntry.id, conversation.id, postIfEntry.id));
            codeEntry.isGroup = textField == null || string.IsNullOrEmpty(textField.value);
            codeEntry.DialogueText = (textField != null) ? TouchUpRichText(textField.value) : string.Empty;
            codeEntry.Sequence = (codeEntry.isGroup || !string.IsNullOrEmpty(codeEntry.DialogueText)) ? string.Empty : ContinueSequence;
            codeEntry.conditionsString = (codeField != null) ? ConvertArcscriptToLua(codeField.value) : string.Empty;
            if (string.IsNullOrEmpty(cumulativeConditions))
            {
                cumulativeConditions = codeEntry.conditionsString;
            }
            else
            {
                if (cumulativeConditions[0] != '(') cumulativeConditions = $"({cumulativeConditions})";
                if (!string.IsNullOrEmpty(codeEntry.conditionsString))
                {
                    cumulativeConditions += $" and ({codeEntry.conditionsString})";
                }
            }
            if (innerCodeField != null) codeEntry.userScript = ConvertArcscriptToLua(innerCodeField.value, true);
            entry.outgoingLinks.Add(new Link(conversation.id, entry.id, conversation.id, codeEntry.id));
            return codeEntry;
        }

        protected virtual void InsertCodeFallthroughEntry(Conversation conversation, DialogueEntry entry, DialogueEntry postIfEntry, string cumulativePreviousConditions)
        {
            var codeEntry = CreateAndInitDialogueEntry(conversation, string.Empty);
            CopyActors(entry, codeEntry);
            codeEntry.Title = "fallthrough";
            codeEntry.outgoingLinks.Add(new Link(conversation.id, codeEntry.id, conversation.id, postIfEntry.id));
            codeEntry.isGroup = true;
            codeEntry.DialogueText = string.Empty;
            codeEntry.Sequence = (codeEntry.isGroup || !string.IsNullOrEmpty(codeEntry.DialogueText)) ? string.Empty : ContinueSequence;
            codeEntry.conditionsString = $"not ({cumulativePreviousConditions})";
            entry.outgoingLinks.Add(new Link(conversation.id, entry.id, conversation.id, codeEntry.id));
        }

        protected virtual string ConvertArcscriptToLua(string code, bool convertIncrementors = false)
        {
            if (string.IsNullOrEmpty(code)) return code;
            code = Tools.RemoveHtml(code);
            code = ConvertVisits(code);
            if (convertIncrementors) code = ConvertIncrementors(code);
            code = ConvertArcscriptVariablesToLua(code);
            code = code.Replace("!=", "~=")
                .Replace("is not", "~=")
                .Replace("!", "not ")
                .Replace("&&", "and")
                .Replace("||", "or");
            return code;
        }

        // Convert visits() function calls.
        protected virtual string ConvertVisits(string code)
        {
            if (!code.Contains("visits(")) return code; ;

            // Convert visits(...data-id="GUID"...) to visits("GUID")
            var matches = VisitsRegex.Matches(code);
            foreach (var match in matches.Cast<Match>().Reverse())
            {
                var visitsCall = code.Substring(match.Index, match.Length);
                var dataIdPos = visitsCall.IndexOf("data-id") + match.Index;
                var guid = code.Substring(dataIdPos + "data-id=\"".Length + 1);
                guid = guid.Substring(0, guid.IndexOf("\""));
                code = Replace(code, match.Index, match.Length, $"visits(\"{guid}\")");
            }

            return code.Replace("visits()", "visits(\"\")");
        }

        protected virtual string ConvertIncrementors(string code)
        {
            // Convert "x +=" to "x = x +" and same for "-=":
            var matches = IncrementorRegex.Matches(code);
            foreach (var match in matches.Cast<Match>().Reverse())
            {
                // Get '+' or '-' of '+=' or '-=':
                var mathOp = match.Value.Substring(0, 1);

                // Get the variable name prior to the incrementor:
                var left = code.Substring(0, match.Index).TrimEnd();
                var variableStartPos = Mathf.Max(0, Mathf.Max(left.LastIndexOf('\n'), left.LastIndexOf(';')));
                var variableName = code.Substring(variableStartPos, match.Index - variableStartPos).Trim();

                // Replace "+= " with "= x +":
                code = Replace(code, match.Index, 2, $"= {variableName} {mathOp}");
            }
            return code;
        }

        protected virtual string ConvertArcscriptVariablesToLua(string code)
        {
            var matches = IdentifierRegex.Matches(code);
            foreach (var match in matches.Cast<Match>().Reverse())
            {
                var identifier = match.Value;
                if (ReservedKeywords.Contains(identifier)) continue;
                if (database.variables.Find(x => x.Name == identifier) == null) continue;
                var luaVariable = $"Variable[\"{identifier}\"]";
                if (numPlayers > 1)
                {
                    //--- Blue Goo Games contribution to support multiple actors:
                    // If identifier begins with global, set luaVariable = $"Variable[\"{identifier}\"]";	
                    // Else set luaVariable = $Variable[Variable[\"ActorIndex\"] .. \"_{identifier}\"]	
                    // Create string array of words that are considered as global variables	
                    bool isGlobalVariable = identifier.StartsWith("global") || globalVariables.Contains(identifier);
                    luaVariable = isGlobalVariable ? $"Variable[\"{identifier}\"]" : $"Variable[Variable[\"ActorIndex\"] .. \"_{identifier}\"]";
                }
                code = Replace(code, match.Index, match.Length, luaVariable);
            }
            return code;
        }

        protected virtual string Replace(string s, int index, int length, string replacement)
        {
            var builder = new System.Text.StringBuilder();
            builder.Append(s.Substring(0, index));
            builder.Append(replacement);
            builder.Append(s.Substring(index + length));
            return builder.ToString();
        }

        #endregion

    }
}

#endif

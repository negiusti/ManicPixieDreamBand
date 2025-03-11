// Copyright (c) 2024 Augie R. Maddox, Guavaman Enterprises. All rights reserved.

#if UNITY_4_6 || UNITY_4_7 || UNITY_5 || UNITY_2017 || UNITY_2018 || UNITY_2019 || UNITY_2020 || UNITY_2021 || UNITY_2022 || UNITY_2023 || UNITY_6000 || UNITY_6000_0_OR_NEWER
#define UNITY_4_6_PLUS
#endif

#pragma warning disable 0649

namespace Rewired.Data {
    using UnityEngine;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using Rewired;
    using Rewired.Utils.Libraries.TinyJson;

    /// <summary>
    /// Abstract class for a key value data store.
    /// </summary>
    public abstract class UserDataStore_KeyValue : UserDataStore {

        private readonly static string thisScriptName = typeof(UserDataStore_KeyValue).Name;
        private const string logPrefix = "Rewired: ";
#if UNITY_EDITOR
        private readonly static string editorLoadedMessage = "\n***IMPORTANT:*** Changes made to the Rewired Input Manager configuration after the last time data was saved WILL NOT be used because the loaded old saved data has overwritten these values. If you change something in the Rewired Input Manager such as a Joystick Map or Input Behavior settings, you will not see these changes reflected in the current configuration. Clear save data using the inspector option on the " + thisScriptName + " component.";
#endif
        private const string key_controllerAssignments = "ControllerAssignments";

        private const int controllerMapKeyVersion = 0;

#if UNITY_4_6_PLUS
        [Tooltip("Should this script be used? If disabled, nothing will be saved or loaded.")]
#endif
        [UnityEngine.SerializeField]
        private bool _isEnabled = true;

#if UNITY_4_6_PLUS
        [Tooltip("Should saved data be loaded on start?")]
#endif
        [UnityEngine.SerializeField]
        private bool _loadDataOnStart = true;

#if UNITY_4_6_PLUS
        [Tooltip("Should Player Joystick assignments be saved and loaded? This is not totally reliable for all Joysticks on all platforms. " +
            "Some platforms/input sources do not provide enough information to reliably save assignments from session to session " +
            "and reboot to reboot.")]
#endif
        [UnityEngine.SerializeField]
        private bool _loadJoystickAssignments = true;

#if UNITY_4_6_PLUS
        [Tooltip("Should Player Keyboard assignments be saved and loaded?")]
#endif
        [UnityEngine.SerializeField]
        private bool _loadKeyboardAssignments = true;

#if UNITY_4_6_PLUS
        [Tooltip("Should Player Mouse assignments be saved and loaded?")]
#endif
        [UnityEngine.SerializeField]
        private bool _loadMouseAssignments = true;

        /// <summary>
        /// Should this script be used? If disabled, nothing will be saved or loaded.
        /// </summary>
        public bool isEnabled { get { return _isEnabled; } set { _isEnabled = value; } }
        /// <summary>
        /// Should saved data be loaded on start?
        /// </summary>
        public bool loadDataOnStart { get { return _loadDataOnStart; } set { _loadDataOnStart = value; } }
        /// <summary>
        /// Should Player Joystick assignments be saved and loaded? This is not totally reliable for all Joysticks on all platforms.
        /// Some platforms/input sources do not provide enough information to reliably save assignments from session to session
        /// and reboot to reboot.
        /// </summary>
        public bool loadJoystickAssignments { get { return _loadJoystickAssignments; } set { _loadJoystickAssignments = value; } }
        /// <summary>
        /// Should Player Keyboard assignments be saved and loaded?
        /// </summary>
        public bool loadKeyboardAssignments { get { return _loadKeyboardAssignments; } set { _loadKeyboardAssignments = value; } }
        /// <summary>
        /// Should Player Mouse assignments be saved and loaded?
        /// </summary>
        public bool loadMouseAssignments { get { return _loadMouseAssignments; } set { _loadMouseAssignments = value; } }
        /// <summary>
        /// The data store.
        /// </summary>
        protected abstract IDataStore dataStore { get; }

        private bool loadControllerAssignments { get { return _loadKeyboardAssignments || _loadMouseAssignments || _loadJoystickAssignments; } }

        private List<int> allActionIds {
            get {
                if (__allActionIds != null) return __allActionIds; // use the cached version
                List<int> ids = new List<int>();
                IList<InputAction> actions = ReInput.mapping.Actions;
                for (int i = 0; i < actions.Count; i++) {
                    ids.Add(actions[i].id);
                }
                __allActionIds = ids;
                return ids;
            }
        }

        private string allActionIdsString {
            get {
                if (!string.IsNullOrEmpty(__allActionIdsString)) return __allActionIdsString; // use the cached version
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                List<int> ids = allActionIds;
                for (int i = 0; i < ids.Count; i++) {
                    if (i > 0) sb.Append(",");
                    sb.Append(ids[i]);
                }
                __allActionIdsString = sb.ToString();
                return __allActionIdsString;
            }
        }

        [NonSerialized]
        private bool _allowImpreciseJoystickAssignmentMatching = true;
        [NonSerialized]
        private bool _deferredJoystickAssignmentLoadPending;
        [NonSerialized]
        private bool _wasJoystickEverDetected;
        [NonSerialized]
        private List<int> __allActionIds;
        [NonSerialized]
        private string __allActionIdsString;
        [NonSerialized]
        private readonly StringBuilder _sb = new StringBuilder();

        #region UserDataStore Implementation

        // Public Methods

        /// <summary>
        /// Save all data now.
        /// </summary>
        public override void Save() {
            if (!_isEnabled) {
                Debug.LogWarning(logPrefix + thisScriptName + " is disabled and will not save any data.", this);
                return;
            }
            SaveAll();

#if UNITY_EDITOR
            Debug.Log(logPrefix + thisScriptName + " saved all user data.");
#endif
        }

        /// <summary>
        /// Save all data for a specific controller for a Player.
        /// </summary>
        /// <param name="playerId">Player id</param>
        /// <param name="controllerType">Controller type</param>
        /// <param name="controllerId">Controller id</param>
        public override void SaveControllerData(int playerId, ControllerType controllerType, int controllerId) {
            if (!_isEnabled) {
                Debug.LogWarning(logPrefix + thisScriptName + " is disabled and will not save any data.", this);
                return;
            }
            SaveControllerDataNow(playerId, controllerType, controllerId);

#if UNITY_EDITOR
            Debug.Log(logPrefix + thisScriptName + " saved " + controllerType + " " + controllerId + " data for Player " + playerId + ".");
#endif
        }

        /// <summary>
        /// Save all data for a specific controller. Does not save Player data.
        /// </summary>
        /// <param name="controllerType">Controller type</param>
        /// <param name="controllerId">Controller id</param>
        public override void SaveControllerData(ControllerType controllerType, int controllerId) {
            if (!_isEnabled) {
                Debug.LogWarning(logPrefix + thisScriptName + " is disabled and will not save any data.", this);
                return;
            }
            SaveControllerDataNow(controllerType, controllerId);

#if UNITY_EDITOR
            Debug.Log(logPrefix + thisScriptName + " saved " + controllerType + " " + controllerId + " data.");
#endif
        }

        /// <summary>
        /// Save all data for a specific Player.
        /// </summary>
        /// <param name="playerId">Player id</param>
        public override void SavePlayerData(int playerId) {
            if (!_isEnabled) {
                Debug.LogWarning(logPrefix + thisScriptName + " is disabled and will not save any data.", this);
                return;
            }
            SavePlayerDataNow(playerId);

#if UNITY_EDITOR
            Debug.Log(logPrefix + thisScriptName + " saved all user data for Player " + playerId + " to.");
#endif
        }

        /// <summary>
        /// Save all data for a specific InputBehavior for a Player.
        /// </summary>
        /// <param name="playerId">Player id</param>
        /// <param name="behaviorId">Input Behavior id</param>
        public override void SaveInputBehavior(int playerId, int behaviorId) {
            if (!_isEnabled) {
                Debug.LogWarning(logPrefix + thisScriptName + " is disabled and will not save any data.", this);
                return;
            }
            SaveInputBehaviorNow(playerId, behaviorId);

#if UNITY_EDITOR
            Debug.Log(logPrefix + thisScriptName + " saved Input Behavior data for Player " + playerId + ".");
#endif
        }

        /// <summary>
        /// Load all data now.
        /// </summary>
        public override void Load() {
            if (!_isEnabled) {
                Debug.LogWarning(logPrefix + thisScriptName + " is disabled and will not load any data.", this);
                return;
            }
            int count = LoadAll();

#if UNITY_EDITOR
            if(count > 0) Debug.LogWarning(logPrefix + thisScriptName + " loaded all user data. " + editorLoadedMessage);
#endif
        }

        /// <summary>
        /// Load all data for a specific controller for a Player.
        /// </summary>
        /// <param name="playerId">Player id</param>
        /// <param name="controllerType">Controller type</param>
        /// <param name="controllerId">Controller id</param>
        public override void LoadControllerData(int playerId, ControllerType controllerType, int controllerId) {
            if (!_isEnabled) {
                Debug.LogWarning(logPrefix + thisScriptName + " is disabled and will not load any data.", this);
                return;
            }
            int count = LoadControllerDataNow(playerId, controllerType, controllerId);

#if UNITY_EDITOR
            if(count > 0) Debug.LogWarning(logPrefix + thisScriptName + " loaded user data for " + controllerType + " " + controllerId + " for Player " + playerId + ". " + editorLoadedMessage);
#endif
        }

        /// <summary>
        /// Load all data for a specific controller. Does not load Player data.
        /// </summary>
        /// <param name="controllerType">Controller type</param>
        /// <param name="controllerId">Controller id</param>
        public override void LoadControllerData(ControllerType controllerType, int controllerId) {
            if (!_isEnabled) {
                Debug.LogWarning(logPrefix + thisScriptName + " is disabled and will not load any data.", this);
                return;
            }
            int count = LoadControllerDataNow(controllerType, controllerId);

#if UNITY_EDITOR
            if(count > 0) Debug.LogWarning(logPrefix + thisScriptName + " loaded user data for " + controllerType + " " + controllerId + ". " + editorLoadedMessage);
#endif
        }

        /// <summary>
        /// Load all data for a specific Player.
        /// </summary>
        /// <param name="playerId">Player id</param>
        public override void LoadPlayerData(int playerId) {
            if (!_isEnabled) {
                Debug.LogWarning(logPrefix + thisScriptName + " is disabled and will not load any data.", this);
                return;
            }
            int count = LoadPlayerDataNow(playerId);

#if UNITY_EDITOR
            if(count > 0) Debug.LogWarning(logPrefix + thisScriptName + " loaded Player + " + playerId + " user data. " + editorLoadedMessage);
#endif
        }

        /// <summary>
        /// Load all data for a specific InputBehavior for a Player.
        /// </summary>
        /// <param name="playerId">Player id</param>
        /// <param name="behaviorId">Input Behavior id</param>
        public override void LoadInputBehavior(int playerId, int behaviorId) {
            if (!_isEnabled) {
                Debug.LogWarning(logPrefix + thisScriptName + " is disabled and will not load any data.", this);
                return;
            }
            int count = LoadInputBehaviorNow(playerId, behaviorId);

#if UNITY_EDITOR
            if(count > 0) Debug.LogWarning(logPrefix + thisScriptName + " loaded Player + " + playerId + " InputBehavior data. " + editorLoadedMessage);
#endif
        }

        // Event Handlers

        /// <summary>
        /// Called when SaveDataStore is initialized.
        /// </summary>
        protected override void OnInitialize() {

            // Disallow imprecise joystick assignment matching on some platforms when
            // system id/player Rewired Player alignment needs to stay fixed.
#if !UNITY_EDITOR && (UNITY_XBOXONE || UNITY_PS4 || UNITY_PS5 || UNITY_SWITCH)
            _allowImpreciseJoystickAssignmentMatching = false;
#endif

            if (_loadDataOnStart) {
                Load();

                // Save the controller assignments immediately only if there were joysticks connected on start
                // so the initial auto-assigned joystick assignments will be saved without any user intervention.
                // This will not save over controller assignment data if no joysticks were attached initially.
                // This is not always saved because of delayed joystick connection on some platforms like iOS.
                if (loadControllerAssignments && ReInput.controllers.joystickCount > 0) {
                    _wasJoystickEverDetected = true;
                    SaveControllerAssignments();
                }
            }
        }

        /// <summary>
        /// Called when a controller is connected.
        /// </summary>
        /// <param name="args">ControllerStatusChangedEventArgs</param>
        protected override void OnControllerConnected(ControllerStatusChangedEventArgs args) {
            if (!_isEnabled) return;

            // Load data when joystick is connected
            if (args.controllerType == ControllerType.Joystick) {
                int count = LoadJoystickData(args.controllerId);
#if UNITY_EDITOR
                if(count > 0) Debug.LogWarning(logPrefix + thisScriptName + " loaded Joystick " + args.controllerId + " (" + ReInput.controllers.GetJoystick(args.controllerId).hardwareName + ") data. " + editorLoadedMessage);
#endif

                // Load joystick assignments once on connect, but deferred until the end of the frame so all joysticks can connect first.
                // This is to get around the issue on some platforms like OSX, Xbox One, and iOS where joysticks are not
                // available immediately and may not be available for several seconds after the Rewired Input manager or
                // Unity starts. Also allows the user to start the game with no joysticks connected and on the first
                // joystick connected, load the assignments for a better user experience on phones/tablets.
                // No further joystick assignments will be made on connect.
                if (_loadDataOnStart && _loadJoystickAssignments && !_wasJoystickEverDetected) {
                    this.StartCoroutine(LoadJoystickAssignmentsDeferred());
                }

                // Save controller assignments
                if (_loadJoystickAssignments && !_deferredJoystickAssignmentLoadPending) { // do not save assignments while deferred loading is still pending
                    SaveControllerAssignments();
                }

                _wasJoystickEverDetected = true;
            }
        }

        /// <summary>
        /// Calls after a controller has been disconnected.
        /// </summary>
        /// <param name="args">ControllerStatusChangedEventArgs</param>
        protected override void OnControllerPreDisconnect(ControllerStatusChangedEventArgs args) {
            if (!_isEnabled) return;

            // Save data before joystick is disconnected
            if (args.controllerType == ControllerType.Joystick) {
                SaveJoystickData(args.controllerId);
#if UNITY_EDITOR
                Debug.Log(logPrefix + thisScriptName + " saved Joystick " + args.controllerId + " (" + ReInput.controllers.GetJoystick(args.controllerId).hardwareName + ") data.");
#endif
            }
        }

        /// <summary>
        /// Called when a controller is disconnected.
        /// </summary>
        /// <param name="args">ControllerStatusChangedEventArgs</param>
        protected override void OnControllerDisconnected(ControllerStatusChangedEventArgs args) {
            if (!_isEnabled) return;

            // Save controller assignments
            if (loadControllerAssignments) SaveControllerAssignments();
        }

        #endregion

        #region IControllerMapStore Implementation

        /// <summary>
        /// Saves a Controller Map.
        /// </summary>
        /// <param name="playerId">The Player id</param>
        /// <param name="controllerMap">The Controller Map</param>
        public override void SaveControllerMap(int playerId, ControllerMap controllerMap) {
            if (controllerMap == null) return;
            Player player = ReInput.players.GetPlayer(playerId);
            if (player == null) return;
            SaveControllerMap(player, controllerMap);
            dataStore.Save();
        }

        /// <summary>
        /// Loads a Controller Map for a Controller.
        /// </summary>
        /// <param name="playerId">The Player id</param>
        /// <param name="controllerIdentifier">Controller Identifier for the Controller. Get this from <see cref="Controller.identifier"/>.</param>
        /// <param name="categoryId">The Map Category id of the Controller Map</param>
        /// <param name="layoutId">The Layout id of the Controller Map</param>
        /// <returns>Controller Map</returns>
        public override ControllerMap LoadControllerMap(int playerId, ControllerIdentifier controllerIdentifier, int categoryId, int layoutId) {
            Player player = ReInput.players.GetPlayer(playerId);
            if (player == null) return null;
            return LoadControllerMap(player, controllerIdentifier, categoryId, layoutId);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Deletes the saved data.
        /// Note: This function does not reload default values in Players, Controllers, etc.
        /// </summary>
        public virtual void ClearSaveData() {
#if UNITY_EDITOR
            // This can be called in the editor, so it must be initialized
            if (!ReInput.isReady) {
                EditorInitialize();
            }
#endif
            dataStore.Clear();
        }

        #endregion

        #region Load

        private int LoadAll() {
            int count = 0;

            // Load controller assignments first so the right maps are loaded
            if (loadControllerAssignments) {
                if (LoadControllerAssignmentsNow()) count += 1;
            }

            // Load all data for all players
            IList<Player> allPlayers = ReInput.players.AllPlayers;
            for (int i = 0; i < allPlayers.Count; i++) {
                count += LoadPlayerDataNow(allPlayers[i]);
            }

            // Load all joystick calibration maps
            count += LoadAllJoystickCalibrationData();

            return count;
        }

        private int LoadPlayerDataNow(int playerId) {
            return LoadPlayerDataNow(ReInput.players.GetPlayer(playerId));
        }
        private int LoadPlayerDataNow(Player player) {
            if (player == null) return 0;

            int count = 0;

            // Load Input Behaviors
            count += LoadInputBehaviors(player.id);

            // Load Keyboard Maps
            count += LoadControllerMaps(player.id, ControllerType.Keyboard, 0);

            // Load Mouse Maps
            count += LoadControllerMaps(player.id, ControllerType.Mouse, 0);

            // Load Joystick Maps for each joystick
            foreach (Joystick joystick in player.controllers.Joysticks) {
                count += LoadControllerMaps(player.id, ControllerType.Joystick, joystick.id);
            }

            // Trigger Layout Manager refresh after load
            RefreshLayoutManager(player.id);

            return count;
        }

        private int LoadAllJoystickCalibrationData() {
            int count = 0;
            // Load all calibration maps from all joysticks
            IList<Joystick> joysticks = ReInput.controllers.Joysticks;
            for (int i = 0; i < joysticks.Count; i++) {
                count += LoadJoystickCalibrationData(joysticks[i]);
            }
            return count;
        }

        private int LoadJoystickCalibrationData(Joystick joystick) {
            if (joystick == null) return 0;
            return joystick.ImportCalibrationMapFromJsonString(GetJoystickCalibrationMapJson(joystick)) ? 1 : 0; // load joystick calibration map
        }
        private int LoadJoystickCalibrationData(int joystickId) {
            return LoadJoystickCalibrationData(ReInput.controllers.GetJoystick(joystickId));
        }

        private int LoadJoystickData(int joystickId) {
            int count = 0;
            // Load joystick maps in all Players for this joystick id
            IList<Player> allPlayers = ReInput.players.AllPlayers;
            for (int i = 0; i < allPlayers.Count; i++) { // this controller may be owned by more than one player, so check all
                Player player = allPlayers[i];
                if (!player.controllers.ContainsController(ControllerType.Joystick, joystickId)) continue; // player does not have the joystick
                count += LoadControllerMaps(player.id, ControllerType.Joystick, joystickId); // load the maps
                RefreshLayoutManager(player.id); // trigger Layout Manager refresh after load
            }

            // Load calibration maps for joystick
            count += LoadJoystickCalibrationData(joystickId);

            return count;
        }

        private int LoadControllerDataNow(int playerId, ControllerType controllerType, int controllerId) {
            int count = 0;

            // Load map data
            count += LoadControllerMaps(playerId, controllerType, controllerId);

            // Trigger Layout Manager refresh after load
            RefreshLayoutManager(playerId);

            // Loat other controller data
            count += LoadControllerDataNow(controllerType, controllerId);

            return count;
        }
        private int LoadControllerDataNow(ControllerType controllerType, int controllerId) {
            int count = 0;

            // Load calibration data for joysticks
            if (controllerType == ControllerType.Joystick) {
                count += LoadJoystickCalibrationData(controllerId);
            }

            return count;
        }

        private int LoadControllerMaps(int playerId, ControllerType controllerType, int controllerId) {
            int count = 0;
            Player player = ReInput.players.GetPlayer(playerId);
            if (player == null) return count;

            Controller controller = ReInput.controllers.GetController(controllerType, controllerId);
            if (controller == null) return count;

            IList<InputMapCategory> categories = ReInput.mapping.MapCategories;
            for (int i = 0; i < categories.Count; i++) {

                InputMapCategory category = categories[i];
                if (!category.userAssignable) continue; // skip map because not user-assignable

                IList<InputLayout> layouts = ReInput.mapping.MapLayouts(controller.type);
                for (int j = 0; j < layouts.Count; j++) {

                    InputLayout layout = layouts[j];

                    // Load the Controller Map
                    ControllerMap controllerMap = LoadControllerMap(player, controller.identifier, category.id, layout.id);
                    if (controllerMap == null) continue;

                    // Add the map to the Player
                    player.controllers.maps.AddMap(controller, controllerMap);
                    count += 1;
                }
            }

            return count;
        }

        private ControllerMap LoadControllerMap(Player player, ControllerIdentifier controllerIdentifier, int categoryId, int layoutId) {
            if (player == null) return null;

            // Get the Json for the Controller Map
            string json = GetControllerMapJson(player, controllerIdentifier, categoryId, layoutId);
            if (string.IsNullOrEmpty(json)) return null;

            ControllerMap controllerMap = ControllerMap.CreateFromJson(controllerIdentifier.controllerType, json);
            if (controllerMap == null) return null;

            // Load default mappings for new Actions
            List<int> knownActionIds = GetControllerMapKnownActionIds(player, controllerIdentifier, categoryId, layoutId);
            AddDefaultMappingsForNewActions(controllerIdentifier, controllerMap, knownActionIds);

            return controllerMap;
        }

        private int LoadInputBehaviors(int playerId) {
            Player player = ReInput.players.GetPlayer(playerId);
            if (player == null) return 0;

            int count = 0;

            // All players have an instance of each input behavior so it can be modified
            IList<InputBehavior> behaviors = ReInput.mapping.GetInputBehaviors(player.id); // get all behaviors from player
            for (int i = 0; i < behaviors.Count; i++) {
                count += LoadInputBehaviorNow(player, behaviors[i]);
            }

            return count;
        }

        private int LoadInputBehaviorNow(int playerId, int behaviorId) {
            Player player = ReInput.players.GetPlayer(playerId);
            if (player == null) return 0;

            InputBehavior behavior = ReInput.mapping.GetInputBehavior(playerId, behaviorId);
            if (behavior == null) return 0;

            return LoadInputBehaviorNow(player, behavior);
        }
        private int LoadInputBehaviorNow(Player player, InputBehavior inputBehavior) {
            if (player == null || inputBehavior == null) return 0;

            string json = GetInputBehaviorJson(player, inputBehavior.id); // try to the behavior for this id
            if (json == null || json == string.Empty) return 0; // no data found for this behavior
            return inputBehavior.ImportJsonString(json) ? 1 : 0; // import the data into the behavior
        }

        private bool LoadControllerAssignmentsNow() {
            try {
                // Try to load assignment save data
                ControllerAssignmentSaveInfo data = LoadControllerAssignmentData();
                if (data == null) return false;

                // Load keyboard and mouse assignments
                if (_loadKeyboardAssignments || _loadMouseAssignments) {
                    LoadKeyboardAndMouseAssignmentsNow(data);
                }

                // Load joystick assignments
                if (_loadJoystickAssignments) {
                    LoadJoystickAssignmentsNow(data);
                }

#if UNITY_EDITOR
                Debug.LogWarning(logPrefix + thisScriptName + " loaded controller assignments.");
#endif
            } catch {
#if UNITY_EDITOR
                Debug.LogError(logPrefix + thisScriptName + " encountered an error loading controller assignments.");
#endif
            }

            return true;
        }

        private bool LoadKeyboardAndMouseAssignmentsNow(ControllerAssignmentSaveInfo data) {
            try {
                // Try to load the save data
                if (data == null && (data = LoadControllerAssignmentData()) == null) return false;

                // Process each Player assigning controllers from the save data
                foreach (Player player in ReInput.players.AllPlayers) {
                    if (!data.ContainsPlayer(player.id)) continue;
                    ControllerAssignmentSaveInfo.PlayerInfo playerData = data.players[data.IndexOfPlayer(player.id)];

                    // Assign keyboard
                    if (_loadKeyboardAssignments) {
                        player.controllers.hasKeyboard = playerData.hasKeyboard;
                    }

                    // Assign mouse
                    if (_loadMouseAssignments) {
                        player.controllers.hasMouse = playerData.hasMouse;
                    }
                }
            } catch {
#if UNITY_EDITOR
                Debug.LogError(logPrefix + thisScriptName + " encountered an error loading keyboard and/or mouse assignments.");
#endif
            }

            return true;
        }

        private bool LoadJoystickAssignmentsNow(ControllerAssignmentSaveInfo data) {
            try {
                if (ReInput.controllers.joystickCount == 0) return false; // no joysticks to assign

                // Try to load the save data
                if (data == null && (data = LoadControllerAssignmentData()) == null) return false;

                // Unassign all Joysticks first
                foreach (Player player in ReInput.players.AllPlayers) {
                    player.controllers.ClearControllersOfType(ControllerType.Joystick);
                }

                // Create a history which helps in assignment of imprecise matches back to the same Players
                // even when the same Joystick is assigned to multiple Players.
                List<JoystickAssignmentHistoryInfo> joystickHistory = _loadJoystickAssignments ? new List<JoystickAssignmentHistoryInfo>() : null;

                // Process each Player assigning controllers from the save data
                foreach (Player player in ReInput.players.AllPlayers) {
                    if (!data.ContainsPlayer(player.id)) continue;
                    ControllerAssignmentSaveInfo.PlayerInfo playerData = data.players[data.IndexOfPlayer(player.id)];

                    // Assign joysticks
                    for (int i = 0; i < playerData.joystickCount; i++) {
                        ControllerAssignmentSaveInfo.JoystickInfo joystickInfo = playerData.joysticks[i];
                        if (joystickInfo == null) continue;

                        // Find a matching Joystick if any
                        Joystick joystick = FindJoystickPrecise(joystickInfo); // only assign joysticks with precise matching information
                        if (joystick == null) continue;

                        // Add the Joystick to the history
                        if (joystickHistory.Find(x => x.joystick == joystick) == null) {
                            joystickHistory.Add(new JoystickAssignmentHistoryInfo(joystick, joystickInfo.id));
                        }

                        // Assign the Joystick to the Player
                        player.controllers.AddController(joystick, false);
                    }
                }

                // Do another joystick assignment pass with imprecise matching info all precise matches are done.
                // This is done to make sure all the joysticks with exact matching info get assigned to the right Players
                // before assigning any joysticks with imprecise matching info to reduce the chances of a mis-assignment.
                // This is not allowed on all platforms to prevent issues with system player/id and Rewired Player alignment.

                if (_allowImpreciseJoystickAssignmentMatching) {
                    foreach (Player player in ReInput.players.AllPlayers) {
                        if (!data.ContainsPlayer(player.id)) continue;
                        ControllerAssignmentSaveInfo.PlayerInfo playerData = data.players[data.IndexOfPlayer(player.id)];

                        for (int i = 0; i < playerData.joystickCount; i++) {
                            ControllerAssignmentSaveInfo.JoystickInfo joystickInfo = playerData.joysticks[i];
                            if (joystickInfo == null) continue;

                            Joystick joystick = null;

                            // Check assignment history for joystick first
                            int index = joystickHistory.FindIndex(x => x.oldJoystickId == joystickInfo.id);
                            if (index >= 0) { // found in history
                                joystick = joystickHistory[index].joystick; // just get the Joystick from the history
                            } else { // not in history, try to find otherwise

                                // Find all matching Joysticks excluding all Joysticks that have precise matching information available
                                List<Joystick> matches;
                                if (!TryFindJoysticksImprecise(joystickInfo, out matches)) continue; // no matches found

                                // Find the first Joystick that's not already in the history
                                foreach (Joystick match in matches) {
                                    if (joystickHistory.Find(x => x.joystick == match) != null) continue;
                                    joystick = match;
                                    break;
                                }
                                if (joystick == null) continue; // no suitable match found

                                // Add the Joystick to the history
                                joystickHistory.Add(new JoystickAssignmentHistoryInfo(joystick, joystickInfo.id));
                            }

                            // Assign the joystick to the Player
                            player.controllers.AddController(joystick, false);
                        }
                    }
                }
            } catch {
#if UNITY_EDITOR
                Debug.LogError(logPrefix + thisScriptName + " encountered an error loading joystick assignments.");
#endif
            }

            // Auto-assign Joysticks in case save data doesn't include all attached Joysticks
            if (ReInput.configuration.autoAssignJoysticks) {
                ReInput.controllers.AutoAssignJoysticks();
            }

            return true;
        }

        private ControllerAssignmentSaveInfo LoadControllerAssignmentData() {
            try {
                // Check if there is any data saved
                string json;
                if (!TryGetString(dataStore, key_controllerAssignments, out json)) return null;
                if (string.IsNullOrEmpty(json)) return null;

                // Parse Json
                ControllerAssignmentSaveInfo data = JsonParser.FromJson<ControllerAssignmentSaveInfo>(json);
                if (data == null || data.playerCount == 0) return null; // no valid save data found

                return data;
            } catch {
                return null;
            }
        }

        private IEnumerator LoadJoystickAssignmentsDeferred() {
            _deferredJoystickAssignmentLoadPending = true;

            yield return new WaitForEndOfFrame(); // defer until the end of the frame
            if (!ReInput.isReady) yield break; // in case Rewired was shut down

            // Load the joystick assignments
            if (LoadJoystickAssignmentsNow(null)) {
#if UNITY_EDITOR
                Debug.LogWarning(logPrefix + thisScriptName + " loaded joystick assignments.");
#endif
            }

            // Save the controller assignments after loading in case anything has been
            // re-assigned to a different Player or a new joystick was connected.
            SaveControllerAssignments();

            _deferredJoystickAssignmentLoadPending = false;
        }

        #endregion

        #region Save

        private void SaveAll() {
            
            // Save all data in all Players including System Player
            IList<Player> allPlayers = ReInput.players.AllPlayers;
            for (int i = 0; i < allPlayers.Count; i++) {
                SavePlayerDataNow(allPlayers[i]);
            }

            // Save joystick calibration maps
            SaveAllJoystickCalibrationData();

            // Save controller assignments
            if (loadControllerAssignments) {
                SaveControllerAssignments();
            }

            // Save changes to file
            dataStore.Save();
        }

        private void SavePlayerDataNow(int playerId) {
            SavePlayerDataNow(ReInput.players.GetPlayer(playerId));

            // Save changes to file
            dataStore.Save();
        }
        private void SavePlayerDataNow(Player player) {
            if (player == null) return;

            // Get all savable data from player
            PlayerSaveData playerData = player.GetSaveData(true);

            // Save Input Behaviors
            SaveInputBehaviors(player, playerData);

            // Save controller maps
            SaveControllerMaps(player, playerData);
        }

        private void SaveAllJoystickCalibrationData() {
            // Save all calibration maps from all joysticks
            IList<Joystick> joysticks = ReInput.controllers.Joysticks;
            for (int i = 0; i < joysticks.Count; i++) {
                SaveJoystickCalibrationData(joysticks[i]);
            }
        }

        private void SaveJoystickCalibrationData(int joystickId) {
            SaveJoystickCalibrationData(ReInput.controllers.GetJoystick(joystickId));
        }
        private void SaveJoystickCalibrationData(Joystick joystick) {
            if (joystick == null) return;
            JoystickCalibrationMapSaveData saveData = joystick.GetCalibrationMapSaveData();
            string key = GetJoystickCalibrationMapKey(joystick);
            dataStore.SetValue(key, saveData.map.ToJsonString()); // save the map in Json format
        }

        private void SaveJoystickData(int joystickId) {
            // Save joystick maps in all Players for this joystick id
            IList<Player> allPlayers = ReInput.players.AllPlayers;
            for (int i = 0; i < allPlayers.Count; i++) { // this controller may be owned by more than one player, so check all
                Player player = allPlayers[i];
                if (!player.controllers.ContainsController(ControllerType.Joystick, joystickId)) continue; // player does not have the joystick

                // Save controller maps
                SaveControllerMaps(player.id, ControllerType.Joystick, joystickId);
            }

            // Save calibration data
            SaveJoystickCalibrationData(joystickId);
        }

        private void SaveControllerDataNow(int playerId, ControllerType controllerType, int controllerId) {

            // Save map data
            SaveControllerMaps(playerId, controllerType, controllerId);

            // Save other controller data
            SaveControllerData(controllerType, controllerId);
        }
        private void SaveControllerDataNow(ControllerType controllerType, int controllerId) {

            // Save calibration data for joysticks
            if (controllerType == ControllerType.Joystick) {
                SaveJoystickCalibrationData(controllerId);
            }
        }

        private void SaveControllerMaps(Player player, PlayerSaveData playerSaveData) {
            foreach (ControllerMapSaveData saveData in playerSaveData.AllControllerMapSaveData) {
                SaveControllerMap(player, saveData.map);
            }
        }
        private void SaveControllerMaps(int playerId, ControllerType controllerType, int controllerId) {
            Player player = ReInput.players.GetPlayer(playerId);
            if (player == null) return;

            // Save controller maps in this player for this controller id
            if (!player.controllers.ContainsController(controllerType, controllerId)) return; // player does not have the controller

            // Save controller maps
            ControllerMapSaveData[] saveData = player.controllers.maps.GetMapSaveData(controllerType, controllerId, true);
            if (saveData == null) return;

            for (int i = 0; i < saveData.Length; i++) {
                SaveControllerMap(player, saveData[i].map);
            }
        }

        private void SaveControllerMap(Player player, ControllerMap controllerMap) {
            // Save the Controller Map
            string key = GetControllerMapKey(player, controllerMap.controller.identifier, controllerMap.categoryId, controllerMap.layoutId, controllerMapKeyVersion);
            dataStore.SetValue(key, controllerMap.ToJsonString());

            // Save the Action ids list for this Controller Map used to allow new Actions to be added to the
            // Rewired Input Manager and have the new mappings show up when saved data is loaded
            key = GetControllerMapKnownActionIdsKey(player, controllerMap.controller.identifier, controllerMap.categoryId, controllerMap.layoutId, controllerMapKeyVersion);
            dataStore.SetValue(key, allActionIdsString);
        }

        private void SaveInputBehaviors(Player player, PlayerSaveData playerSaveData) {
            if (player == null) return;
            InputBehavior[] inputBehaviors = playerSaveData.inputBehaviors;
            for (int i = 0; i < inputBehaviors.Length; i++) {
                SaveInputBehaviorNow(player, inputBehaviors[i]);
            }
        }

        private void SaveInputBehaviorNow(int playerId, int behaviorId) {
            Player player = ReInput.players.GetPlayer(playerId);
            if (player == null) return;

            InputBehavior behavior = ReInput.mapping.GetInputBehavior(playerId, behaviorId);
            if (behavior == null) return;

            SaveInputBehaviorNow(player, behavior);

            // Save changes to file
            dataStore.Save();
        }
        private void SaveInputBehaviorNow(Player player, InputBehavior inputBehavior) {
            if (player == null || inputBehavior == null) return;
            string key = GetInputBehaviorKey(player, inputBehavior.id);
            dataStore.SetValue(key, inputBehavior.ToJsonString()); // save the behavior in Json format
        }

        private bool SaveControllerAssignments() {
            try {
                // Save a complete snapshot of controller assignments in all Players
                ControllerAssignmentSaveInfo allPlayerData = new ControllerAssignmentSaveInfo(ReInput.players.allPlayerCount);

                for (int i = 0; i < ReInput.players.allPlayerCount; i++) {
                    Player player = ReInput.players.AllPlayers[i];

                    ControllerAssignmentSaveInfo.PlayerInfo playerData = new ControllerAssignmentSaveInfo.PlayerInfo();
                    allPlayerData.players[i] = playerData;

                    playerData.id = player.id;

                    // Add has keyboard
                    playerData.hasKeyboard = player.controllers.hasKeyboard;

                    // Add has mouse
                    playerData.hasMouse = player.controllers.hasMouse;

                    // Add joysticks
                    ControllerAssignmentSaveInfo.JoystickInfo[] joystickInfos = new ControllerAssignmentSaveInfo.JoystickInfo[player.controllers.joystickCount];
                    playerData.joysticks = joystickInfos;
                    for (int j = 0; j < player.controllers.joystickCount; j++) {
                        Joystick joystick = player.controllers.Joysticks[j];

                        ControllerAssignmentSaveInfo.JoystickInfo joystickInfo = new ControllerAssignmentSaveInfo.JoystickInfo();

                        // Record the device instance id.
                        joystickInfo.instanceGuid = joystick.deviceInstanceGuid;

                        // Record the joystick id for joysticks with only imprecise information so we can use this
                        // to determine if the same joystick was assigned to multiple Players.
                        joystickInfo.id = joystick.id;

                        // Record the hardware identifier string.
                        joystickInfo.hardwareIdentifier = joystick.hardwareIdentifier;

                        // Store the info
                        joystickInfos[j] = joystickInfo;
                    }
                }

                // Save data
                dataStore.SetValue(key_controllerAssignments, JsonWriter.ToJson(allPlayerData));
                dataStore.Save();

#if UNITY_EDITOR
                Debug.Log(logPrefix + thisScriptName + " saved controller assignments.");
#endif
            } catch {
#if UNITY_EDITOR
                Debug.LogError(logPrefix + thisScriptName + " encountered an error saving controller assignments.");
#endif
            }
            return true;
        }

        #endregion

        #region Key Methods

        private static void AppendBaseKey(StringBuilder sb, Player player) {
            sb.Append("playerId=");
            sb.Append(player.id); // make a key for this specific player, could use id, descriptive name, or a custom profile identifier of your choice
        }

        private string GetControllerMapKey(Player player, ControllerIdentifier controllerIdentifier, int categoryId, int layoutId, int ppKeyVersion) {
            _sb.Length = 0;
            AppendBaseKey(_sb, player);
            _sb.Append("|dataType=ControllerMap");
            AppendControllerMapKeyCommonSuffix(_sb, player, controllerIdentifier, categoryId, layoutId, ppKeyVersion);
            return _sb.ToString();
        }

        private string GetControllerMapKnownActionIdsKey(Player player, ControllerIdentifier controllerIdentifier, int categoryId, int layoutId, int ppKeyVersion) {
            _sb.Length = 0;
            AppendBaseKey(_sb, player);
            _sb.Append("|dataType=ControllerMap_KnownActionIds");
            AppendControllerMapKeyCommonSuffix(_sb, player, controllerIdentifier, categoryId, layoutId, ppKeyVersion);
            return _sb.ToString();
        }

        private static void AppendControllerMapKeyCommonSuffix(StringBuilder sb, Player player, ControllerIdentifier controllerIdentifier, int categoryId, int layoutId, int keyVersion) {
            sb.Append("|kv=");
            sb.Append(keyVersion); // include the key version in the string
            sb.Append("|controllerMapType=");
            sb.Append((int)controllerIdentifier.controllerType);
            sb.Append("|categoryId=");
            sb.Append(categoryId);
            sb.Append("|");
            sb.Append("layoutId=");
            sb.Append(layoutId);

            // Support loading controller maps for disconnected controllers
            sb.Append("|hardwareGuid=");
            sb.Append(controllerIdentifier.hardwareTypeGuid); // the identifying GUID that determines which known controller this is
            if (controllerIdentifier.hardwareTypeGuid == Guid.Empty) { // not recognized, Hardware Idenfitier is required
                // This is not included for recognized controllers because it makes it impossible to lookup the map when the controller is not attached because the hardware identifier cannot be known without the device present.
                sb.Append("|hardwareIdentifier=");
                sb.Append(controllerIdentifier.hardwareIdentifier); // the hardware identifier string helps us identify maps for unknown hardware because it doesn't have a Guid
            }
            if (controllerIdentifier.controllerType == ControllerType.Joystick) { // store special info for joystick maps
                sb.Append("|duplicate=");
                sb.Append(GetDuplicateIndex(player, controllerIdentifier).ToString());
            }
        }

        private string GetJoystickCalibrationMapKey(Joystick joystick) {
            _sb.Length = 0;
            _sb.Append("dataType=CalibrationMap");
            _sb.Append("|controllerType=");
            _sb.Append((int)joystick.type);
            _sb.Append("|hardwareIdentifier=");
            _sb.Append(joystick.hardwareIdentifier); // the hardware identifier string helps us identify maps for unknown hardware because it doesn't have a Guid
            _sb.Append("|hardwareGuid=");
            _sb.Append(joystick.hardwareTypeGuid.ToString());
            return _sb.ToString();
        }

        private string GetInputBehaviorKey(Player player, int inputBehaviorId) {
            _sb.Length = 0;
            AppendBaseKey(_sb, player);
            _sb.Append("|dataType=InputBehavior");
            _sb.Append("|id=");
            _sb.Append(inputBehaviorId);
            return _sb.ToString();
        }

        private string GetControllerMapJson(Player player, ControllerIdentifier controllerIdentifier, int categoryId, int layoutId) {
            string key;
            // Must try many times because of new additions in various versions
            for (int i = controllerMapKeyVersion; i >= 0; i--) {
                key = GetControllerMapKey(player, controllerIdentifier, categoryId, layoutId, i);
                string value;
                if (TryGetString(dataStore, key, out value) && !string.IsNullOrEmpty(value)) {
                    return value;
                }
            }
            return null;
        }

        private List<int> GetControllerMapKnownActionIds(Player player, ControllerIdentifier controllerIdentifier, int categoryId, int layoutId) {
            List<int> actionIds = new List<int>();
            string key;
            string value = null;
            bool found = false;

            // Must try many times because of new additions in various versions
            for (int i = controllerMapKeyVersion; i >= 0; i--) {
                key = GetControllerMapKnownActionIdsKey(player, controllerIdentifier, categoryId, layoutId, i);
                if (TryGetString(dataStore, key, out value)) {
                    found = true;
                    break;
                }
            }
            if (!found) return actionIds; // key does not exist

            // Get the data and try to parse it
            if (string.IsNullOrEmpty(value)) return actionIds;

            string[] split = value.Split(',');
            for (int i = 0; i < split.Length; i++) {
                if (string.IsNullOrEmpty(split[i])) continue;
                int id;
                if (int.TryParse(split[i], out id)) {
                    actionIds.Add(id);
                }
            }
            return actionIds;
        }

        private string GetJoystickCalibrationMapJson(Joystick joystick) {
            string key = GetJoystickCalibrationMapKey(joystick);
            string value;
            TryGetString(dataStore, key, out value);
            return value;
        }

        private string GetInputBehaviorJson(Player player, int id) {
            string key = GetInputBehaviorKey(player, id);
            string value;
            TryGetString(dataStore, key, out value);
            return value;
        }

        #endregion

        #region Editor

#if UNITY_EDITOR

        protected virtual void EditorInitialize() {
        }

#endif

        #endregion

        #region Misc

        private void AddDefaultMappingsForNewActions(ControllerIdentifier controllerIdentifier, ControllerMap controllerMap, List<int> knownActionIds) {
            if (controllerMap == null || knownActionIds == null) return;
            if (knownActionIds == null || knownActionIds.Count == 0) return;

            // Check for new Actions added to the default mappings that didn't exist when the Controller Map was saved

            // Load default map for comparison
            ControllerMap defaultMap = ReInput.mapping.GetControllerMapInstance(controllerIdentifier, controllerMap.categoryId, controllerMap.layoutId);
            if (defaultMap == null) return;

            // Find any new Action ids that didn't exist when the Controller Map was saved
            List<int> unknownActionIds = new List<int>();
            foreach (int id in allActionIds) {
                if (knownActionIds.Contains(id)) continue;
                unknownActionIds.Add(id);
            }

            if (unknownActionIds.Count == 0) return; // no new Action ids

            // Add all mappings in the default map for previously unknown Action ids
            foreach (ActionElementMap aem in defaultMap.AllMaps) {
                if (!unknownActionIds.Contains(aem.actionId)) continue;

                // Skip this ActionElementMap if there's a conflict within the loaded map
                if (controllerMap.DoesElementAssignmentConflict(aem)) continue;

                // Create an assignment
                ElementAssignment assignment = new ElementAssignment(
                    controllerMap.controllerType,
                    aem.elementType,
                    aem.elementIdentifierId,
                    aem.axisRange,
                    aem.keyCode,
                    aem.modifierKeyFlags,
                    aem.actionId,
                    aem.axisContribution,
                    aem.invert
                );

                // Assign it
                controllerMap.CreateElementMap(assignment);
            }
        }

        private Joystick FindJoystickPrecise(ControllerAssignmentSaveInfo.JoystickInfo joystickInfo) {
            if (joystickInfo == null) return null;
            if (joystickInfo.instanceGuid == Guid.Empty) return null; // do not handle invalid instance guids

            // Find a matching joystick
            IList<Joystick> joysticks = ReInput.controllers.Joysticks;
            for (int i = 0; i < joysticks.Count; i++) {
                if (joysticks[i].deviceInstanceGuid == joystickInfo.instanceGuid) return joysticks[i];
            }

            return null;
        }

        private bool TryFindJoysticksImprecise(ControllerAssignmentSaveInfo.JoystickInfo joystickInfo, out List<Joystick> matches) {
            matches = null;
            if (joystickInfo == null) return false;
            if (string.IsNullOrEmpty(joystickInfo.hardwareIdentifier)) return false; // do not handle invalid hardware identifiers

            // Find a matching joystick
            IList<Joystick> joysticks = ReInput.controllers.Joysticks;
            for (int i = 0; i < joysticks.Count; i++) {
                if (string.Equals(joysticks[i].hardwareIdentifier, joystickInfo.hardwareIdentifier, StringComparison.OrdinalIgnoreCase)) {
                    if (matches == null) matches = new List<Joystick>();
                    matches.Add(joysticks[i]);
                }
            }
            return matches != null;
        }

        private void RefreshLayoutManager(int playerId) {
            Player player = ReInput.players.GetPlayer(playerId);
            if (player == null) return;
            player.controllers.maps.layoutManager.Apply();
        }

        #endregion

        #region Static

        private static int GetDuplicateIndex(Player player, ControllerIdentifier controllerIdentifier) {
            // Determine how many duplicates of this controller are owned by this Player
            Controller controller = ReInput.controllers.GetController(controllerIdentifier);
            if (controller == null) return 0; // cannot support index count if the controller is not connected
            int duplicateCount = 0;
            foreach (var c in player.controllers.Controllers) {
                if (c.type != controller.type) continue;
                bool isRecognized = false;
                if (controller.type == ControllerType.Joystick) {
                    if ((c as Joystick).hardwareTypeGuid != controller.hardwareTypeGuid) continue;
                    if (controller.hardwareTypeGuid != Guid.Empty) isRecognized = true;
                }
                if (!isRecognized && c.hardwareIdentifier != controller.hardwareIdentifier) continue;
                if (c == controller) return duplicateCount;
                duplicateCount++;
            }
            return duplicateCount;
        }

        private static bool TryGetString(IDataStore store, string key, out string result) {
            if (store == null || string.IsNullOrEmpty(key)) {
                result = null;
                return false;
            }
            object objectValue;
            if (!store.TryGetValue(key, out objectValue)) {
                result = null;
                return false;
            }
            result = objectValue as string;
            return objectValue is string;
        }

        #endregion

        #region Classes

        private class ControllerAssignmentSaveInfo {

            public PlayerInfo[] players;

            public int playerCount { get { return players != null ? players.Length : 0; } }

            public ControllerAssignmentSaveInfo() {
            }
            public ControllerAssignmentSaveInfo(int playerCount) {
                this.players = new PlayerInfo[playerCount];
                for (int i = 0; i < playerCount; i++) {
                    players[i] = new PlayerInfo();
                }
            }

            public int IndexOfPlayer(int playerId) {
                for (int i = 0; i < playerCount; i++) {
                    if (players[i] == null) continue;
                    if (players[i].id == playerId) return i;
                }
                return -1;
            }

            public bool ContainsPlayer(int playerId) {
                return IndexOfPlayer(playerId) >= 0;
            }

            public class PlayerInfo {

                public int id;
                public bool hasKeyboard;
                public bool hasMouse;
                public JoystickInfo[] joysticks;

                public int joystickCount { get { return joysticks != null ? joysticks.Length : 0; } }

                public int IndexOfJoystick(int joystickId) {
                    for (int i = 0; i < joystickCount; i++) {
                        if (joysticks[i] == null) continue;
                        if (joysticks[i].id == joystickId) return i;
                    }
                    return -1;
                }

                public bool ContainsJoystick(int joystickId) {
                    return IndexOfJoystick(joystickId) >= 0;
                }
            }

            public class JoystickInfo {
                public Guid instanceGuid;
                public string hardwareIdentifier;
                public int id;
            }
        }

        private class JoystickAssignmentHistoryInfo {

            public readonly Joystick joystick;
            public readonly int oldJoystickId;

            public JoystickAssignmentHistoryInfo(Joystick joystick, int oldJoystickId) {
                if (joystick == null) throw new ArgumentNullException("joystick");
                this.joystick = joystick;
                this.oldJoystickId = oldJoystickId;
            }
        }

        /// <summary>
        /// Interface for a data store.
        /// </summary>
        protected interface IDataStore {
            /// <summary>
            /// Save data.
            /// </summary>
            /// <returns>True on success, false on failure.</returns>
            bool Save();
            /// <summary>
            /// Load data.
            /// </summary>
            /// <returns>True on success, false on failure.</returns>
            bool Load();
            /// <summary>
            /// Clear data.
            /// </summary>
            /// <returns>True on success, false on failure.</returns>
            bool Clear();
            /// <summary>
            /// Gets the value at the specified key if it exists.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <param name="result">The result.</param>
            /// <returns>True on success, false on failure.</returns>
            bool TryGetValue(string key, out object result);
            /// <summary>
            /// Sets the value at the specified key.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <param name="value">The value.</param>
            /// <returns>True on success, false on failure.</returns>
            bool SetValue(string key, object value);
        }

        #endregion
    }
}
// Copyright (c) 2024 Augie R. Maddox, Guavaman Enterprises. All rights reserved.

#pragma warning disable 0649

namespace Rewired.Editor {

    using UnityEngine;
    using UnityEditor;
    using Rewired.Data;

    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    [CustomEditor(typeof(UserDataStore_File), true)]
    public sealed class UserDataStore_FileInspector : UnityEditor.Editor {

        private bool showDebugOptions;

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            showDebugOptions = EditorGUILayout.Foldout(showDebugOptions, new GUIContent("Debug Options"));

            if(showDebugOptions) {
                GUILayout.Space(15);
                if(GUILayout.Button(new GUIContent("Clear Data", "This will clear saved user data."))) {
                    if(EditorUtility.DisplayDialog("Clear Data", "WARNING: This will delete saved user data. Are you sure?", "DELETE", "Cancel")) {
                        (target as UserDataStore_File).ClearSaveData();
                    }
                }
                GUILayout.Space(15);
            }
        }
    }
}
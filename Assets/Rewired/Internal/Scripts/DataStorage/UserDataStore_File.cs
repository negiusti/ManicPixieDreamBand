// Copyright (c) 2024 Augie R. Maddox, Guavaman Enterprises. All rights reserved.

#if UNITY_4_6 || UNITY_4_7 || UNITY_5 || UNITY_2017 || UNITY_2018 || UNITY_2019 || UNITY_2020 || UNITY_2021 || UNITY_2022 || UNITY_2023 || UNITY_6000 || UNITY_6000_0_OR_NEWER
#define UNITY_4_6_PLUS
#endif

#pragma warning disable 0649

namespace Rewired.Data {
    using UnityEngine;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Rewired.Utils.Libraries.TinyJson;
    using System.Text;

    /// <summary>
    /// Saves data to a file.
    /// Add this as a component to your Rewired Input Manager to save and load data.
    /// Can be extended to customize data handling.
    /// </summary>
    public class UserDataStore_File : UserDataStore_KeyValue {

        private readonly static string thisScriptName = typeof(UserDataStore_File).Name;
        private const string logPrefix = "Rewired: ";
        private const string defaultExtensionText = ".json";
        private const string defaultExtensionBinary = ".bin";
        private const string defaultFileName = "RewiredSaveData" + defaultExtensionText;

#if UNITY_4_6_PLUS
        [Tooltip("The data file name. Changing this will make saved data already stored with the old file name no longer accessible.")]
#endif
        [UnityEngine.SerializeField]
        private string _fileName = defaultFileName;
#if UNITY_4_6_PLUS
        [Tooltip("Determines if the file should be stored as binary or text. Changing this will make saved data already stored no longer accessible.")]
#endif
        [UnityEngine.SerializeField]
        private DataFormat _dataFormat = DataFormat.Text;

        /// <summary>
        /// The absolute path to the data directory.
        /// This defaults to Application.persistentDataPath.
        /// Set to null to use default directory.
        /// </summary>
        public string directory {
            get {
                return !string.IsNullOrEmpty(__directory) ? __directory : (__directory = Application.persistentDataPath);
            }
            set {
                __directory = value;
                if (_initialized) OnDataSourceChanged(); // allow setting during initialization
            }
        }

        /// <summary>
        /// The data file name. Change this to change how where data is stored. Changing this will make saved data already stored no longer accessible.
        /// </summary>
        public string fileName {
            get {
                return _fileName;
            }
            set {
                _fileName = value; if (_initialized) OnDataSourceChanged(); // allow setting during initialization
            }
        }

        /// <summary>
        /// Determines if the file should be stored as binary or text.
        /// Changing this will make saved data already stored no longer accessible.
        /// </summary>
        public DataFormat dataFormat {
            get {
                return _dataFormat;
            }
            set {
                _dataFormat = value;
                if (_initialized) OnDataSourceChanged(); // allow setting during initialization
            }
        }

        /// <summary>
        /// Handler for saving, loading, and clearing data.
        /// Set this if you want to manage data storage manually.
        /// This can be changed if you want to store the save data in a custom format or location.
        /// Set this to null to use the default local file handler.
        /// </summary>
        protected IDataHandler dataHandler {
            get {
                return __dataHandler != null ?
                    __dataHandler :
                    __dataHandler = new LocalFileDataHandler(() => _dataFormat, new CLZF2());
            }
            set {
                __dataHandler = value;
                if (_initialized) OnDataSourceChanged(); // allow setting during initialization
            }
        }

        /// <summary>
        /// The data store.
        /// </summary>
        protected override IDataStore dataStore {
            get {
                return _dataStore;
            }
        }

        [NonSerialized]
        private string __directory;
        [NonSerialized]
        private DataStore _dataStore;
        [NonSerialized]
        private IDataHandler __dataHandler;
        [NonSerialized]
        private bool _initialized;

        /// <summary>
        /// Override this to set values before initialization.
        /// </summary>
        protected virtual void SetInitialValues() {
        }

        /// <summary>
        /// This should not be overridden.
        /// Initialize sub-classes using <see cref="SetInitialValues"/> instead.
        /// </summary>
        protected override void OnInitialize() {
            SetInitialValues();
            _initialized = true;
            OnDataSourceChanged();
            base.OnInitialize();
        }

        #region Misc

        private void OnDataSourceChanged() {
            _dataStore = new DataStore(
                !string.IsNullOrEmpty(_fileName) ? _fileName : defaultFileName, // make sure file name is non null
                directory,
                dataHandler
            );
        }

        #endregion

#if UNITY_EDITOR

        [NonSerialized]
        private DataFormat _editor_dataFormat;

        private void OnValidate() {
            if (_editor_dataFormat != _dataFormat) {
                _editor_dataFormat = _dataFormat;
                if (_dataFormat == DataFormat.Binary && _fileName.EndsWith(defaultExtensionText)) {
                    _fileName = Path.GetFileNameWithoutExtension(_fileName) + defaultExtensionBinary;
                    OnDataSourceChanged();
                } else if(_dataFormat == DataFormat.Text && _fileName.EndsWith(defaultExtensionBinary)) {
                    _fileName = Path.GetFileNameWithoutExtension(_fileName) + defaultExtensionText;
                    OnDataSourceChanged();
                }
            }
        }

        protected override void EditorInitialize() {
            base.EditorInitialize();
            if (_dataStore == null) {
                SetInitialValues();
                OnDataSourceChanged();
            }
        }

#endif

        // Static

        #region Classes

        private sealed class DataStore : IDataStore {

            private Dictionary<string, object> _data;
            private readonly string _absFilePath;
            private IDataHandler _dataHandler;

            public DataStore(string fileName, string absDirectory, IDataHandler dataHandler) {
                _absFilePath = Path.Combine(absDirectory, fileName);
                if (dataHandler == null) throw new ArgumentNullException("dataHandler");
                _dataHandler = dataHandler;
                _data = new Dictionary<string, object>();
                Load();
            }

            public bool TryGetValue(string key, out object value) {
                if (string.IsNullOrEmpty(key)) {
                    value = null;
                    return false;
                }
                return _data.TryGetValue(key, out value);
            }

            public bool SetValue(string key, object value) {
                if (string.IsNullOrEmpty(key)) return false;
                _data[key] = value;
                return true;
            }

            public bool Save() {
                try {
                    return _dataHandler.Save(_absFilePath, JsonWriter.ToJson(_data));
                } catch (Exception ex) {
                    UnityEngine.Debug.LogError(ex);
                    return false;
                }
            }

            public bool Load() {
                bool result;
                try {
                    string json;
                    result = _dataHandler.Load(_absFilePath, out json);
                    if (result) {
                        Dictionary<string, object> jsonDict = JsonParser.FromJson<Dictionary<string, object>>(json);
                        if (jsonDict == null) jsonDict = new Dictionary<string, object>();
                        _data = jsonDict;
                    }
                    return result;
                } catch (Exception ex) {
                    UnityEngine.Debug.LogError(ex);
                    return false;
                }
            }

            public bool Clear() {
                bool result;
                try {
                    result = _dataHandler.Clear(_absFilePath);
                } catch (Exception ex) {
                    UnityEngine.Debug.LogError(ex);
                    result = false;
                }
                _data.Clear();
                return result;
            }
        }

        private sealed class LocalFileDataHandler : IDataHandler {

            private readonly Func<DataFormat> _dataFormatDelegate;
            private readonly Codec _codec;

            public LocalFileDataHandler(Func<DataFormat> dataFormatDelegate, Codec codec) {
                if (dataFormatDelegate == null) throw new ArgumentNullException("dataFormatDelegate");
                _dataFormatDelegate = dataFormatDelegate;
                if (codec == null) codec = new UTF8Text();
                _codec = codec;
            }

            public bool Load(string absoluteFilePath, out string data) {
                data = null;
                if (string.IsNullOrEmpty(absoluteFilePath)) return false;
                if (!File.Exists(absoluteFilePath)) return false;
                try {
                    switch (_dataFormatDelegate()) {
                        case DataFormat.Binary: {
                                byte[] bytes = File.ReadAllBytes(absoluteFilePath);
                                data = _codec.Decode(bytes);
                                return bytes != null && bytes.Length > 0;
                            }
                        case DataFormat.Text: {
                                data = File.ReadAllText(absoluteFilePath);
                                return !string.IsNullOrEmpty(data);
                            }
                        default:
                            throw new NotImplementedException();
                    }
                } catch (Exception ex) {
                    UnityEngine.Debug.LogError(ex);
                    return false;
                }
            }

            public bool Save(string absoluteFilePath, string data) {
                if (string.IsNullOrEmpty(absoluteFilePath)) return false;
                try {
                    if (!Directory.Exists(Path.GetDirectoryName(absoluteFilePath))) {
                        Directory.CreateDirectory(Path.GetDirectoryName(absoluteFilePath));
                    }
                    switch(_dataFormatDelegate()) {
                        case DataFormat.Binary:
                            File.WriteAllBytes(absoluteFilePath, _codec.Encode(data));
                            break;
                        case DataFormat.Text:
                            File.WriteAllText(absoluteFilePath, data);
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                    return true;
                } catch (Exception ex) {
                    UnityEngine.Debug.LogError(ex);
                    return false;
                }
            }

            public bool Clear(string absoluteFilePath) {
                if (string.IsNullOrEmpty(absoluteFilePath)) return false;
                try {
                    if (File.Exists(absoluteFilePath)) {
                        File.Delete(absoluteFilePath);
#if UNITY_EDITOR
                        UnityEngine.Debug.Log(logPrefix + thisScriptName + ": Deleted " + absoluteFilePath);
#endif
                        return true;
                    }
                } catch (Exception ex) {
                    UnityEngine.Debug.LogError(ex);
                }
                return false;
            }
        }

        /// <summary>
        /// Data encode / decode.
        /// </summary>
        private abstract class Codec {

            /// <summary>
            /// Encode string to bytes.
            /// </summary>
            /// <param name="string">Input string.</param>
            /// <returns>Encoded bytes.</returns>
            public abstract byte[] Encode(string @string);

            /// <summary>
            /// Decode bytes to string.
            /// </summary>
            /// <param name="data">Input bytes.</param>
            /// <returns>Decoded string.</returns>
            public abstract string Decode(byte[] data);
        }

        /// <summary>
        /// Default UTF8 Text codec.
        /// </summary>
        private sealed class UTF8Text : Codec {

            /// <summary>
            /// Encode string to bytes.
            /// </summary>
            /// <param name="string">Input string.</param>
            /// <returns>Encoded bytes.</returns>
            public override byte[] Encode(string @string) {
                return Encoding.UTF8.GetBytes(@string);
            }

            /// <summary>
            /// Decode bytes to string.
            /// </summary>
            /// <param name="data">Input bytes.</param>
            /// <returns>Decoded string.</returns>
            public override string Decode(byte[] data) {
                return Encoding.UTF8.GetString(data);
            }
        }

        /// <summary>
        /// LZF compression codec.
        /// </summary>
        private sealed class CLZF2 : Codec {

            private readonly Rewired.Utils.Libraries.CLZF2.CLZF2 _cLZF2;

            public CLZF2() {
                _cLZF2 = new Rewired.Utils.Libraries.CLZF2.CLZF2();
            }

            /// <summary>
            /// Encode string to bytes.
            /// </summary>
            /// <param name="string">Input string.</param>
            /// <returns>Encoded bytes.</returns>
            public override byte[] Encode(string @string) {
                return _cLZF2.Compress(Encoding.UTF8.GetBytes(@string));
            }

            /// <summary>
            /// Decode bytes to string.
            /// </summary>
            /// <param name="data">Input bytes.</param>
            /// <returns>Decoded string.</returns>
            public override string Decode(byte[] data) {
                return Encoding.UTF8.GetString(_cLZF2.Decompress(data));
            }
        }

        /// <summary>
        /// Provides handlers for data-related functions.
        /// </summary>
        public interface IDataHandler {

            /// <summary>
            /// Load data.
            /// </summary>
            /// <param name="absoluteFilePath">The absolute path to the file.</param>
            /// <param name="data">The loaded data.</param>
            /// <returns>True on success, false on failure.</returns>
            bool Load(string absoluteFilePath, out string data);

            /// <summary>
            /// Save data.
            /// </summary>
            /// <param name="absoluteFilePath">The absolute path to the file.</param>
            /// <param name="data">The data to save.</param>
            /// <returns>True on success, false on failure.</returns>
            bool Save(string absoluteFilePath, string data);

            /// <summary>
            /// Clear data.
            /// </summary>
            /// <param name="absoluteFilePath">The absolute path to the file.</param>
            /// <returns>True on success, false on failure.</returns>
            bool Clear(string absoluteFilePath);
        }

        /// <summary>
        /// Data format.
        /// </summary>
        public enum DataFormat {
            /// <summary>
            /// Text.
            /// </summary>
            Text,
            /// <summary>
            /// Binary.
            /// </summary>
            Binary
        }

        #endregion
    }
}
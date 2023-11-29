using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    //static string characterSavePath = Application.persistentDataPath + "/character.save";
    private static string contactsSavePath = Application.persistentDataPath + "/contacts.save";

    public static void SaveCharacter(Character character)
    { 
        string characterSavePath = Application.persistentDataPath + "/" + character.CharacterName()  + "/character.save";
        // Create the directory structure if it doesn't exist
        string directoryPath = Path.GetDirectoryName(characterSavePath);
        Directory.CreateDirectory(directoryPath);
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(characterSavePath, FileMode.Create);
        CharacterData characterData = new CharacterData(character);
        formatter.Serialize(stream, characterData);
        stream.Close();
    }

    public static CharacterData LoadCharacter(string name)
    {
        string characterSavePath = Application.persistentDataPath + "/" + name + "/character.save";
        return Load(characterSavePath);
    }

    private static CharacterData Load(string characterSavePath)
    {
        if(File.Exists(characterSavePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(characterSavePath, FileMode.Open);
            CharacterData data = formatter.Deserialize(stream) as CharacterData;
            stream.Close();
            return data;
        } else
        {
            Debug.LogError("Save file not found in" + characterSavePath);
            return null;
        }
    }

    public static void SaveContactsList(HashSet<string> contacts)
    {
        string directoryPath = Path.GetDirectoryName(contactsSavePath);
        Directory.CreateDirectory(directoryPath);
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(contactsSavePath, FileMode.Create);
        formatter.Serialize(stream, contacts);
        stream.Close();
    }

    public static HashSet<string> LoadContactsList()
    {
        if (File.Exists(contactsSavePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(contactsSavePath, FileMode.Open);
            HashSet<string> data = formatter.Deserialize(stream) as HashSet<string>;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in" + contactsSavePath);
            return null;
        }
    }

    //public static CharacterData LoadCharacter()
    //{
    //    string characterSavePath = Application.persistentDataPath + "/character.save";
    //    return Load(characterSavePath);
    //}
}

using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    //static string characterSavePath = Application.persistentDataPath + "/character.save";

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

    //public static CharacterData LoadCharacter()
    //{
    //    string characterSavePath = Application.persistentDataPath + "/character.save";
    //    return Load(characterSavePath);
    //}
}

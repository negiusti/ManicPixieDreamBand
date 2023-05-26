using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    //static string playerSavePath = Application.persistentDataPath + "/player.save";

    public static void SavePlayer(Player player)
    { 
        string playerSavePath = Application.persistentDataPath + "/" + player.PlayerName()  + "/player.save";
        // Create the directory structure if it doesn't exist
        string directoryPath = Path.GetDirectoryName(playerSavePath);
        Directory.CreateDirectory(directoryPath);
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(playerSavePath, FileMode.Create);
        PlayerData playerData = new PlayerData(player);
        formatter.Serialize(stream, playerData);
        stream.Close();
    }

    public static PlayerData LoadPlayer(string name)
    {
        string playerSavePath = Application.persistentDataPath + "/" + name + "/player.save";
        return Load(playerSavePath);
    }

    private static PlayerData Load(string playerSavePath)
    {
        if(File.Exists(playerSavePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(playerSavePath, FileMode.Open);
            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();
            return data;
        } else
        {
            Debug.LogError("Save file not found in" + playerSavePath);
            return null;
        }
    }

    public static PlayerData LoadPlayer()
    {
        string playerSavePath = Application.persistentDataPath + "/player.save";
        return Load(playerSavePath);
    }
}

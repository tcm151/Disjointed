using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace OGAM.Tools
{
    public static class SaveSystem
    {
        public static void SavePlayer(GameObject player)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/PlayerData";
            FileStream stream = new FileStream(path, FileMode.Create);

            PlayerData playerData = new PlayerData(player);

            formatter.Serialize(stream, playerData);
            stream.Close();
        }

        public static PlayerData LoadPlayer()
        {
            string path = Application.persistentDataPath + "/PlayerData";
            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);

                PlayerData playerData = formatter.Deserialize(stream) as PlayerData;
                stream.Close();
                return playerData;
            }
            else
            {
                Debug.LogError("Save file not found in " + path);
                return null;
            }
        }
    }
}

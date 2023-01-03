using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace BumperBallGame
{
    public static class SaveSystem
    {
        public static void SaveConfig(ConfigData data)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/config.data";
            FileStream stream = new FileStream(path, FileMode.Create);

            formatter.Serialize(stream, data);
            stream.Close();
        }

        public static ConfigData LoadConfig()
        {
            string path = Application.persistentDataPath + "/config.data";
            if (!File.Exists(path))
            {
                Debug.Log("Save file not found in " + path);
                return null;
            }
            else
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);
                ConfigData data = formatter.Deserialize(stream) as ConfigData;
                stream.Close();
                return data;
            }
        }
    }
}
using System.IO;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

namespace SaveSystem
{
    public static class LocalDiskSaveManager
    {
        public static void Save<T>(T data, string path)
        {
            var saveData = JsonConvert.SerializeObject(data);

            File.WriteAllText(path, saveData, Encoding.UTF8);
            File.ReadAllText(path,Encoding.UTF8);
        }

        public static T Load<T>(string path)
        {
            if (!File.Exists(path)) return (T) default;

            var content = File.ReadAllText(path, Encoding.UTF8);
            var obj = JsonConvert.DeserializeObject<T>(content);
            return obj;
        }

    }
}
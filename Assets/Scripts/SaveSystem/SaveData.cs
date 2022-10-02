using UnityEngine;

namespace SaveSystem
{
    public abstract class SaveData
    {
        public string GetSaveDataPath()
        {
            return Application.persistentDataPath + FolderName();
        }

        protected virtual string FolderName() => "/SaveFile.data";
    }
}
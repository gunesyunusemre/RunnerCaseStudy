using UnityEditor;

namespace SaveSystem
{
    public static class SaveDataHelper
    {
        public static GameSaveData GameSaveData = new GameSaveData();

        public static void LoadAll()
        {
            var path = GameSaveData.GetSaveDataPath();
            GameSaveData = LocalDiskSaveManager.Load<GameSaveData>(path) ?? GameSaveData;
        }

        public static void SaveAll()
        {
            var path = GameSaveData.GetSaveDataPath();
            LocalDiskSaveManager.Save(GameSaveData, path);
        }

#if UNITY_EDITOR
        [MenuItem("Game/Reset Save")]
        public static void ResetSave()
        {
            GameSaveData = new GameSaveData();
            
            
            var path = GameSaveData.GetSaveDataPath();
            LocalDiskSaveManager.Save(GameSaveData, path);
        }
#endif
    }
}
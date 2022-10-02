namespace SaveSystem
{
    public class GameSaveData : SaveData
    {
        public int LevelIndex = 0;
        
        
        
        
        
        
        
        protected override string FolderName()
        {
            return "GameSaveData.data";
        }
    }
}
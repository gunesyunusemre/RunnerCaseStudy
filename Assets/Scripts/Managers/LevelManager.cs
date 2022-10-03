using System;
using System.Collections.Generic;
using Managers.Base;
using NaughtyAttributes;
using SaveSystem;
using UnityEngine;

namespace Managers
{
    public class LevelManager : BaseManager
    {
        [SerializeField] private List<Level> levelList;
        [SerializeField] private List<Level> randomLevelList;
        
        [SerializeField] private bool isSpecificLevel;
        [SerializeField][ShowIf("isSpecificLevel")][OnValueChanged("ChangeSpecificIndex")] 
        private int specificLevelNumber;

        private Level _currentLevel;
        
        
        
        private readonly LevelManagerEvents managerEvents = new LevelManagerEvents();
        
        public override void Init()
        {
            CreateLevel();

            managerEvents.OnNextLevel += NextLevel;
            managerEvents.OnRetryLevel += RetryLevel;
        }

        public override Type GetEvents(out BaseManagerEvents instance)
        {
            instance = managerEvents;
            return typeof(LevelManagerEvents);
        }

        public void RetryLevel()
        {
            CreateLevel();
        }

        public void NextLevel()
        {
            SaveDataHelper.GameSaveData.LevelIndex++;
            SaveDataHelper.SaveAll();

            CreateLevel();
        }

        private Level CreateLevel()
        {
            Level levelBlueprint;
            var levelIndex = SaveDataHelper.GameSaveData.LevelIndex;
            if (CheckRandom())
            {
                //Random
                var randomIndex = levelIndex - levelList.Count;
                if (randomIndex >= randomLevelList.Count)
                    randomIndex %= randomLevelList.Count;

                levelBlueprint = randomLevelList[randomIndex];
            }
            else
            {
                //Normal
                levelBlueprint = levelList[levelIndex];
            }

            if (isSpecificLevel)
            {
                levelBlueprint = levelList[specificLevelNumber];
            }

            if (_currentLevel)
                Destroy(_currentLevel);

            var instance = Instantiate(levelBlueprint);
            instance.gameObject.name += " LevelNumber " + (levelIndex +1);
            _currentLevel = instance;
            _currentLevel.Construct(managerEvents);
            return instance;
        }

        private bool CheckRandom()
        {
            var levelIndex = SaveDataHelper.GameSaveData.LevelIndex;
            if (levelIndex >= levelList.Count)
                return true;

            return false;
        }

        private void ChangeSpecificIndex()
        {
            if (specificLevelNumber < 0)
                specificLevelNumber = 0;

            if (specificLevelNumber >= levelList.Count)
                specificLevelNumber = levelList.Count - 1;
        }
    }
}
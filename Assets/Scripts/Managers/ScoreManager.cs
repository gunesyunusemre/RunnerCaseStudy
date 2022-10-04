using System;
using Dreamteck.Splines;
using Helpers;
using Managers.Base;
using SaveSystem;
using UnityEngine;

namespace Managers
{
    public class ScoreManager : BaseManager
    {
        private float _currentScore;
        private float _levelScore;
        
        private readonly ScoreManagerEvents managerEvents = new ScoreManagerEvents();
        private UIManagerEvents uiManagerEvents;
        public override void Init()
        {
            _currentScore = SaveDataHelper.GameSaveData.Score;
            
            managerEvents.OnDecreaseScore+= OnDecreaseScore;
            managerEvents.OnIncreaseScore += OnIncreaseScore;
            RegisterUIManagerEvents();
        }

        public override Type GetEvents(out BaseManagerEvents instance)
        {
            instance = managerEvents;
            return typeof(ScoreManagerEvents);
        }

        private void OnEnable()
        {
            var id = GetInstanceID();
            managerEvents.FireOnEnable(id);
        }

        private void OnDisable()
        {
            var id = GetInstanceID();
            managerEvents.FireOnDisable(id);
        }
        private void RegisterUIManagerEvents()
        {
            var checkLevelManagerEvents = ManagerEventsHelper.TryGetManagerEvents(out uiManagerEvents);
            if (!checkLevelManagerEvents)
            {
                "Level manager event can not found".Log();
                return;
            }
            
            uiManagerEvents.OnClaimLevelScore += OnClaimLevelScore;
        }

        private void OnClaimLevelScore()
        {
            _currentScore += _levelScore;
            managerEvents.FireOnChangeScore(_currentScore);
            _levelScore = 0;

            SaveDataHelper.GameSaveData.Score = _currentScore;
            SaveDataHelper.SaveAll();
        }


        private void OnDecreaseScore(float decreaseValue)
        {
            //_currentScore -= decreaseValue;
            _levelScore -= decreaseValue;
            //managerEvents.FireOnChangeScore(_currentScore);
            managerEvents.FireOnChangeLevelScore(_levelScore);
        }

        private void OnIncreaseScore(float increaseValue)
        {
            //_currentScore += increaseValue;
            _levelScore += increaseValue;
            //managerEvents.FireOnChangeScore(_currentScore);
            managerEvents.FireOnChangeLevelScore(_levelScore);
        }
    }
}
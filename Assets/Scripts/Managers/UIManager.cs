using System;
using Helpers;
using Managers.Base;
using UnityEngine;

namespace Managers
{
    public class UIManager : BaseManager
    {
        [SerializeField] private GameObject tapToStart;
        [SerializeField] private GameObject levelWinPanel;


        private readonly UIManagerEvents managerEvents = new UIManagerEvents();
        private BasicInputManagerEvents inputManagerEvents;
        private LevelManagerEvents levelManagerEvents;
        private bool _isStarted = false;


        public override void Init()
        {
            ManagerID = GetInstanceID();
            RegisterInputManagerEvents();
            RegisterLevelManagerEvents();
        }

        public override Type GetEvents(out BaseManagerEvents instance)
        {
            instance = managerEvents;
            return typeof(UIManagerEvents);
        }

        private void OnEnable()
        {
            managerEvents.FireOnEnable(ManagerID);
        }

        private void OnDisable()
        {
            managerEvents.FireOnDisable(ManagerID);
            UnregisterInputManagerEvents(0);
        }

        private void RegisterLevelManagerEvents()
        {
            var checkLevelManagerEvents = ManagerEventsHelper.TryGetManagerEvents(out levelManagerEvents);
            if (!checkLevelManagerEvents)
            {
                "Level manager event can not found".Log();
                return;
            }
            
            levelManagerEvents.OnLevelFinish += OnLevelFinish;
        }

       
        private void RegisterInputManagerEvents()
        {
            var checkInputManagerEvents = ManagerEventsHelper.TryGetManagerEvents(out inputManagerEvents);
            if (!checkInputManagerEvents)
            {
                "Input manager event can not found".Log();
                return;
            }
            inputManagerEvents.OnStartTouch += OnStartTouch;
            inputManagerEvents.OnDisable += UnregisterInputManagerEvents;
        }
        
        private void UnregisterInputManagerEvents(int id)
        {
            var checkInputManagerEvents = ManagerEventsHelper.TryGetManagerEvents(out inputManagerEvents);
            if (!checkInputManagerEvents)
                return;
            
            inputManagerEvents.OnStartTouch -= OnStartTouch;
            inputManagerEvents.OnDisable -= UnregisterInputManagerEvents;
        }

        private void OnStartTouch(Vector3 arg0)
        {
            if (_isStarted)
                return;

            _isStarted = true;
            managerEvents.FireOnTapToPlay();
            tapToStart.SetActive(false);
        }
        
        private void OnLevelFinish()
        {
            levelWinPanel.SetActive(true);
        }

    }
}
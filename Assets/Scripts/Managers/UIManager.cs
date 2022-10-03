using System;
using DG.Tweening;
using Helpers;
using Managers.Base;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class UIManager : BaseManager
    {
        [SerializeField] private GameObject tapToStart;
        [SerializeField] private GameObject levelWinPanel;
        [SerializeField] private Button winClaimButton;
        [SerializeField] private CanvasGroup fader;
        
        


        private readonly UIManagerEvents managerEvents = new UIManagerEvents();
        private BasicInputManagerEvents inputManagerEvents;
        private LevelManagerEvents levelManagerEvents;
        private bool _isStarted = false;


        public override void Init()
        {
            winClaimButton.onClick.AddListener(WinClaimClick);
            
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
        
        private void WinClaimClick()
        {
            _isStarted = false;
            levelWinPanel.SetActive(false);

            fader.gameObject.SetActive(true);
            DOVirtual.Float(0f, 1f, .5f, value =>
            {
                fader.alpha = value;
            }).OnKill(() =>
            {
                levelManagerEvents.FireOnNextLevel();
                DOVirtual.Float(1f, 0f, .5f, value =>
                {
                    fader.alpha = value;
                }).OnKill(()=>fader.gameObject.SetActive(true));
            });
        }

    }
}
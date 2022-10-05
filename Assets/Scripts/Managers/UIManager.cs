using System;
using System.Globalization;
using DG.Tweening;
using Helpers;
using Managers.Base;
using SaveSystem;
using TMPro;
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
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text levelScoreText;
        [SerializeField] private TMP_Text levelText;
        
        

        private readonly UIManagerEvents managerEvents = new UIManagerEvents();
        private BasicInputManagerEvents inputManagerEvents;
        private LevelManagerEvents levelManagerEvents;
        private ScoreManagerEvents scoreManagerEvents;
        private bool _isStarted = false;
        
        public override void Init()
        {
            winClaimButton.onClick.AddListener(WinClaimClick);

            var loadScore = SaveDataHelper.GameSaveData.Score;
            ScoreManagerEventsOnChangeScore(loadScore);
            var currentLevel = SaveDataHelper.GameSaveData.LevelIndex + 1;
            WriteLevelNumber(currentLevel);
            
            ManagerID = GetInstanceID();
            RegisterInputManagerEvents();
            RegisterLevelManagerEvents();
            RegisterScoreManagerEvents();
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
        
        private void RegisterScoreManagerEvents()
        {
            var checkScoreManagerEvents = ManagerEventsHelper.TryGetManagerEvents(out scoreManagerEvents);
            if (!checkScoreManagerEvents)
            {
                "Score manager event can not found".Log();
                return;
            }
            
            scoreManagerEvents.OnChangeScore += ScoreManagerEventsOnChangeScore;
            scoreManagerEvents.OnChangeLevelScore += ScoreManagerEventsOnChangeLevelScore;
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
            winClaimButton.interactable = false;
            managerEvents.FireOnClaimLevelScore();
            WinFader();
            //Invoke(nameof(WinFader), 1f);
        }

        private void WinFader()
        {
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
                }).OnKill(() =>
                {
                    winClaimButton.interactable = true;

                    _isStarted = false;
                    fader.gameObject.SetActive(false);
                    tapToStart.SetActive(true);
                    
                    var currentLevel = SaveDataHelper.GameSaveData.LevelIndex + 1;
                    WriteLevelNumber(currentLevel);
                });
            });
        }
        
        private void ScoreManagerEventsOnChangeScore(float score)
        {
            scoreText.text = score.ToString(CultureInfo.InvariantCulture);
        }
        
        private void ScoreManagerEventsOnChangeLevelScore(float levelScore)
        {
            levelScoreText.text = levelScore.ToString(CultureInfo.InvariantCulture);
        }

        private void WriteLevelNumber(int currentLevel)
        {
            levelText.text = "Level " + currentLevel;
        }

    }
}
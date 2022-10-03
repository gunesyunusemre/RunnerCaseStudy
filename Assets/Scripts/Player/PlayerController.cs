using System;
using Dreamteck.Splines;
using Helpers;
using Managers;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerContainer playerContainer;

        private LevelManagerEvents levelManagerEvents;

        private void Awake()
        {
            playerContainer.OnLevelFinish += PlayerContainerOnLevelFinish;
            
            var checkEvents =
                ManagerEventsHelper.TryGetManagerEvents(out levelManagerEvents);
            if (checkEvents)
            {
                levelManagerEvents.OnLevelStarted += LevelManagerEventsOnLevelStarted;
            }
        }

        private void OnEnable()
        {
            var id = gameObject.GetInstanceID();
            this.SetPlayerContainer(playerContainer, id);
        }

        private void Update()
        {
            playerContainer.FireOnUpdate();
        }

        private void PlayerContainerOnLevelFinish()
        {
            levelManagerEvents.FireOnLevelFinish();
        }
        
        private void LevelManagerEventsOnLevelStarted(SplineComputer computer, float distance)
        {
            playerContainer.FireOnStart(computer, distance);
        }
    }
}
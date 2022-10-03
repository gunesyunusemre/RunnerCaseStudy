using System;
using Managers;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerContainer playerContainer;

        private LevelManagerEvents levelManagerEvents;
        private void Start()
        {
            playerContainer.OnLevelFinish += OnLevelFinish;
            
            var checkEvents =
                ManagerEventsHelper.TryGetManagerEvents(out levelManagerEvents);
        }

        private void OnEnable()
        {
            var id = gameObject.GetInstanceID();
            this.SetPlayerContainer(playerContainer, id);
        }

        private void OnLevelFinish()
        {
            levelManagerEvents.FireOnLevelFinish();
        }
    }
}
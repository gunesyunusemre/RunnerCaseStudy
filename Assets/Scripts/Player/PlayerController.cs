using Cinemachine;
using Dreamteck.Splines;
using Helpers;
using Managers;
using NaughtyAttributes;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerContainer playerContainer;
        [SerializeField] private CinemachineImpulseSource impulseSource;
        

        private LevelManagerEvents levelManagerEvents;

        private void Awake()
        {
            playerContainer.OnLevelFinish += PlayerContainerOnLevelFinish;
            playerContainer.OnTakeDamage += PlayerContainerOnTakeDamage;
            
            var checkEvents =
                ManagerEventsHelper.TryGetManagerEvents(out levelManagerEvents);
            if (checkEvents)
            {
                levelManagerEvents.OnLevelStarted += LevelManagerEventsOnLevelStarted;
                levelManagerEvents.OnNextLevel += LevelManagerEventsOnNextLevel;
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
            PlayFx.Finish();
            levelManagerEvents.FireOnLevelFinish();
        }
        
        [Button()]
        private void PlayerContainerOnTakeDamage()
        {
            PlayFx.RemoveStack();
            impulseSource.GenerateImpulse(5);
        }

        private void LevelManagerEventsOnLevelStarted(SplineComputer computer, float distance)
        {
            playerContainer.FireOnStart(computer, distance);
        }

        private void LevelManagerEventsOnNextLevel()
        {
            playerContainer.FireOnNextLevel();
        }
    }
}
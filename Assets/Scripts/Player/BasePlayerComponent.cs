using System;
using Dreamteck.Splines;
using Helpers;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerController))]
    public class BasePlayerComponent : MonoBehaviour
    {
        protected PlayerContainer PlayerContainer;

        protected virtual void Start()
        {
            var id = gameObject.GetInstanceID();
            var checkPlayerContainer  = PlayerHelper.TryGetPlayerContainer(id, out PlayerContainer);
            if (checkPlayerContainer)
            {
                PlayerContainer.OnLevelFinish += OnLevelFinish;
                PlayerContainer.OnLevelStart += OnLevelStart;
                PlayerContainer.OnUpdate += OnUpdate;
            }
        }

        protected virtual void OnUpdate()
        {
        }

        protected virtual void OnLevelStart(SplineComputer computer, float distance)
        {
            
        }

        protected virtual void OnLevelFinish()
        {
            
        }
    }
}
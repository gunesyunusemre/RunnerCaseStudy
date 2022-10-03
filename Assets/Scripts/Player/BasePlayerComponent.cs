using System;
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
            }
        }
        
        protected virtual void OnLevelFinish()
        {
            
        }
    }
}
using System;
using Player;
using UnityEngine;

namespace Interactable
{
    public class FinishZone : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            var id = other.gameObject.GetInstanceID();
            var checkPlayer = PlayerHelper.TryGetPlayerContainer(id, out var playerContainer);
            if (checkPlayer == false)
                return;
            
            playerContainer.FireOnFinish();
        }
    }
}
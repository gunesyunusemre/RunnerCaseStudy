using System;
using Helpers;
using Player;
using Stacks.Instance;
using UnityEngine;

namespace Interactable
{
    public class Obstacle : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            CheckStackController(other);

            CheckPlayer(other);
        }

        private static void CheckStackController(Collider other)
        {
            var id = other.gameObject.GetInstanceID();
            if (!StackControllerInstanceHelper.TryGetInstance(id, out IStackControllerInstance stackControllerInstance))
                return;

            var checkStack = stackControllerInstance.TryRequestStack(out var stackInstance);
            if (checkStack)
            {
                //Here stack force
                stackInstance.DestroyYourself();
            }
        }

        private static void CheckPlayer(Collider other)
        {
            var id = other.gameObject.GetInstanceID();
            var checkPlayer = PlayerHelper.TryGetPlayerContainer(id, out var playerContainer);
            if (checkPlayer == false)
                return;

            playerContainer.FireOnChangeFriction(200);
        }
    }
}
using Dreamteck.Splines;
using NaughtyAttributes;
using Player;
using Stacks.Instance;
using UnityEngine;

namespace Interactable
{
    public class Obstacle : MonoBehaviour
    {
        [SerializeField] private float frictionPercent = 200;
        [SerializeField][Range(-3.5f, 3.5f)] private float xPosition;
        [SerializeField, ReadOnly] private SplinePositioner positioner;


        private void OnValidate()
        {
            if (positioner == null)
                positioner = GetComponent<SplinePositioner>();

            if (positioner == null)
                return;

            var offset = positioner.motion.offset;
            offset.x = xPosition;
            positioner.motion.offset = offset;
        }

        private void OnTriggerEnter(Collider other)
        {
            CheckStackController(other);

            CheckPlayer(other);
        }

        private void CheckStackController(Collider other)
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

        private void CheckPlayer(Collider other)
        {
            var id = other.gameObject.GetInstanceID();
            var checkPlayer = PlayerHelper.TryGetPlayerContainer(id, out var playerContainer);
            if (checkPlayer == false)
                return;

            playerContainer.FireOnChangeFriction(frictionPercent);
        }
    }
}
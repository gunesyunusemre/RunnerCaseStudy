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
        [SerializeField] private float yOffset;
        
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
            offset.y = yOffset;
            positioner.motion.offset = offset;
        }

        private void OnTriggerEnter(Collider other)
        {
            var canAddFriction = CheckStackController(other);
            if (canAddFriction)
                CheckPlayer(other);
        }

        private bool CheckStackController(Collider other)
        {
            var id = other.gameObject.GetInstanceID();
            if (!StackControllerInstanceHelper.TryGetInstance(id, out IStackControllerInstance stackControllerInstance))
                return false;
            
            var checkStack = stackControllerInstance.CheckStack();
            stackControllerInstance.BreakStack();
            return checkStack;
            /*var checkStack = stackControllerInstance.TryRequestStack(out var stackInstance);
            if (checkStack)
            {
                stackInstance.DestroyYourself();
                stackControllerInstance.BreakStack();
                return true;
            }
            return false;*/
        }

        private void CheckPlayer(Collider other)
        {
            var id = other.gameObject.GetInstanceID();
            var checkPlayer = PlayerHelper.TryGetPlayerContainer(id, out var playerContainer);
            if (checkPlayer == false)
                return;

            playerContainer.FireOnChangeFriction(frictionPercent);
            playerContainer.FireOnTakeDamage();
        }
    }
}
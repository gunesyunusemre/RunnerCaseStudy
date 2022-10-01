using Stacks.Instance;
using UnityEngine;
using UnityEngine.Events;

namespace Stacks
{
    [CreateAssetMenu(fileName = "StackContainer", menuName = "Interactable/Stack Container", order = 0)]
    public class StackContainer : ScriptableObject
    {
        [SerializeField] private float rotDamping = .02f;
        [SerializeField] private float damping = .02f;
        [SerializeField] private float height;

        public float RotDamping => rotDamping;
        public float Damping => damping;


        public float GetHeight()
        {
            return height;
        }
        
        
        public event UnityAction<IStackControllerInstance> OnBeforeCollect;
        public void FireOnBeforeCollect(IStackControllerInstance controllerInstance) => OnBeforeCollect?.Invoke(controllerInstance);
        
        public event UnityAction<Transform, int, Transform> OnCollect;
        public void FireOnCollect(Transform followTarget, int index, Transform followParent) => OnCollect?.Invoke(followTarget, index, followParent);
    }
}
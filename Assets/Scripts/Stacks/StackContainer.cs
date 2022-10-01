using Stacks.Instance;
using UnityEngine;
using UnityEngine.Events;

namespace Stacks
{
    [CreateAssetMenu(fileName = "StackContainer", menuName = "Interactable/Stack Container", order = 0)]
    public class StackContainer : ScriptableObject
    {
        [SerializeField] private float rotDamping = .02f;
        [SerializeField] private float minDamping = .003f;
        [SerializeField] private float maxDamping = .085f;
        [SerializeField] private int maxStackCount = 15;
        [SerializeField] private float height;

        public float RotDamping => rotDamping;
        public float MaxDamping => maxDamping;
        public float MinDamping => minDamping;
        public float MaxStackCount => maxStackCount;


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
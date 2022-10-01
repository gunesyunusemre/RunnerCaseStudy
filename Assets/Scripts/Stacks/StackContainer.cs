using Stacks.Instance;
using UnityEngine;
using UnityEngine.Events;

namespace Stacks
{
    [CreateAssetMenu(fileName = "StackContainer", menuName = "Interactable/Stack Container", order = 0)]
    public class StackContainer : ScriptableObject
    {
        [SerializeField] private float maxSpeed = 200f;
        [SerializeField] private float damping = 10f;
        [SerializeField] private float tolerance = .1f;
        [SerializeField] private float height;

        public float MaxSpeed => maxSpeed;
        public float Damping => damping;
        public float Tolerance => tolerance;


        public float GetHeight()
        {
            return height;
        }
        
        
        public event UnityAction<IStackControllerInstance> OnBeforeCollect;
        public void FireOnBeforeCollect(IStackControllerInstance controllerInstance) => OnBeforeCollect?.Invoke(controllerInstance);
        
        public event UnityAction<Transform, bool> OnCollect;
        public void FireOnCollect(Transform followTarget, bool IsFirst) => OnCollect?.Invoke(followTarget, IsFirst);
    }
}
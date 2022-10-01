using System;
using Helpers;
using InstanceSystem;
using Stacks.Instance;
using UnityEngine;

namespace Stacks
{
    public class Stack : MonoBehaviour, IStackInstance
    {
        public int InstanceID { get; private set; }
        public StackContainer container { get; set; }

        private Transform _transform;

        private void OnEnable()
        {
            _transform = transform;
            InstanceID = gameObject.GetInstanceID();
            this.AddInstance();
        }

        private void Start()
        {
            container.OnBeforeCollect += OnBeforeCollect;
            container.OnCollect += OnCollect;
        }

        private void OnDisable()
        {
            this.RemoveInstance();
        }

        public void DestroyYourself()
        {
            Destroy(gameObject);
        }

        public Transform GetTransform() => _transform;
        
        private void OnBeforeCollect(IStackControllerInstance stackController)
        {
            stackController.AddStack(this);
        }

        private void OnCollect(Transform followTarget, bool isFirst)
        {
            var followComponent = gameObject.AddComponent<StackFollowComponent>();
            followComponent.Construct(container, followTarget, isFirst);
        }
    }
}

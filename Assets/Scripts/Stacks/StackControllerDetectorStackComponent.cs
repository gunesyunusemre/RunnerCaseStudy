using System;
using Helpers;
using Stacks.Instance;
using UnityEngine;

namespace Stacks
{
    public class StackControllerDetectorStackComponent : StackBaseComponent
    {
        private StackContainer container;

        private void Start()
        {
            var id = gameObject.GetInstanceID();
            if (!StackInstanceHelper.TryGetInstance(id, out IStackInstance stackInstance))
            {
                "I cannot find my stack container".Log();
                return;
            }

            container = stackInstance.container;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (container==null)
                return;

            var id = other.gameObject.GetInstanceID();
            if (!StackControllerInstanceHelper.TryGetInstance(id, out IStackControllerInstance stackControllerInstance))
                return;
            
            container.FireOnBeforeCollect(stackControllerInstance);
            Destroy(this);
        }
    }
}
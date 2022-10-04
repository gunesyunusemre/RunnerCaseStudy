using System.Collections.Generic;
using Helpers;
using Stacks.Instance;
using UnityEngine;

namespace Stacks
{
    public class StackMeshControllerComponent : StackBaseComponent
    {
        [SerializeField] private List<GameObject> meshList;
        
        
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

            for (int i = 0; i < meshList.Count; i++)
            {
                meshList[i].SetActive(i == container.StackType);
            }
        }
        
    }
}
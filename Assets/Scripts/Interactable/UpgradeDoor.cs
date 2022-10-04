using Stacks.Instance;
using UnityEngine;

namespace Interactable
{
    public class UpgradeDoor : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            var id = other.gameObject.GetInstanceID();
            if (!StackControllerInstanceHelper.TryGetInstance(id, out IStackControllerInstance stackControllerInstance))
                return;
            
            stackControllerInstance.Upgrade();
        }
    }
}
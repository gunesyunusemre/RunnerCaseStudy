using System.Collections.Generic;

namespace Stacks.Instance
{
    public static class StackControllerInstanceHelper
    {
        public static readonly Dictionary<int, IStackControllerInstance> InstanceDict = new Dictionary<int, IStackControllerInstance>();

        public static void AddInstance(this IStackControllerInstance instance)
        {
            if (InstanceDict.ContainsKey(instance.InstanceID))
                return;
        
            InstanceDict.Add(instance.InstanceID, instance);
        }
    
        public static void RemoveInstance(this IStackControllerInstance instance)
        {
            if (!InstanceDict.ContainsKey(instance.InstanceID))
                return;
        
            InstanceDict.Remove(instance.InstanceID);
        }

        public static bool TryGetInstance(int id, out IStackControllerInstance target)
        {
            target = default;
            var checkInstance = InstanceDict.ContainsKey(id);
            if (checkInstance)
                target = InstanceDict[id];

            return checkInstance;
        }
    
        public static void ClearAll()
        {
            InstanceDict.Clear();
        }
    }
}
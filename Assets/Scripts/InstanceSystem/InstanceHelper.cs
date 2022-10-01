using System.Collections.Generic;
using Stacks;

namespace InstanceSystem
{
    public static class InstanceHelper
    {
        public static Dictionary<int, IInstance> InstanceDict = new Dictionary<int, IInstance>();

        public static void AddInstance(IInstance instance)
        {
            if (InstanceDict.ContainsKey(instance.InstanceID))
                return;
        
            InstanceDict.Add(instance.InstanceID, instance);
        }
    
        public static void RemoveInstance(IInstance instance)
        {
            if (!InstanceDict.ContainsKey(instance.InstanceID))
                return;
        
            InstanceDict.Remove(instance.InstanceID);
        }

        public static bool TryGetInstance(int id, out IInstance target)
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
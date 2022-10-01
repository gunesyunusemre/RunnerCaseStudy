using System.Collections.Generic;

namespace Stacks.Instance
{
    public static class StackInstanceHelper
    {
        public static readonly Dictionary<int, IStackInstance> InstanceDict = new Dictionary<int, IStackInstance>();

        public static void AddInstance(this IStackInstance instance)
        {
            if (InstanceDict.ContainsKey(instance.InstanceID))
                return;
        
            InstanceDict.Add(instance.InstanceID, instance);
            instance.container = GameInstaller.instance.GetStackContainer();
        }
    
        public static void RemoveInstance(this IStackInstance instance)
        {
            if (!InstanceDict.ContainsKey(instance.InstanceID))
                return;
        
            InstanceDict.Remove(instance.InstanceID);
        }

        public static bool TryGetInstance(int id, out IStackInstance target)
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
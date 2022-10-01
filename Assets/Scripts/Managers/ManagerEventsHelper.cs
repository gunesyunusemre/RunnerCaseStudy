using System;
using System.Collections.Generic;

namespace Managers
{
    public static class ManagerEventsHelper
    {
        private static readonly Dictionary<Type, BaseManagerEvents> ManagerEventDict = new Dictionary<Type, BaseManagerEvents>();

        public static void AddManagerEvents(BaseManagerEvents managerEvents, Type type)
        {
            if (ManagerEventDict.ContainsKey(type))
                return;
            
            ManagerEventDict.Add(type, managerEvents);
        }

        public static void RemoveManagerEvents<T>(BaseManagerEvents managerEvents)
        {
            var t = typeof(T);
            if (!ManagerEventDict.ContainsKey(t))
                return;

            ManagerEventDict.Remove(t);
        }
        
        public static bool TryGetManagerEvents<T>(out T managerEvent)
            where T : BaseManagerEvents
        {
            var t = typeof(T);
            managerEvent = default;
            var checkHaveEvents = ManagerEventDict.ContainsKey(t);
            if (!checkHaveEvents)
                return false;
            
            managerEvent = (T)ManagerEventDict[t];
            return true;
        }
    }
}
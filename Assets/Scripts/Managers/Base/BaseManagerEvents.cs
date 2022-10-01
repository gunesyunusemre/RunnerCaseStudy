using Managers.Base;
using UnityEngine.Events;

namespace Managers
{
    public abstract class BaseManagerEvents
    {
        public event UnityAction<int> OnDisable;
        public virtual void FireOnDisable(int managerID) => OnDisable?.Invoke(managerID);
        
        public event UnityAction<int> OnEnable;
        public virtual void FireOnEnable(int managerID) => OnEnable?.Invoke(managerID);
    }
}
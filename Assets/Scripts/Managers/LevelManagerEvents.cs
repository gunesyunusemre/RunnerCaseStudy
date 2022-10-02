using UnityEngine.Events;

namespace Managers
{
    public class LevelManagerEvents : BaseManagerEvents
    {
        public event UnityAction OnNextLevel;
        public void FireOnNextLevel() =>OnNextLevel?.Invoke();
        
        public event UnityAction OnRetryLevel;
        public void FireOnRetryLevel() =>OnRetryLevel?.Invoke();
      
    }
}
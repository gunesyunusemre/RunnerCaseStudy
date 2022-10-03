using UnityEngine.Events;

namespace Managers
{
    public class LevelManagerEvents : BaseManagerEvents
    {
        public event UnityAction OnLevelFinish;
        public void FireOnLevelFinish() =>OnLevelFinish?.Invoke();
        
        public event UnityAction OnNextLevel;
        public void FireOnNextLevel() =>OnNextLevel?.Invoke();
        
        public event UnityAction OnRetryLevel;
        public void FireOnRetryLevel() =>OnRetryLevel?.Invoke();
      
    }
}
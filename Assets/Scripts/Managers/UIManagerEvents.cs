using UnityEngine.Events;

namespace Managers
{
    public class UIManagerEvents : BaseManagerEvents
    {
        public event UnityAction OnTapToPlay;
        public void FireOnTapToPlay() => OnTapToPlay?.Invoke();

        public event UnityAction OnClaimLevelScore;
        public void FireOnClaimLevelScore() => OnClaimLevelScore?.Invoke();
    }
}
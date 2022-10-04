using UnityEngine.Events;

namespace Managers
{
    public class ScoreManagerEvents : BaseManagerEvents
    {
        public event UnityAction<float> OnIncreaseScore;
        public void FireOnIncreaseScore(float increaseValue) =>OnIncreaseScore?.Invoke(increaseValue);
        
        
        public event UnityAction<float> OnDecreaseScore;
        public void FireOnDecreaseScore(float decreaseValue) =>OnDecreaseScore?.Invoke(decreaseValue);
        
        public event UnityAction<float> OnChangeScore;
        public void FireOnChangeScore(float score) =>OnChangeScore?.Invoke(score);
        
        public event UnityAction<float> OnChangeLevelScore;
        public void FireOnChangeLevelScore(float score) =>OnChangeLevelScore?.Invoke(score);
    }
}
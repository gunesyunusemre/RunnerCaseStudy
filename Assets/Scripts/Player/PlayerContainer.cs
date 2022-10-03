using Dreamteck.Splines;
using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    [CreateAssetMenu(fileName = "PlayerContainer", menuName = "Game/Player Container", order = 0)]

    public class PlayerContainer : ScriptableObject
    {
        public event UnityAction OnLevelFinish;
        public void FireOnFinish() => OnLevelFinish?.Invoke();
        
        public event UnityAction<SplineComputer, float> OnLevelStart;
        public void FireOnStart(SplineComputer computer, float distance) => OnLevelStart?.Invoke(computer, distance);
    }
}
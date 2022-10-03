using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    [CreateAssetMenu(fileName = "PlayerContainer", menuName = "Game/Player Container", order = 0)]

    public class PlayerContainer : ScriptableObject
    {
        public event UnityAction OnLevelFinish;
        public void FireOnFinish() => OnLevelFinish?.Invoke();
    }
}
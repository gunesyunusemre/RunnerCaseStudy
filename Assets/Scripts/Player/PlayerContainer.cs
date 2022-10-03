using Dreamteck.Splines;
using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    [CreateAssetMenu(fileName = "PlayerContainer", menuName = "Game/Player Container", order = 0)]

    public class PlayerContainer : ScriptableObject
    {
        [SerializeField] private float minDistance;
        [SerializeField] private Vector2 bound;
        [SerializeField, Range(5f, 160f)] private float swerveSpeed;
        [SerializeField, Range(0.001f, 0.06f)] private float minDistToMove;
        
        public float MinDistance => minDistance;
        public Vector2 Bound => bound;
        public float SwerveSpeed => swerveSpeed;
        public float MinDistToMove => minDistToMove;


        public event UnityAction OnLevelFinish;
        public void FireOnFinish() => OnLevelFinish?.Invoke();
        
        public event UnityAction<SplineComputer, float> OnLevelStart;
        public void FireOnStart(SplineComputer computer, float distance) => OnLevelStart?.Invoke(computer, distance);
    }
}
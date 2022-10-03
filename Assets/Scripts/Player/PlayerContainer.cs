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
        [SerializeField] private float forwardSpeed;
        [SerializeField] private float frictionMoveToZeroSpeed = 5f;
        
        
        
        public float MinDistance => minDistance;
        public Vector2 Bound => bound;
        public float SwerveSpeed => swerveSpeed;
        public float MinDistToMove => minDistToMove;
        public float ForwardSpeed => forwardSpeed;
        public float FrictionMoveToZeroSpeed => frictionMoveToZeroSpeed;


        public event UnityAction OnLevelFinish;
        public void FireOnFinish() => OnLevelFinish?.Invoke();
        
        public event UnityAction<SplineComputer, float> OnLevelStart;
        public void FireOnStart(SplineComputer computer, float distance) => OnLevelStart?.Invoke(computer, distance);
        
        public event UnityAction OnUpdate;
        public void FireOnUpdate() => OnUpdate?.Invoke();
        
        public event UnityAction<float> OnChangeFriction;
        public void FireOnChangeFriction(float percent) => OnChangeFriction?.Invoke(percent);
        
    }
}
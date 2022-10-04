using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Interactable
{
    public class HandAnimation : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private float left;
        [SerializeField] private float right;

        private float _defaultPos = 0;
        private Transform _transform;
        private bool _flipFlopState = false;
        private bool _canMove = false;

        private void Awake()
        {
            _transform = transform;
            _defaultPos = _transform.localPosition.x;
            
            Invoke(nameof(PlayMove), Random.Range(0f, 1.5f));
        }

        private void PlayMove() => _canMove = true;

        private void Update()
        {
            if (!_canMove)
                return;
           
            Move(_flipFlopState ? left + _defaultPos : right + _defaultPos);
        }

        private void Move(float targetX)
        {
            var pos = _transform.localPosition;
            var currentPos = pos;
            pos.x = targetX;
            _transform.localPosition = Vector3.MoveTowards(currentPos, pos, Time.deltaTime * speed);

            if (Math.Abs(_transform.position.x - targetX) < .05f)
                _flipFlopState = !_flipFlopState;
            
        }
    }
}
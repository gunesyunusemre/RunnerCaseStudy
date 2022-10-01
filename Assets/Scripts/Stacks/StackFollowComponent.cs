using System;
using UnityEngine;

namespace Stacks
{
    public class StackFollowComponent : StackBaseComponent
    {
        private Transform follow;
        private float height;
        private float maxSpeed;
        private float damping;
        private float tolerance;
    
    
        private Vector3 velocity = new Vector3();
        private bool _isConstructed = false;
        private Transform _transform;

        public void Construct(StackContainer container, Transform followTarget, bool isFirst)
        {
            follow = followTarget;
            height = container.GetHeight();
            if (isFirst)
            {
                maxSpeed = 999999;
                damping = 99999;
                tolerance = 0f;
            }
            else
            {
                maxSpeed = container.MaxSpeed;
                damping = container.Damping;
                tolerance = container.Tolerance;
            }

            _transform = transform;

            var followPos = follow.position;
            var target = new Vector3(followPos.x, followPos.y + height, followPos.z);
            _transform.position = target;

            _isConstructed = true;
        }

        private void Update()
        {
            if (!_isConstructed)
                return;

            var followPos = follow.position;
            Vector3 target;
            if (Math.Abs(_transform.position.x - followPos.x) < tolerance && Math.Abs(_transform.position.z - followPos.z) < tolerance)
                target = new Vector3(followPos.x, followPos.y + height, followPos.z);
            else
                target = new Vector3(followPos.x, followPos.y, followPos.z);


            velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
            var myPosition = _transform.position;
            var n1 = velocity - (myPosition - target) * (damping * damping * Time.deltaTime);
            var n2 = 1 + damping * Time.deltaTime;
            velocity = n1 / (n2 * n2);

            var targetPosition = myPosition;
            targetPosition += velocity * Time.deltaTime;
            targetPosition.z = followPos.z;
            myPosition = targetPosition;
            _transform.position = myPosition;


            Vector3 direction = (follow.position - myPosition).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(0, 0, direction.z));
            _transform.rotation = Quaternion.Slerp(_transform.rotation, lookRotation, Time.deltaTime * 9999f);
            var euler = _transform.eulerAngles;
            euler += follow.eulerAngles;
            _transform.eulerAngles = euler;
        }
    }
}

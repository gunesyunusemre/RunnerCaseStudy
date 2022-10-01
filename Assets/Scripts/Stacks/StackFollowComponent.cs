using System;
using Helpers;
using UnityEngine;

namespace Stacks
{
    public class StackFollowComponent : StackBaseComponent
    {
        private Transform follow;
        private Transform followParent;
        private float height;
        //private float maxSpeed;
        //private float damping;


        private Vector3 velocity = new Vector3();
        private Vector3 rotVelocity = new Vector3();
        private bool _isConstructed = false;
        private Transform _transform;

        [SerializeField] private float dampValue = .01f;
        [SerializeField] private float rotDampValue = .01f;

        public void Construct(StackContainer container, Transform followTarget, int index, Transform followparent)
        {
            followParent = followparent;
            follow = followTarget;
            height = container.GetHeight();
            if (index == 0)
                height = 0f;

            rotDampValue = container.RotDamping;
            dampValue = (float)Formula.Map(index, 0, container.MaxStackCount, container.MinDamping, container.MaxDamping);

            _transform = transform;

            var followPos = follow.position;
            var target = new Vector3(followPos.x, followPos.y + height, followPos.z);
            _transform.position = target;

            _isConstructed = true;
        }

        [SerializeField] private float quadSpeed = 30f;
        [SerializeField] private bool isSmoothDamp = true;
        
        
        private void Update()
        {
            if (!_isConstructed)
                return;

            
            //Pos
            var beforePos = follow.position;
            var mPos = _transform.position;
            var dir = beforePos - mPos;
            var target = new Vector3(beforePos.x, beforePos.y + height, beforePos.z);
            if (isSmoothDamp)
            {
                
                _transform.position = Vector3.SmoothDamp(mPos, target, ref velocity, dampValue);
            }
            else
            {
                var cPos = followParent.position;
                cPos.y += height;
                _transform.position = Formula.QuadraticCurve(mPos, target, cPos, Time.deltaTime * quadSpeed);
            }

            


            //Rot
            var myRot = _transform.localRotation;
            var currentRotVector = new Vector3(myRot.eulerAngles.x, myRot.eulerAngles.y, myRot.eulerAngles.z);
            var targetRot = Quaternion.LookRotation(dir.normalized);
            var targetRotVector = new Vector3(targetRot.eulerAngles.x, targetRot.eulerAngles.y, targetRot.eulerAngles.z);
            var euler = Vector3.SmoothDamp(currentRotVector, targetRotVector, ref rotVelocity, rotDampValue);
            euler = new Vector3(0f, 0f, euler.z);
            if (isSmoothDamp)
            {
                _transform.localRotation = Quaternion.Euler(euler);
            }
            else
            {
                var cPos = followParent.position;
                cPos.y += height;
                var dirC = target - cPos;
                var targetCRot = Quaternion.LookRotation(dirC.normalized);
                var targetCRotVector = new Vector3(targetCRot.eulerAngles.x, targetCRot.eulerAngles.y, targetCRot.eulerAngles.z);
                var quadraRot = Formula.QuadraticCurve(currentRotVector, targetRotVector, targetCRotVector, Time.deltaTime * quadSpeed);
                quadraRot = new Vector3(0f, 0f, quadraRot.z);
                _transform.localRotation = Quaternion.Euler(quadraRot);
            }
            
            
        }
        
        
        //Test 1
        /*var followPos = follow.position;
           Vector3 target;

           var diffAbs = Mathf.Abs(followParent.position.x - _transform.position.x);
           diffAbs = 0; //(float)Formula.Map(diffAbs, 0f, 10f, 0f, .7f);}}

           target = new Vector3(followPos.x, followPos.y + height - diffAbs, followPos.z);
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
        if (follow == followParent)
           return;

         Vector3 direction = (follow.position - myPosition).normalized;
         Quaternion lookRotation = Quaternion.LookRotation(new Vector3(0, 0, direction.z));
         _transform.rotation = Quaternion.Slerp(_transform.rotation, lookRotation, Time.deltaTime * 9999f);
         var euler = _transform.eulerAngles;
          euler += follow.eulerAngles;
         _transform.eulerAngles = euler;*/
    }
}
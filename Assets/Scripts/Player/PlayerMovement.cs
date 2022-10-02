using System;
using Dreamteck.Splines;
using Helpers;
using Managers;
using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private SplineFollower follower;
        [SerializeField] private Transform target;
        [SerializeField] private float minDistance;
        [SerializeField] private float bound;
        [SerializeField, Range(5f, 160f)] private float speed;
        [SerializeField, Range(0.001f, 0.06f)] private float minDistToMove;

        
        private BasicInputManagerEvents inputManagerEvents;
        private UIManagerEvents uiManagerEvents;
        private bool _disableInput;
        private Vector3 _oldMousePos;
        
        private void Start()
        {
            var checkEvents =
                ManagerEventsHelper.TryGetManagerEvents(out inputManagerEvents);
            if (!checkEvents)
            {
                "Input manager event can not found".Log();
                return;
            }
            
            var checkUIEvents =
                ManagerEventsHelper.TryGetManagerEvents(out uiManagerEvents);
            if (!checkUIEvents)
            {
                "UI manager event can not found".Log();
                return;
            }

            RegisterInputManagerEvents(0);
            RegisterUIManagerEvents();
        }

        private void OnDisable()
        {
            UnregisterInputManagerEvents();
            UnregisterUIManagerEvents();
        }

        private void OnStartTouch(Vector3 mousePos)
        {
            if (_disableInput) return;
            
            _oldMousePos = mousePos;
        }

        private void OnPerformingTouch(Vector3 mousePos)
        {
            if (_disableInput) return;
            
            var distance = Vector3.Distance(mousePos,
                _oldMousePos);
            
            if(distance < minDistance) return;
            var pos = mousePos;
            var targetPos = target.localPosition;

            if (targetPos.x <= bound && pos.x > _oldMousePos.x)
            {
                target.localPosition = Vector3.MoveTowards(target.localPosition, 
                    new Vector3(targetPos.x + minDistToMove * distance, targetPos.y, targetPos.z),
                    Time.fixedDeltaTime * speed);
                if (target.localPosition.x > bound)
                    target.localPosition = new Vector3(bound, targetPos.y, targetPos.z);
            }
            
            if (targetPos.x >= -bound && pos.x < _oldMousePos.x)
            {
                target.localPosition = Vector3.MoveTowards(target.localPosition, 
                    new Vector3(targetPos.x - minDistToMove * distance, targetPos.y, targetPos.z),
                    Time.fixedDeltaTime * speed);
                if (target.localPosition.x < -bound)
                    target.localPosition = new Vector3(-bound, targetPos.y, targetPos.z);
            }

            _oldMousePos = mousePos;
        }

        private void OnEndTouch(Vector3 mousePos)
        {
        }

        private void SetStatusForwardMovement(bool status) => follower.follow = status;

        private void OnTapToPlay()
        {
            SetStatusForwardMovement(true);
        }

        private void RegisterUIManagerEvents()
        {
            uiManagerEvents.OnTapToPlay += OnTapToPlay;
        }
        private void UnregisterUIManagerEvents()
        {
            uiManagerEvents.OnTapToPlay -= OnTapToPlay;
        }

        private void RegisterInputManagerEvents(int managerID)
        {
            inputManagerEvents.OnStartTouch += OnStartTouch;
            inputManagerEvents.OnPerformingTouch += OnPerformingTouch;
            inputManagerEvents.OnEndTouch += OnEndTouch;
            inputManagerEvents.OnDisable += InputManagerDisable;
        }

        private void UnregisterInputManagerEvents()
        {
            inputManagerEvents.OnStartTouch -= OnStartTouch;
            inputManagerEvents.OnPerformingTouch -= OnPerformingTouch;
            inputManagerEvents.OnEndTouch -= OnEndTouch;
            inputManagerEvents.OnDisable -= InputManagerDisable;
            inputManagerEvents.OnEnable -= RegisterInputManagerEvents;
        }

        private void InputManagerDisable(int arg0)
        {
            UnregisterInputManagerEvents();
            inputManagerEvents.OnEnable += RegisterInputManagerEvents;
        }
    }
}
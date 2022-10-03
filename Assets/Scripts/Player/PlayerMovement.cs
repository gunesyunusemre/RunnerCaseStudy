using System;
using DG.Tweening;
using Dreamteck.Splines;
using Helpers;
using Managers;
using UnityEngine;

namespace Player
{
    public class PlayerMovement : BasePlayerComponent
    {
        [SerializeField] private SplineFollower follower;
        [SerializeField] private Transform target;
        
        
        private float minDistance;
        private Vector2 bound;
        private float speed;
        private float minDistToMove;

        
        private BasicInputManagerEvents inputManagerEvents;
        private UIManagerEvents uiManagerEvents;
        private bool _disableInput = true;
        private Vector3 _oldMousePos;
        
        protected override void Awake()
        {
            base.Awake();

            minDistance = PlayerContainer.MinDistance;
            bound = PlayerContainer.Bound;
            speed = PlayerContainer.SwerveSpeed;
            minDistToMove = PlayerContainer.MinDistToMove;
            
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

            if (targetPos.x <= bound.y && pos.x > _oldMousePos.x)
            {
                target.localPosition = Vector3.MoveTowards(target.localPosition, 
                    new Vector3(targetPos.x + minDistToMove * distance, targetPos.y, targetPos.z),
                    Time.fixedDeltaTime * speed);
                if (target.localPosition.x > bound.y)
                    target.localPosition = new Vector3(bound.y, targetPos.y, targetPos.z);
            }
            
            if (targetPos.x >= bound.x && pos.x < _oldMousePos.x)
            {
                target.localPosition = Vector3.MoveTowards(target.localPosition, 
                    new Vector3(targetPos.x - minDistToMove * distance, targetPos.y, targetPos.z),
                    Time.fixedDeltaTime * speed);
                if (target.localPosition.x < bound.x)
                    target.localPosition = new Vector3(bound.x, targetPos.y, targetPos.z);
            }

            _oldMousePos = mousePos;
        }

        private void OnEndTouch(Vector3 mousePos)
        {
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
        
        private void SetStatusForwardMovement(bool status) => follower.follow = status;

        private void OnTapToPlay()
        {
            _disableInput = false;
            SetStatusForwardMovement(true);
        }

        protected override void OnLevelFinish()
        {
            base.OnLevelFinish();
            follower.follow = false;
            follower.enabled = false;
            _disableInput = true;
            var localPosition = target.localPosition;
            localPosition.x = 0f;
            localPosition.y = 1f;
            target.DOLocalMove(localPosition, .5f);
        }

        protected override void OnLevelStart(SplineComputer computer, float distance)
        {
            base.OnLevelStart(computer, distance);
            follower.spline = computer;
            follower.onPostBuild += () => follower.SetDistance(distance);
            follower.enabled = true;
            target.localPosition = Vector3.zero;
        }
    }
}
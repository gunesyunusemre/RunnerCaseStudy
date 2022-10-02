using System;
using Helpers;
using Managers.Base;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Managers
{
    public class BasicInputManager : BaseManager
    {
        private readonly BasicInputManagerEvents managerEvents = new BasicInputManagerEvents();
        private bool _isStartedTouch = false;
        public override void Init()
        {
            ManagerID = GetInstanceID();
        }

        public override Type GetEvents(out BaseManagerEvents instance)
        {
            instance = managerEvents;
            return typeof(BasicInputManagerEvents);
        }

        private void OnDisable()
        {
            managerEvents.FireOnDisable(ManagerID);
        }

        private void OnEnable()
        {
            managerEvents.FireOnEnable(ManagerID);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            StartTouch();
            PerformingTouch();
            EndTouch();
        }

        private void StartTouch()
        {
            if (!Input.GetMouseButtonDown(0))
                return;

            if (EventSystem.current.IsPointerOverGameObject())
                return;

            managerEvents.FireOnStartTouch(Input.mousePosition);
            _isStartedTouch = true;
        }

        private void PerformingTouch()
        {
            if (!Input.GetMouseButton(0))
                return;

            if (EventSystem.current.IsPointerOverGameObject()) 
                return;

            if (!_isStartedTouch)
            {
                managerEvents.FireOnStartTouch(Input.mousePosition);
                _isStartedTouch = true;
            }
            
            managerEvents.FireOnPerformingTouch(Input.mousePosition);
        }

        private void EndTouch()
        {
            if (!Input.GetMouseButtonUp(0))
                return;

            if (EventSystem.current.IsPointerOverGameObject()) 
                return;

            _isStartedTouch = false;
            managerEvents.FireOnEndTouch(Input.mousePosition);
        }
    }
}
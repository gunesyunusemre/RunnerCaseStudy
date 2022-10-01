using System;
using Managers.Base;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Managers
{
    public class BasicInputManager : BaseManager
    {
        private readonly BasicInputManagerEvents managerEvents = new BasicInputManagerEvents();
        public override void Init()
        {
        }

        public override Type GetEvents(out BaseManagerEvents instance)
        {
            instance = managerEvents;
            return typeof(BasicInputManagerEvents);
        }

        private void OnDisable()
        {
            var managerID = GetInstanceID();
            managerEvents.FireOnDisable(managerID);
        }

        private void OnEnable()
        {
            var managerID = GetInstanceID();
            managerEvents.FireOnEnable(managerID);
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
        }

        private void PerformingTouch()
        {
            if (!Input.GetMouseButton(0))
                return;

            if (EventSystem.current.IsPointerOverGameObject()) 
                return;
            
            managerEvents.FireOnPerformingTouch(Input.mousePosition);
        }

        private void EndTouch()
        {
            if (!Input.GetMouseButtonUp(0))
                return;

            if (EventSystem.current.IsPointerOverGameObject()) 
                return;
            
            managerEvents.FireOnEndTouch(Input.mousePosition);
        }
    }
}
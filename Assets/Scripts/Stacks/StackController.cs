using System;
using System.Collections.Generic;
using DG.Tweening;
using Helpers;
using Managers;
using Stacks.Instance;
using UnityEngine;

namespace Stacks
{
    public class StackController : MonoBehaviour, IStackControllerInstance
    {
        [SerializeField] private Transform parent;
        [SerializeField] private Transform followParent;
        [SerializeField] private List<Transform> parentList;


        private List<IStackInstance> _stackInstanceList = new List<IStackInstance>();
        public int InstanceID { get; private set; }

        private void OnEnable()
        {
            InstanceID = gameObject.GetInstanceID();
            this.AddInstance();
        }

        private void OnDisable()
        {
            this.RemoveInstance();
        }

        private void Awake()
        {
            var checkEvents =
                ManagerEventsHelper.TryGetManagerEvents(out LevelManagerEvents levelManagerEvents);
            if (checkEvents)
            {
                levelManagerEvents.OnNextLevel += ClearCurrentStack;
            }
        }

        public void AddStack(IStackInstance stackInstance)
        {
            PlayFx.AddStack();
            
            var stackTransform = stackInstance.GetTransform();
            var targetPos = Vector3.zero;
            foreach (var instance in _stackInstanceList)
                targetPos.y += instance.container.GetHeight();


            var parentIndex = _stackInstanceList.Count > parentList.Count -1 ? parentList.Count -1 : _stackInstanceList.Count;
            var targetParent = parentList[parentIndex];
            //var isFirst = _stackInstanceList.Count <= 0;
            //var isSecond = _stackInstanceList.Count <= 1;
            //var lastIndex = isFirst ? 0 : _stackInstanceList.Count - 1;
            //var index = isSecond ? 0 : _stackInstanceList.Count - 2;
            //var collectIndex = isFirst ? 0 : lastIndex + 1;

            stackTransform.GoToDynamicPosition(targetParent, Vector3.zero, .5f, 2f)
                .OnComplete(() =>
                {
                    // Transform followTarget = default;
                    // Transform followSecondTarget = default;
                    // if (isFirst)
                    //     followTarget = followParent;
                    // else
                    // {
                    //     var last = _stackInstanceList[lastIndex];
                    //     followTarget = last.GetTransform();
                    // }
//
                    // if (//
                    //    followSecondTarget = followTarget;
                    // else
                    //{
                    //    var last = _stackInstanceList[index];
                    //     followSecondTarget = last.GetTransform();
                    // }
                    //stackInstance.container.FireOnCollect(followTarget, collectIndex, followSecondTarget);
                    var followSecondTarget = parentIndex - 1 < 0 ? parentList[0] : parentList[parentIndex - 1];
                    stackInstance.container.FireOnCollect(targetParent, parentIndex, followSecondTarget);
                });
            _stackInstanceList.Add(stackInstance);
        }

        public bool TryRequestStack(out IStackInstance stackInstance)
        {
            stackInstance = default;
            if (_stackInstanceList.Count <= 0)
                return false;

            var index = _stackInstanceList.Count - 1;
            stackInstance = _stackInstanceList[index];
            _stackInstanceList.Remove(stackInstance);
            return true;
        }

        public void RemoveStack(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var index = _stackInstanceList.Count - 1;
                var last = _stackInstanceList[index];

                _stackInstanceList.Remove(last);
                last.DestroyYourself();
            }
        }
        
        private void ClearCurrentStack()
        {
            foreach (var stackInstance in _stackInstanceList)
                stackInstance.DestroyYourself();
            
            _stackInstanceList.Clear();
        }
    }
}
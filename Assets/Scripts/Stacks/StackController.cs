using System;
using System.Collections.Generic;
using DG.Tweening;
using Helpers;
using Stacks.Instance;
using UnityEngine;

namespace Stacks
{
    public class StackController : MonoBehaviour, IStackControllerInstance
    {
        [SerializeField] private Transform parent;
        [SerializeField] private Transform followParent;
        
        
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

        public void AddStack(IStackInstance stackInstance)
        {
            var stackTransform = stackInstance.GetTransform();
            var targetPos = Vector3.zero;
            foreach (var instance in _stackInstanceList)
                targetPos.y += instance.container.GetHeight();
            
            var isFirst = _stackInstanceList.Count <= 0;
            var isSecond = _stackInstanceList.Count <= 1;
            var lastIndex = isFirst ? 0 : _stackInstanceList.Count -1;
            var index = isSecond ? 0 : _stackInstanceList.Count - 2;
            var collectIndex = isFirst ? 0 : lastIndex + 1;
            _stackInstanceList.Add(stackInstance);

            stackTransform.GoToDynamicPosition(parent, targetPos, .5f, 2f)
                .OnKill(() =>
                {
                    Transform followTarget = default;
                    Transform followSecondTarget = default;
                    if (isFirst)
                        followTarget = followParent;
                    else
                    {
                        var last = _stackInstanceList[lastIndex];
                        followTarget = last.GetTransform();
                    }

                    if (isSecond)
                        followSecondTarget = followTarget;
                    else
                    {
                        var last = _stackInstanceList[index];
                        followSecondTarget = last.GetTransform();
                    }
                    stackInstance.container.FireOnCollect(followTarget, collectIndex, followSecondTarget);
                });
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
    }
}
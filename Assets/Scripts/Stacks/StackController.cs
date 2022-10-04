using System;
using System.Collections.Generic;
using DG.Tweening;
using Dreamteck.Splines;
using Helpers;
using Managers;
using Stacks.Instance;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Stacks
{
    public class StackController : MonoBehaviour, IStackControllerInstance
    {
        [SerializeField] private Transform parent;
        [SerializeField] private Transform followParent;
        [SerializeField] private List<Transform> parentList;
        [SerializeField] private SplinePositioner dummy;
        


        private readonly List<IStackInstance> _stackInstanceList = new List<IStackInstance>();
        private SplineComputer _computer;
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
            GameObject pleb = new GameObject();
            pleb.name = "Break Stack Dummy Positioner";
            dummy = pleb.AddComponent<SplinePositioner>();
            
            var checkEvents =
                ManagerEventsHelper.TryGetManagerEvents(out LevelManagerEvents levelManagerEvents);
            if (checkEvents)
            {
                levelManagerEvents.OnNextLevel += ClearCurrentStack;
                levelManagerEvents.OnLevelStarted += OnLevelStarted;
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

        public void BreakStack()
        {
            var currentPos = transform.position;
            var currentSample = _computer.Project(currentPos);
            var distance = _computer.CalculateLength(0f, currentSample.percent);
            distance += 10f;
            //var maxDistance = _computer.CalculateLength();
            //var targetPercent = Formula.Map(distance, 0f, maxDistance, 0f, 1f);

            //var targetPos = _computer.EvaluatePosition(targetPercent);
            dummy.SetDistance(distance);

            for (int i = 0; i < 5; i++)
            {
                var randomUnit = Random.insideUnitCircle * 2.5f;
                var localPos = new Vector3(randomUnit.x, 1f, randomUnit.y);
                var stackTargetPos = dummy.transform.TransformPoint(localPos);

                var checkStack = TryRequestStack(out var stackInstance);
                if (!checkStack)
                    break;
                
                stackInstance.BreakStack(stackTargetPos);
            }
        }
        
        private void ClearCurrentStack()
        {
            foreach (var stackInstance in _stackInstanceList)
                stackInstance.DestroyYourself();
            
            _stackInstanceList.Clear();
        }
        
        private void OnLevelStarted(SplineComputer computer, float _)
        {
            _computer = computer;
            dummy.spline = _computer;
        }
    }
}
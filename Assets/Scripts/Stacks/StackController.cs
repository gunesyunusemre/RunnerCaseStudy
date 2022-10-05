using System;
using System.Collections.Generic;
using System.Linq;
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
        private bool _isUntouchable = false;
        private float _currentScore = 0;
        
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
            
            CalculateScore();
        }

        public bool TryRequestStack(out IStackInstance stackInstance)
        {
            stackInstance = default;
            if (_isUntouchable)
                return false;

            
            if (_stackInstanceList.Count <= 0)
                return false;

            var index = _stackInstanceList.Count - 1;
            stackInstance = _stackInstanceList[index];
            _stackInstanceList.Remove(stackInstance);
            
            CalculateScore();
            return true;
        }

        public void RemoveStack(int count)
        {
            if (_isUntouchable)
                return;

            for (int i = 0; i < count; i++)
            {
                var index = _stackInstanceList.Count - 1;
                var last = _stackInstanceList[index];

                _stackInstanceList.Remove(last);
                last.DestroyYourself();
            }
            PlayFx.RemoveStack();
            CalculateScore();
        }

        public bool CheckStack() => _stackInstanceList.Count > 0;

        public void BreakStack()
        {
            if (_isUntouchable)
                return;

            
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

        public void Upgrade()
        {
            if (_isUntouchable)
                return;

            if (_stackInstanceList.Count <= 0)
                return;

            _isUntouchable = true;
            
            var first = _stackInstanceList[0];
            int lastType = first.container.StackType;
            IStackInstance upgradeTarget = first;

            List<IStackInstance> upgradeTargetList = new List<IStackInstance>();
            upgradeTargetList.Add(upgradeTarget);
            
            Dictionary<IStackInstance, IStackInstance> upgradedDict = new Dictionary<IStackInstance, IStackInstance>();
            for (int i = 1; i < _stackInstanceList.Count; i++)
            {
                var currentStack = _stackInstanceList[i];
                var currentStackType = currentStack.container.StackType;
                if (lastType == currentStackType)
                {
                    upgradedDict.Add(currentStack, upgradeTarget);
                }
                else
                {
                    lastType = currentStackType;
                    upgradeTarget = currentStack;
                }
                if (!upgradeTargetList.Contains(upgradeTarget))
                    upgradeTargetList.Add(upgradeTarget);
            }

            for (int i = 0; i < upgradeTargetList.Count; i++)
            {
                var current = upgradeTargetList[i];
                var currentTransform = current.GetTransform();
                var currentParent = parentList[i];
                
                currentTransform.SetParent(currentParent);
                currentTransform.localPosition = Vector3.zero;
            }

            int upgradedCount = 0;
            foreach (var kvp in upgradedDict)
            {
                var current = kvp.Key;
                var target = kvp.Value;

                var currentTransform = current.GetTransform();
                var targetTransform = target.GetTransform();

                currentTransform.GoToDynamicPosition(targetTransform.parent, Vector3.zero, .2f, 0f).OnKill(() =>
                {
                    current.DestroyYourself();
                    targetTransform.localScale += Vector3.one *.05f;
                    target.container.MyLevel++;
                    _stackInstanceList.Remove(current);

                    upgradedCount++;
                    if (upgradedCount >= upgradeTargetList.Count)
                        _isUntouchable = false;
                });
            }

            if (upgradedDict.Count <= 0)
                _isUntouchable = false;
            
            PlayFx.UpgradeStacks();
            CalculateScore();
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
            _currentScore = 0;
        }

        private void CalculateScore()
        {
            float score = 0f;
            foreach (var stackInstance in _stackInstanceList)
            {
                score += stackInstance.container.MyLevel * 1f;
            }

            var dif = score - _currentScore;
            _currentScore = score;
            var checkEvents =
                ManagerEventsHelper.TryGetManagerEvents(out ScoreManagerEvents scoreManagerEvents);
            if (!checkEvents) return;
            if (dif > 0)
                scoreManagerEvents.FireOnIncreaseScore(dif);
            else if (dif< 0)
                scoreManagerEvents.FireOnDecreaseScore(Mathf.Abs(dif));
        }
    }
}
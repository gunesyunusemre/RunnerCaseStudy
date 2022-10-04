using DG.Tweening;
using UnityEngine;

namespace Helpers
{
    public static class TweenHelper
    {
        public static Tweener GoToStaticPosition(this Transform targetTransform, Vector3 targetPosition, float duration, float offset, float rotationOffset)
        {
            targetTransform.parent = null;

            var startPosition = targetTransform.position;
            var targetPos = targetPosition;
            var _targetEuler = Vector3.zero;
            _targetEuler = targetTransform.localEulerAngles;
            var startRotation = _targetEuler.y;
            var endRotation = startRotation + rotationOffset;
            var tween = DOTween.To(value =>
                {
                    targetTransform.position = Formula.SmoothCurveY(startPosition, targetPos,
                        offset, value);
                    
                    _targetEuler.y = Mathf.Lerp(startRotation, endRotation, value);
                    targetTransform.localEulerAngles = _targetEuler;
                            
                    //targetTransform.localScale = Vector3.Lerp(targetTransform.localScale, Vector3.one, value);
                }, 0f, 1f, duration)
                .SetUpdate(UpdateType.Normal)
                .SetEase(Ease.InOutQuad);

            return tween;
        }
        
        
        public static Tweener GoToDynamicPosition(this Transform transform, Transform newParent, Vector3 targetPosition, float duration, float offset)
        {
            transform.parent = null;
            
            var startRot = transform.rotation;

            var tween = DOTween.To(value =>
                {
                    var targetPos = newParent.position;
                    var newTargetVec = Quaternion.Euler(newParent.eulerAngles) * targetPosition;
                    targetPos += newTargetVec;
                    startRot = Quaternion.Lerp(startRot, newParent.rotation, value);
                    transform.rotation = startRot;
                    transform.position = Formula.SmoothCurveY(transform.position, targetPos,
                        offset, value);
                }, 0f, 1f, duration)
                .SetUpdate(UpdateType.Normal)
                .SetEase(Ease.OutCubic)
                .OnKill(() =>
                {
                    transform.rotation = newParent.rotation;
                    transform.position = Quaternion.Euler(newParent.eulerAngles) * targetPosition + newParent.position;
                    if (transform.gameObject.activeSelf)
                        transform.SetParent(newParent);
                });

            return tween;
        }
    }
}
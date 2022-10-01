using UnityEngine;
using UnityEngine.Events;

namespace Managers
{
    public class BasicInputManagerEvents : BaseManagerEvents
    {
        public event UnityAction<Vector3> OnStartTouch;
        public void FireOnStartTouch(Vector3 mousePosition) => OnStartTouch?.Invoke(mousePosition);
        
        public event UnityAction<Vector3> OnPerformingTouch;
        public void FireOnPerformingTouch(Vector3 mousePosition) => OnPerformingTouch?.Invoke(mousePosition);
        
        public event UnityAction<Vector3> OnEndTouch;
        public void FireOnEndTouch(Vector3 mousePosition) => OnEndTouch?.Invoke(mousePosition);
    }
}
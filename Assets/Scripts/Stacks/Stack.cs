using DG.Tweening;
using Helpers;
using Stacks.Instance;
using UnityEngine;

namespace Stacks
{
    [SelectionBase]
    public class Stack : MonoBehaviour, IStackInstance
    {
        public int InstanceID { get; private set; }
        public StackContainer container { get; set; }

        private Transform _transform;
        private Transform _firstParent;

        private void OnEnable()
        {
            _transform = transform;
            _firstParent = _transform.parent;
            InstanceID = gameObject.GetInstanceID();
            this.AddInstance();
        }

        private void Start()
        {
            container.OnBeforeCollect += OnBeforeCollect;
            container.OnCollect += OnCollect;
        }

        private void OnDisable()
        {
            this.RemoveInstance();
        }

        public void DestroyYourself()
        {
            Destroy(gameObject);
        }

        public Transform GetTransform() => _transform;
        public void BreakStack(Vector3 targetPos)
        {
            _transform.SetParent(null);
            _transform.rotation = Quaternion.identity;
            _transform.GoToStaticPosition(targetPos, .75f, 5f, 0f).OnKill(() =>
            {
                gameObject.AddComponent<StackControllerDetectorStackComponent>();
                _transform.SetParent(_firstParent);
            });
        }

        private void OnBeforeCollect(IStackControllerInstance stackController)
        {
            stackController.AddStack(this);
        }

        private void OnCollect(Transform followTarget, int index, Transform followParent)
        {
            //var followComponent = gameObject.AddComponent<StackFollowComponent>();
            //followComponent.Construct(container, followTarget, index, followParent);
        }
    }
}

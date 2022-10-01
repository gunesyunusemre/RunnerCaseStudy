using System;
using UnityEngine;

namespace Managers.Base
{
    public abstract class BaseManager : MonoBehaviour
    {
        public abstract void Init();
        public abstract Type GetEvents(out BaseManagerEvents instance);
        public virtual void OnUpdate(){}
    }
}
using System.Collections.Generic;
using Managers.Base;
using UnityEngine;

namespace Managers
{
    [CreateAssetMenu(fileName = "Managers Container", menuName = "Game/Managers Container", order = 0)]
    public class ManagersContainer : ScriptableObject
    {
        [SerializeField] private List<BaseManager> managerList;

        public List<BaseManager> ManagerList => managerList;
    }
}
using Stacks;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "Game Container", menuName = "Game/Container", order = 0)]
    public class GameContainer : ScriptableObject
    {
        [SerializeField] private StackContainer stackContainer;

        
        
        public StackContainer StackContainer => stackContainer;
    }
}
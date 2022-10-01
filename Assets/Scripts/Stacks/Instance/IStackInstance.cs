using InstanceSystem;
using UnityEngine;

namespace Stacks.Instance
{
    public interface IStackInstance : IInstance
    {
        StackContainer container { get; set; }
        void DestroyYourself();
        Transform GetTransform();
    }
}
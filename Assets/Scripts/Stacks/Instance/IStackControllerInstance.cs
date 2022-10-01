using InstanceSystem;

namespace Stacks.Instance
{
    public interface IStackControllerInstance : IInstance
    {
        void AddStack(IStackInstance instance);
    }
}
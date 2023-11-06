using Osyacat.Ecs.System;
using Osyacat.Ecs.World;

namespace Osyacat.Ecs
{
    public interface IEcsUniverse
    {
        IWorld World { get; }
        ISystems Systems { get; }
    }
}
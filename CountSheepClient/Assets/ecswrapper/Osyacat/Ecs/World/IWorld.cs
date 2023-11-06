using System;
using Osyacat.Ecs.Entity;
using Osyacat.Ecs.Matcher;

namespace Osyacat.Ecs.World
{
    public interface IWorld
    {
        IEntity CreateEntity();
        IFilter GetFilter(Func<IEntryMatcher, IMatcher> matcher);
    }
}
using System;
using System.Collections.Generic;
using Osyacat.Ecs.Entity;
using Osyacat.Ecs.Matcher;

namespace Osyacat.Ecs.System
{
    public interface IReactSystem : ISystem
    {
        Func<IEntryMatcher, IMatcher> Matcher { get; }

        void React(List<IEntity> entities);
    }
}
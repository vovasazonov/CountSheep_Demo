using System.Collections.Generic;
using Osyacat.Ecs.Entity;
using Osyacat.Ecs.System;
using Osyacat.Ecs.World;
using UnityEngine;

namespace Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Invisible
{
    public class InvisibleSystem : IUpdateSystem
    {
        private readonly IFilter _invisiblesFilter;
        private readonly List<IEntity> _invisibleBuffer = new();

        public InvisibleSystem(IWorld world)
        {
            _invisiblesFilter = world.GetFilter(matcher => matcher.Has<InvisibleComponent>());
        }

        public void Update()
        {
            _invisiblesFilter.UpdateBuffer(_invisibleBuffer);
            
            foreach (var invisible in _invisibleBuffer)
            {
                var seconds = invisible.Get<InvisibleComponent>().Seconds - Time.deltaTime;

                if (seconds <= 0)
                {
                    invisible.Remove<InvisibleComponent>();
                }
                else
                {
                    invisible.Replace<InvisibleComponent>().Seconds = seconds;
                }
            }
        }
    }
}
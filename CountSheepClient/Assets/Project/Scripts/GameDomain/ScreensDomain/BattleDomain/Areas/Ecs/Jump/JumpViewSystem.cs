using System;
using System.Collections.Generic;
using Osyacat.Ecs.Entity;
using Osyacat.Ecs.Matcher;
using Osyacat.Ecs.System;
using Project.CoreDomain.Services.Audio;

namespace Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Jump
{
    public class JumpViewSystem : IReactSystem, IUpdateSystem
    {
        private readonly IAudioService _audioService;
        private HashSet<IEntity> _jumpingEntities = new();
        private List<IEntity> _toRemove = new();

        public Func<IEntryMatcher, IMatcher> Matcher => matcher => matcher.Has<JumpingComponent>();

        public JumpViewSystem(IAudioService audioService)
        {
            _audioService = audioService;
        }

        public void React(List<IEntity> entities)
        {
            foreach (var entity in entities)
            {
                if (!_jumpingEntities.Contains(entity))
                {
                    _jumpingEntities.Add(entity);
                    _audioService.Sound.Play(BattleScreenContentIds.JumpSound);
                }
            }
        }

        public void Update()
        {
            foreach (var entity in _jumpingEntities)
            {
                if (!entity.Contains<JumpingComponent>())
                {
                    _toRemove.Add(entity);
                }
            }

            foreach (var toRemove in _toRemove)
            {
                _jumpingEntities.Remove(toRemove);
            }
            
            _toRemove.Clear();
        }
    }
}
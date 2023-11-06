using System;
using System.Collections.Generic;
using Osyacat.Ecs.Entity;
using Osyacat.Ecs.Matcher;
using Osyacat.Ecs.System;

namespace Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Effect
{
    public class EffectReactSystem<TComponent> : IReactSystem where TComponent : class
    {
        private readonly string _effectId;
        
        public Func<IEntryMatcher, IMatcher> Matcher => matcher => matcher.Has<TComponent>();

        public EffectReactSystem(string effectId)
        {
            _effectId = effectId;
        }

        public void React(List<IEntity> entities)
        {
            foreach (var tile in entities)
            {
                if (tile.Contains<EffectComponent>())
                {
                    var effect = tile.Get<EffectComponent>().Effects;
                    if (!effect.Contains(_effectId))
                    {
                        effect.Add(_effectId);
                        effect = new List<string>(effect);
                    }

                    tile.Get<EffectComponent>().Effects = null;
                    tile.Replace<EffectComponent>().Effects = effect;
                }
            }
        }
    }
}
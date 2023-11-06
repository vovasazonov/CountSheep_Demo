using System;
using System.Collections.Generic;
using Osyacat.Ecs.Entity;
using Osyacat.Ecs.Matcher;
using Osyacat.Ecs.System;
using Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Invisible;
using Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Jump;
using Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Player;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Score.Model;

namespace Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Score
{
    public class ScoreSystem : IInitializeSystem, IReactSystem
    {
        private readonly IScoreModel _scoreModel;

        public Func<IEntryMatcher, IMatcher> Matcher => matcher => matcher.Has<PlayerSideComponent>();

        public ScoreSystem(IScoreModel scoreModel)
        {
            _scoreModel = scoreModel;
        }

        public void Initialize()
        {
            _scoreModel.CurrentScore = 0;
        }

        public void React(List<IEntity> entities)
        {
            foreach (var player in entities)
            {
                if (player.Contains<JumpingComponent>() && !player.Contains<InvisibleComponent>())
                {
                    _scoreModel.CurrentScore++;
                }
            }
        }
    }
}
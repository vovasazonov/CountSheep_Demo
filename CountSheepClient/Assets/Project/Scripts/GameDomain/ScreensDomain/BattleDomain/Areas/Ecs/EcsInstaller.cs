using Osyacat.Ecs.System;
using Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Effect;
using Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Enemy;
using Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Farm;
using Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Input;
using Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Invisible;
using Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Jump;
using Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Move;
using Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Player;
using Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Rule;
using Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Score;
using Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.View;
using Zenject;

namespace Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs
{
    public class EcsInstaller : Installer<EcsInstaller>
    {
        public override void InstallBindings()
        {
            BindLogic();
            BindView();
        }

        private void BindLogic()
        {
            Container.Bind<ISystem>().To<InputSystem>().AsSingle();
            Container.Bind<ISystem>().To<PlayerCreatorSystem>().AsSingle();
            Container.Bind<ISystem>().To<EnemyCreatorSystem>().AsSingle();
            Container.Bind<ISystem>().To<MoveSystem>().AsSingle();
            Container.Bind<ISystem>().To<SwitchMoveDirectionSystem>().AsSingle();
            Container.Bind<ISystem>().To<ViewSystem>().AsSingle();
            Container.Bind<ISystem>().To<JumpSystem>().AsSingle();
            Container.Bind<ISystem>().To<GameoverSystem>().AsSingle();
            Container.Bind<ISystem>().To<PlayerJumpOnlyNearEnemySystem>().AsSingle();
            Container.Bind<ISystem>().To<PlayerSideChangerSystem>().AsSingle();
            Container.Bind<ISystem>().To<PlayerAnimationSystem>().AsSingle();
            Container.Bind<ISystem>().To<InvisibleSystem>().AsSingle();
            Container.Bind<ISystem>().To<ResumeSystem>().AsSingle();
            Container.Bind<ISystem>().To<PlaySystem>().AsSingle();
            Container.Bind<ISystem>().To<ScoreSystem>().AsSingle();
            Container.Bind<ISystem>().To<FarmSystem>().AsSingle();
            Container.Bind<ISystem>().To<AutoJumpOverGateSystem>().AsSingle();
        }

        private void BindView()
        {
            Container.Bind<ISystem>().To<InivisibleViewSystem>().AsSingle();
            Container.Bind<ISystem>().FromInstance(new EffectReactSystem<InvisibleViewComponent>("invisible"));
            
            Container.Bind<ISystem>().To<JumpViewSystem>().AsSingle();
        }
    }
}
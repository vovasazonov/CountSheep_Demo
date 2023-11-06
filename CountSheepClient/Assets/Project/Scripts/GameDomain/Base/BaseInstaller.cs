using Project.CoreDomain.Services;
using Project.GameDomain.ScreensDomain;
using Zenject;

namespace Project.GameDomain.Base
{
    public class BaseInstaller : MonoInstaller<BaseInstaller>
    {
        public override void InstallBindings()
        {
            ServicesInstaller.Install(Container);
            ScreensInstaller.Install(Container);
            AreasInstaller.Install(Container);
        }
    }
}

using System;

namespace Project.CoreDomain.Services.Engine
{
    public interface IEngineService
    {
        event Action Updating;
        event Action FixedUpdating;
        event Action Destroying;
        event Action Paused;
        event Action UnPaused;
    }
}
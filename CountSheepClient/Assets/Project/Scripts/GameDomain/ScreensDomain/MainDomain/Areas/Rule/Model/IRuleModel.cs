using System;

namespace Project.GameDomain.ScreensDomain.MainDomain.Areas.Rule.Presenter
{
    public interface IRuleModel
    {
        event Action Prepared;
        event Action Started;
        event Action Finished;
        event Action Resumed;
        event Action Paused;
        
        bool IsReadyToPlay { get; }
        bool IsPlaying { get; }

        void PrepareToStart();
        void Start();
        void Finish();
        void Resume();
        void Pause();
    }
}
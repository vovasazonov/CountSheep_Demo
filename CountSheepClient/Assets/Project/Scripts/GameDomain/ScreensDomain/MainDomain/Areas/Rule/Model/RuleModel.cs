using System;

namespace Project.GameDomain.ScreensDomain.MainDomain.Areas.Rule.Presenter
{
    public class RuleModel : IRuleModel
    {
        public event Action Finished;
        public event Action Prepared;
        public event Action Started;
        public event Action Resumed;
        public event Action Paused;

        public bool IsReadyToPlay { get; private set; }
        public bool IsPlaying { get; private set; }

        public void PrepareToStart()
        {
            IsReadyToPlay = true;
            IsPlaying = false;
            Prepared?.Invoke();
        }

        public void Start()
        {
            IsReadyToPlay = false;
            IsPlaying = true;
            Started?.Invoke();
        }

        public void Finish()
        {
            IsReadyToPlay = false;
            IsPlaying = false;
            Finished?.Invoke();
        }

        public void Resume()
        {
            if (!IsReadyToPlay)
            {
                IsPlaying = true;
            }

            Resumed?.Invoke();
        }

        public void Pause()
        {
            if (!IsReadyToPlay)
            {
                IsPlaying = false;
            }
            
            Paused?.Invoke();
        }
    }
}
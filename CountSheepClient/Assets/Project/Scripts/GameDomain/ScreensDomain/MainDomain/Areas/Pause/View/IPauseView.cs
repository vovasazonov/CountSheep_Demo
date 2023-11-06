using System;

namespace Project.GameDomain.ScreensDomain.MainDomain.Areas.Pause.View
{
    public interface IPauseView
    {
        event Action Paused;
        event Action Resumed;
        event Action Restarted;
        event Action MusicStatusChanged;
        event Action SoundStatusChanged;
        
        bool IsMusicOn { set; }
        bool IsSoundOn { set; }
        
        void ShowPauseButton();
        void HidePauseButton();
        void ShowPopup();
        void HidePopup();
    }
}
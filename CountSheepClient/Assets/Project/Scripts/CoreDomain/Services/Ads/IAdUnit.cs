using System;

namespace Project.Scripts.CoreDomain.Services.Ads
{
    public interface IAdUnit
    {
        event Action ReadyUpdated;
        
        bool IsEnable { get; set; }
        bool IsReady { get; }

        void Watch(Action onSuccess, Action onError);
        void Watch();
    }
}
using System;

namespace Project.CoreDomain.Services.GoogplayGames
{
    public interface IGoogleplayGamesService
    {
        bool IsAuthenticated { get; }

        void SignIn(Action onSign);
        void SignOut();
    }
}
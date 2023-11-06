using System;
using Cysharp.Threading.Tasks;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

namespace Project.CoreDomain.Services.GoogplayGames
{
    public class GoogleplayGamesService : IGoogleplayGamesService, ITaskAsyncInitializable
    {
        public bool IsAuthenticated { get; private set; }

        public UniTask InitializeAsync()
        {
            try
            {
                PlayGamesPlatform.Activate();
                PlayGamesPlatform.Instance.Authenticate(result =>
                {
                    if (result == SignInStatus.Success)
                    {
                        SetAuthenticationInformation(true);
                    }
                    else
                    {
                        SetAuthenticationInformation(false);
                    }
                });
            }
            catch (Exception e)
            {
                OsyaLogger.LogError(e.Message);
            }

            return UniTask.CompletedTask;
        }

        public void SignIn(Action onSign)
        {
            try
            {
                if (!IsAuthenticated)
                {
                    PlayGamesPlatform.Instance.ManuallyAuthenticate(authResult =>
                    {
                        if (authResult == SignInStatus.Success)
                        {
                            SetAuthenticationInformation(true);
                            onSign.Invoke();
                        }
                        else
                        {
                            SetAuthenticationInformation(false);
                        }
                    });
                }
            }
            catch (Exception e)
            {
                OsyaLogger.LogError(e.Message);
            }
        }

        private void SetAuthenticationInformation(bool isAuthenticated)
        {
            IsAuthenticated = isAuthenticated;
            OsyaLogger.Log($"GoogleplayGames isAuthenication = {isAuthenticated}");
        }

        public void SignOut()
        {
            throw new System.NotImplementedException();
        }
    }
}
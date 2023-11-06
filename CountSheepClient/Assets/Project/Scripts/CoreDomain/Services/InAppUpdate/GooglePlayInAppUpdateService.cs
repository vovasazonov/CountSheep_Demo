using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using Google.Play.AppUpdate;
using Google.Play.Common;
using UnityEngine;

namespace Project.CoreDomain.Services.InAppUpdate
{
    public class GooglePlayInAppUpdateService : IInAppUpdateService, ITaskAsyncInitializable
    {
        private AppUpdateManager _appUpdateManager;

        public async UniTask InitializeAsync()
        {
            try
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    _appUpdateManager = new AppUpdateManager();
                    StartUpdate();
                }
            }
            catch (Exception e)
            {
                OsyaLogger.LogError(e.Message);
#if UNITY_EDITOR
                throw;
#endif
            }
        }

        private async UniTask StartUpdate()
        {
            try
            {
                await CheckForUpdate().ToUniTask();
            }
            catch (Exception e)
            {
                OsyaLogger.LogError(e.Message);
            }
        }

        private IEnumerator CheckForUpdate()
        {
            PlayAsyncOperation<AppUpdateInfo, AppUpdateErrorCode> appUpdateInfoOperation = _appUpdateManager.GetAppUpdateInfo();

            // Wait until the asynchronous operation completes.
            yield return appUpdateInfoOperation;

            if (appUpdateInfoOperation.IsSuccessful)
            {
                var appUpdateInfoResult = appUpdateInfoOperation.GetResult();
                // Check AppUpdateInfo's UpdateAvailability, UpdatePriority,
                // IsUpdateTypeAllowed(), etc. and decide whether to ask the user
                // to start an in-app update.

                if (appUpdateInfoResult.UpdateAvailability == UpdateAvailability.UpdateAvailable)
                {
                    var appUpdateOptions = AppUpdateOptions.FlexibleAppUpdateOptions();
                    yield return StartFlexibleAppUpdate(appUpdateInfoResult, appUpdateOptions);
                }
            }
            else
            {
                // Log appUpdateInfoOperation.Error.
            }
        }

        private IEnumerator StartFlexibleAppUpdate(AppUpdateInfo info, AppUpdateOptions options)
        {
            var startUpdateRequest = _appUpdateManager.StartUpdate(info, options);

            while (!startUpdateRequest.IsDone)
            {
                yield return null;
            }
            
            var result = _appUpdateManager.CompleteUpdate();
            
            yield return result;
        }
    }
}
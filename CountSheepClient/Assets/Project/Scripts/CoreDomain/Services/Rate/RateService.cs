using System;
using Cysharp.Threading.Tasks;
#if UNITY_ANDROID
using Google.Play.Review;
#endif
using Project.CoreDomain.Services.Logger;
#if UNITY_IOS
using UnityEngine.iOS;
#endif

namespace Project.CoreDomain.Services.Rate
{
    public class RateService : IRateService
    {
        public async UniTask Rate(Action onSuccess, Action onError)
        {
#if UNITY_ANDROID
            await AndroidRate(onSuccess, onError);
#elif UNITY_IOS
            await IosRate(onSuccess);
#endif
        }

#if UNITY_ANDROID
        private async UniTask AndroidRate(Action onSuccess, Action onError)
        {
            var reviewManager = new ReviewManager();

            await UniTask.Delay(1000);

            var requestFlowOperation = reviewManager.RequestReviewFlow();
            await requestFlowOperation.ToUniTask();

            if (requestFlowOperation.Error != ReviewErrorCode.NoError)
            {
                OsyaLogger.LogError($"Review failed: {requestFlowOperation.Error.ToString()}");
                onError?.Invoke();
            }
            else
            {
                var playReviewInfo = requestFlowOperation.GetResult();
                var launchFlowOperation = reviewManager.LaunchReviewFlow(playReviewInfo);
                await launchFlowOperation.ToUniTask();

                if (launchFlowOperation.Error != ReviewErrorCode.NoError)
                {
                    OsyaLogger.LogError($"Review failed: {launchFlowOperation.Error.ToString()}");
                    onError?.Invoke();
                }
                else
                {
                    onSuccess?.Invoke();
                }
            }
        }
#endif

#if UNITY_IOS
        private async UniTask IosRate(Action onSuccess)
        {
            Device.RequestStoreReview();
            onSuccess?.Invoke();
        }
#endif
    }
}
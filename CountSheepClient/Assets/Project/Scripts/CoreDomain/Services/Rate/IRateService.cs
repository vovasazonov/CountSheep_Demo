using System;
using Cysharp.Threading.Tasks;

namespace Project.CoreDomain.Services.Rate
{
    public interface IRateService
    {
        UniTask Rate(Action onSuccess, Action onError);
    }
}
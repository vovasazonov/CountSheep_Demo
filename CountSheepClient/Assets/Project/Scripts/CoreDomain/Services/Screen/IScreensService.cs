using System;
using Cysharp.Threading.Tasks;

namespace Project.CoreDomain.Services.Screen
{
    public interface IScreensService
    {
        event Action Switched;
        
        string Current { get; }
        
        UniTask SwitchAsync(string screenId);
    }
}
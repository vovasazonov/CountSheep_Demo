using Cysharp.Threading.Tasks;
using Project.CoreDomain.Services.Audio.Configs.AudioPlayer;
using Project.CoreDomain.Services.Audio.Player;

namespace Project.CoreDomain.Services.Audio.Collection
{
    public interface IAudioCollection
    {
        float Volume { get; set; }
        bool IsMuted { get; set; }
        
        UniTask<IAudioStopper> Play(string playerId);
        IAudioStopper PlayImmediately(string playerId);
        IAudioStopper PlayImmediately(AudioPlayerConfig config);
    }
}
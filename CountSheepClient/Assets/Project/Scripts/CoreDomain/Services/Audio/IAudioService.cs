using Project.CoreDomain.Services.Audio.Collection;

namespace Project.CoreDomain.Services.Audio
{
    public interface IAudioService
    {
        IAudioCollection Music { get; }
        IAudioCollection Sound { get; }
    }
}
namespace Project.CoreDomain.Services.Audio.Player
{
    public interface IAudioStopper
    {
        bool IsStopped { get; }
        void Stop();
    }
}
namespace Project.CoreDomain.Services.Audio.Player
{
    internal interface IAudioPlayer : IAudioStopper
    {
        string Id { get; }
        float Volume { get; set; }
        bool IsMuted { get; set; }
        void Play();
        void Update();
    }
}
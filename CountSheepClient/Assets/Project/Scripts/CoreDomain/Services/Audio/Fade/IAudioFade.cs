namespace Project.CoreDomain.Services.Audio.Fade
{
    internal interface IAudioFade
    {
        float PercentFade { get; }
        
        void Update();
    }
}
using System;

namespace Project.CoreDomain.Services.Time
{
    public interface ITimeService
    {
        float DeltaTime { get; }
        float FixedTime { get; }
        float InGameTime { get; }
        DateTime CurrentTime { get; }
    }
}
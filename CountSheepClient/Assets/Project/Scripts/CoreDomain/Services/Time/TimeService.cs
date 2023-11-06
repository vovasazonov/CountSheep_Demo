using System;

namespace Project.CoreDomain.Services.Time
{
    public class TimeService : ITimeService
    {
        public float DeltaTime => UnityEngine.Time.deltaTime;
        public float FixedTime => UnityEngine.Time.fixedTime;
        public float InGameTime => UnityEngine.Time.time;
        public DateTime CurrentTime => DateTime.Now;
    }
}
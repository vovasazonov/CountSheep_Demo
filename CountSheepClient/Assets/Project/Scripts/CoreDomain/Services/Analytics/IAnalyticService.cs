using System.Collections.Generic;

namespace Project.CoreDomain.Services.Analytics
{
    public interface IAnalyticService
    {
        void Track(string name, Dictionary<string, object> properties = null);
    }
}
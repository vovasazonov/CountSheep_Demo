using System.Collections.Generic;
using Zenject;

namespace Project.CoreDomain.Services.Di
{
    public class DiService : IDiService
    {
        public Dictionary<string, DiContainer> ContainerByScreenId { get; } = new();
    }
}
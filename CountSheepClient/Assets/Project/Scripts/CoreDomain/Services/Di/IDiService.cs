using System.Collections.Generic;
using Zenject;

namespace Project.CoreDomain.Services.Di
{
    public interface IDiService
    {
        public Dictionary<string, DiContainer> ContainerByScreenId { get; }
    }
}
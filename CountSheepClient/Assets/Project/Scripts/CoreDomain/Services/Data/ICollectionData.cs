using System.Collections.Generic;

namespace Project.CoreDomain.Services.Data
{
    public interface ICollectionData<T>
    {
        IEnumerable<T> Collection { get; }
        bool Remove(T data);
        void Add(T data);
        T Create();
    }
}
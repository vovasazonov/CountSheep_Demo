using System;
using System.Collections.Generic;

namespace Project.GameDomain.ScreensDomain.MainDomain.Areas.Farm.Model
{
    public interface IFarmModel
    {
        event Action<string> CollectionUpdated;
        event Action<int> Added;
        event Action<int> Removed;
        event Action<int> Updated;
        event Action AreaUpdated;

        IEnumerable<string> Collection { get; }
        IReadOnlyDictionary<int, string> Animals { get; }
        
        void Add(string playerId);
        void Remove(int entityId);
        bool TryMerge(int first, List<int> second);
        bool ContainsInCollection(string id);
        void ResetCollection();
        bool IsAnimalMaxLevel(int id);
        string GetName(string id);
        int GetCoinsPerClick(string id);
        int GetCoinsPerClick(int id);
        bool HasPlaceInFarm();
        int GetMaxAnimalInFarm();
        void IncreaseLevelArea();
        int GetCostNextArea();
        bool HasIncreaseAreaLevel();
    }
}
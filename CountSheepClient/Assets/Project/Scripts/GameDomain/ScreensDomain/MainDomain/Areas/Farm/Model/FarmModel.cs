using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Project.CoreDomain;
using Project.CoreDomain.Services.Analytics;
using Project.CoreDomain.Services.Data;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Farm.Config;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Farm.Data;
using Project.Scripts.CoreDomain.Services.Ads;

namespace Project.GameDomain.ScreensDomain.MainDomain.Areas.Farm.Model
{
    public class FarmModel : IFarmModel, IDomainTaskAsyncInitializable
    {
        public event Action<string> CollectionUpdated;
        public event Action<int> Added;
        public event Action<int> Removed;
        public event Action<int> Updated;
        public event Action AreaUpdated;

        private readonly IDataStorageService _dataStorageService;
        private readonly IFarmConfigKeeper _farmConfigKeeper;
        private readonly IAdsService _adsService;
        private FarmData _data;
        private int _counterId;
        private int _adsCounter;

        public IEnumerable<string> Collection => _data.Collection;
        public IReadOnlyDictionary<int, string> Animals => _data.Animals;

        public FarmModel(
            IDataStorageService dataStorageService, 
            IFarmConfigKeeper farmConfigKeeper,
            IAdsService adsService)
        {
            _dataStorageService = dataStorageService;
            _farmConfigKeeper = farmConfigKeeper;
            _adsService = adsService;
        }

        public UniTask InitializeAsync()
        {
            var dataKey = "farm";
            _data = _dataStorageService.Contains(dataKey) ? _dataStorageService.Get<FarmData>(dataKey) : _dataStorageService.Create<FarmData>(dataKey);
            _counterId = Animals.Count == 0 ? 0 : Animals.Select(i => i.Key).Max();
            return UniTask.CompletedTask;
        }

        public void Add(string playerId)
        {
            if (!HasPlaceInFarm())
            {
                OsyaLogger.LogError("Was added animal, but over available place");
            }

            _counterId++;

            _data.Animals.Add(_counterId, playerId);
            Added?.Invoke(_counterId);

            if (!_data.Collection.Contains(playerId))
            {
                _data.Collection.Add(playerId);
                AnalyticService.Instance.Track("new_animal_opened", new Dictionary<string, object>()
                {
                    {"itemName", playerId}
                });
                CollectionUpdated?.Invoke(playerId);
            }

            TrackMaxAmountAnalytic();
        }

        private void TrackMaxAmountAnalytic()
        {
            if (_data.Animals.Count == GetMaxAnimalInFarm())
            {
                AnalyticService.Instance.Track("area_merge_full");

                try
                {
                    if (_data.Animals.Values.GroupBy(x => x)
                        .Select(g => new { Value = g.Key, Count = g.Count() })
                        .OrderByDescending(x => x.Count).Any(i => i.Count > 1 && !IsAnimalMaxLevel(_data.Animals.First(j => j.Value == i.Value).Key)))
                    {
                        AnalyticService.Instance.Track("merge_area_notmergable");
                    }
                }
                catch (Exception e)
                {
                    OsyaLogger.LogError(e.Message);
                }
            }
        }

        public void Remove(int entityId)
        {
            _data.Animals.Remove(entityId);
            Removed?.Invoke(entityId);
        }

        public bool TryMerge(int first, List<int> seconds)
        {
            foreach (var second in seconds)
            {
                if (second != first && Animals.ContainsKey(second))
                {
                    var firstId = Animals[first];
                    var secondId = Animals[second];
                    var variantsIds = _farmConfigKeeper.FarmConfig.Animals.Where(i =>
                        i.Parents != null &&
                        i.Parents.Count == 2 &&
                        (i.Parents[0] == firstId && i.Parents[1] == secondId ||
                         i.Parents[0] == secondId && i.Parents[1] == firstId)
                    ).Select(i => i.Id);

                    foreach (var variantId in variantsIds)
                    {
                        Remove(first);
                        _data.Animals[second] = variantId;
                        Updated?.Invoke(second);
                        _adsCounter++;

                        if (!_data.Collection.Contains(variantId))
                        {
                            _data.Collection.Add(variantId);
                            CollectionUpdated?.Invoke(variantId);
                        }
                        else
                        {
                            if (_adsCounter > 5)
                            {
                                _adsCounter = 0;
                                _adsService.Inter.Watch();
                            }
                        }

                        _dataStorageService.Save();

                        return true;
                    }
                }
            }

            return false;
        }

        public bool ContainsInCollection(string id)
        {
            return _data.Collection.Contains(id);
        }

        public void ResetCollection()
        {
            _data.Collection.Clear();
        }

        public bool IsAnimalMaxLevel(int id)
        {
            var configId = Animals[id];
            return _farmConfigKeeper.FarmConfig.Animals.FirstOrDefault(i =>
                i.Parents != null &&
                i.Parents.Count == 2 &&
                i.Parents[0] == configId &&
                i.Parents[1] == configId
            ) == null;
        }

        public string GetName(string id)
        {
            return _farmConfigKeeper.FarmConfig.Animals.First(i => i.Id == id).Name;
        }

        public int GetCoinsPerClick(string id)
        {
            return _farmConfigKeeper.FarmConfig.Animals.First(i => i.Id == id).CoinsPerClick;
        }

        public int GetCoinsPerClick(int id)
        {
            return GetCoinsPerClick(Animals[id]);
        }

        public bool HasPlaceInFarm()
        {
            return GetMaxAnimalInFarm() > Animals.Count;
        }

        public int GetMaxAnimalInFarm()
        {
            return _farmConfigKeeper.FarmConfig.Areas.First(i => i.Level == _data.CurrentLevelArea).AmountAnimal;
        }

        public void IncreaseLevelArea()
        {
            _data.CurrentLevelArea++;
            AreaUpdated?.Invoke();
        }

        public int GetCostNextArea()
        {
            var nextAreaConfig = _farmConfigKeeper.FarmConfig.Areas.FirstOrDefault(i => i.Level == _data.CurrentLevelArea + 1);
            return nextAreaConfig?.Cost ?? 0;
        }

        public bool HasIncreaseAreaLevel()
        {
            return _farmConfigKeeper.FarmConfig.Areas.Any(i => i.Level == _data.CurrentLevelArea + 1);
        }
    }
}
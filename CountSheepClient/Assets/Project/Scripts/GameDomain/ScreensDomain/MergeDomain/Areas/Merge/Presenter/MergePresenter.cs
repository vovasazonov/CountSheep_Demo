using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Project.CoreDomain;
using Project.CoreDomain.Services.Data;
using Project.CoreDomain.Services.View;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Currency.Model;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Farm.Model;
using Project.GameDomain.ScreensDomain.MergeDomain.Areas.Merge.View;

namespace Project.GameDomain.ScreensDomain.MergeDomain.Areas.Merge.Presenter
{
    public class MergePresenter : IPresenter
    {
        private readonly IViewService _viewService;
        private readonly IDataStorageService _dataStorageService;
        private readonly IFarmModel _farmModel;
        private readonly ICurrencyModel _currencyModel;
        private IDisposableView<MergeView> _backgroundView;
        private PrimitiveData<long> _leftTicksData;

        public MergePresenter(IViewService viewService, IDataStorageService dataStorageService, IFarmModel farmModel, ICurrencyModel currencyModel)
        {
            _viewService = viewService;
            _dataStorageService = dataStorageService;
            _farmModel = farmModel;
            _currencyModel = currencyModel;
        }

        private void InitializeData()
        {
            if (_leftTicksData != null)
            {
                return;
            }
            
            var dataKey = "merge_ticks";

            if (_dataStorageService.Contains(dataKey))
            {
                _leftTicksData = _dataStorageService.Get<PrimitiveData<long>>(dataKey);
            }
            else
            {
                _leftTicksData = _dataStorageService.Create<PrimitiveData<long>>(dataKey);
                _leftTicksData.Value = DateTime.Now.Ticks;
                _dataStorageService.Save();
            }
        }

        public async UniTask InitializeAsync()
        {
            InitializeData();
            _backgroundView = await _viewService.CreateAsync<MergeView>(MergeScreenContentIds.MergePrefab);
            UpdateAmountCollectedCoins();
        }

        private void UpdateAmountCollectedCoins()
        {
            var previousData = new DateTime(_leftTicksData.Value);
            TimeSpan diff = DateTime.Now - previousData;
            var collectedCoins = (diff.Seconds * _farmModel.Animals.Keys.Select(i => _farmModel.GetCoinsPerClick(i)).Sum())/20;
            _currencyModel.Amount += collectedCoins;
            _backgroundView.Value.ShowAmountCollectedCoins(collectedCoins);
        }

        public UniTask DisposeAsync()
        {
            _backgroundView?.Dispose();
            _backgroundView = null;
            _leftTicksData.Value = DateTime.Now.Ticks;
            _dataStorageService.Save();
            
            return UniTask.CompletedTask;
        }
    }
}
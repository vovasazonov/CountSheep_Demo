using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Project.CoreDomain;
using Project.CoreDomain.Services.Analytics;
using Project.CoreDomain.Services.Data;
using Project.CoreDomain.Services.Rate;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Farm.Model;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Rule.Presenter;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Score.Model;

namespace Project.GameDomain.ScreensDomain.MainDomain.Areas.Rate
{
    public class RateModel : IDomainTaskAsyncInitializable, IDomainTaskAsyncDisposable
    {
        private readonly IRateService _rateService;
        private readonly IDataStorageService _dataStorageService;
        private readonly IScoreModel _scoreModel;
        private readonly IFarmModel _farmModel;
        private readonly IRuleModel _ruleModel;
        private readonly string _rateDataKey = "rated_on_30";
        private PrimitiveData<bool> _isRated;
        private bool _isSuggestRateInThisSession = true;

        public RateModel(IRateService rateService, IDataStorageService dataStorageService, IScoreModel scoreModel, IFarmModel farmModel, IRuleModel ruleModel)
        {
            _rateService = rateService;
            _dataStorageService = dataStorageService;
            _scoreModel = scoreModel;
            _farmModel = farmModel;
            _ruleModel = ruleModel;
        }

        public UniTask InitializeAsync()
        {
            _farmModel.CollectionUpdated += OnCollectionsUpdated;
            _ruleModel.Finished += OnFinished;
            _isRated = _dataStorageService.Contains(_rateDataKey) ? _dataStorageService.Get<PrimitiveData<bool>>(_rateDataKey) : _dataStorageService.Create<PrimitiveData<bool>>(_rateDataKey);
            return UniTask.CompletedTask;
        }

        public UniTask DisposeAsync()
        {
            _farmModel.CollectionUpdated -= OnCollectionsUpdated;
            _ruleModel.Finished -= OnFinished;
            return UniTask.CompletedTask;
        }

        private void OnFinished()
        {
            InvokeRate();
        }

        private void OnCollectionsUpdated(string obj)
        {
            InvokeRate();
        }

        private void InvokeRate()
        {
            try
            {
                if ((_scoreModel.MaxScore > 50 || _farmModel.Collection.Count() > 9) && !_isRated.Value && _isSuggestRateInThisSession)
                {
                    _rateService.Rate(() =>
                        {
                            _isRated.Value = true;
                            _dataStorageService.Save();
                            AnalyticService.Instance.Track("rate_app");
                        },
                        () => _isSuggestRateInThisSession = false);
                }

                _isSuggestRateInThisSession = false;
            }
            catch (Exception e)
            {
                OsyaLogger.LogError(e.Message);
            }
        }
    }
}
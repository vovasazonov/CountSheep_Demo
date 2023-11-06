using System;
using Cysharp.Threading.Tasks;
using Project.CoreDomain;
using Project.CoreDomain.Services.Data;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Currency.Data;

namespace Project.GameDomain.ScreensDomain.MainDomain.Areas.Currency.Model
{
    public class CurrencyModel : ICurrencyModel, IDomainTaskAsyncInitializable
    {
        private readonly IDataStorageService _dataStorageService;
        private CurrencyData _data;

        public event Action Updated;

        public int Amount
        {
            get => _data.Amount;
            set
            {
                _data.Amount = value;
                Updated?.Invoke();
                _dataStorageService.Save();
            }
        }

        public CurrencyModel(IDataStorageService dataStorageService)
        {
            _dataStorageService = dataStorageService;
        }

        public UniTask InitializeAsync()
        {
            var dataKey = "currency";
            _data = _dataStorageService.Contains(dataKey) ? _dataStorageService.Get<CurrencyData>(dataKey) : _dataStorageService.Create<CurrencyData>(dataKey);
            return UniTask.CompletedTask;
        }
    }
}
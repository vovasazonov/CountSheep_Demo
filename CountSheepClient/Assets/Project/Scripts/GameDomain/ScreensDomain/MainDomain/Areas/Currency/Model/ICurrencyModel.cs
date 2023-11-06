using System;

namespace Project.GameDomain.ScreensDomain.MainDomain.Areas.Currency.Model
{
    public interface ICurrencyModel
    {
        event Action Updated;
        
        int Amount { get; set; }
    }
}
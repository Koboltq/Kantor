using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Models;

namespace Repository.IRepo
{
    public interface ICurrencyRepo
    {
        IQueryable<Currency> GetCurrencies();
        IQueryable<Currency> GetCurrencies(string userId,int? page = 1, int? pageSize = 10);
        Currency GetCurrencyById(int idCurrency);
        bool DeleteCurrency(int idCurrency);
        void SaveChanges();
        bool Add(Currency currency);
        void Update(Currency currency);
        void UpdateSalesPrice(Response response);
        IEnumerable<string> GetAvailableCurrency();
    }
}

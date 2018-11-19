using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Repository.IRepo;
using Repository.Models;
using System.Data.Entity;
using Newtonsoft.Json;

namespace Repository.Repo
{
    public class CurrencyRepo : ICurrencyRepo
    {
        private readonly IOglContext _db;
        
        public CurrencyRepo(IOglContext db)
        {
            _db = db;
        }

        public bool Add(Currency currency)
        {
            if (_db.Currencies.Any(x => x.Name == currency.Name && x.UserId == currency.UserId))
                return false ;
            ICurrencyFixerRepo getFixerCurrency = new CurrencyFixerRepo();
            var response = getFixerCurrency.GetCurrentRates();
            var rate = response.Rates.First(x => x.Code == currency.Name);
            currency.SalesPrice = rate.Bid;
            currency.PurchasePrice = rate.Ask;
            currency.UpdatedDate = response.TimeStamp;
            _db.Currencies.Add(currency);
            return true;
        }

        public bool DeleteCurrency(int idCurrency)
        {
            try
            {
                Currency currency = _db.Currencies.Find(idCurrency);
                _db.Currencies.Remove(currency);
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public IEnumerable<string> GetAvailableCurrency()
        {
            return (from currency in _db.Currencies
                        group currency.Name by currency.Name into newgroup
                        select newgroup.Key).ToList();
        }

        public IQueryable<Currency> GetCurrencies(string userId, int? page = 1, int? pageSize = 10)
        {
            var currencies = _db.Currencies
                .Where(x=>x.UserId == userId)
                .OrderByDescending(o => o.AddedDate)
                .Skip((page.Value - 1) * pageSize.Value)
                .Take(pageSize.Value);
            return currencies;
        }

        public virtual IQueryable<Currency> GetCurrencies()
        {
            var currencies = _db.Currencies
                .OrderByDescending(o => o.AddedDate);
            return currencies;
        }

        public Currency GetCurrencyById(int idCurrency)
        {
            Currency currency = _db.Currencies.Find(idCurrency);
            return currency;
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }

        public void Update(Currency currency)
        {
            _db.Entry(currency).State = EntityState.Modified;
        }

        public void UpdateSalesPrice(Response response )
        {
            DateTime currentTime = response.TimeStamp;
            User user = _db.User.First(x => x.UserName == "Admin");

            IEnumerable<Currency> getCurrenciesList = _db.Currencies.ToList();
            IEnumerable<Currency> getCurrenciesToUpdate =
                (from currency in getCurrenciesList
                 join res in response.Rates on currency.Name equals res.Code
                where currency.UpdatedDate < response.TimeStamp || currency.UpdatedDate == null
                select new Currency()
                {
                    Name = currency.Name,
                    AddedDate = currency.AddedDate,
                    UpdatedDate = response.TimeStamp,
                    SalesPrice = res.Bid,
                    PurchasePrice = res.Ask,
                    Quantity = currency.Quantity,
                    UserId = user.Id
                }).ToList();
            if (getCurrenciesToUpdate.Any())
            {
                try
                {
                    foreach (Currency currency in getCurrenciesToUpdate)
                    {
                        Add(currency);
                        SaveChanges();
                    }
                }
                catch(Exception ex)
                {
                    //jak starczy czasu to podpiac ilogger-a
                    Console.WriteLine(ex);
                }
            }
        }
    }

}
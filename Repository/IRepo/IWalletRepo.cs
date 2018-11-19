using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Models;

namespace Repository.IRepo
{
    public interface IWalletRepo
    {
        IQueryable<Wallet> GetCurrenciesWallet(string userId, int? page = 1, int? pageSIze = 10);
        Wallet GetCurrencyWalletById(int idCurrencyWallet, string userId);
        bool DeleteCurrency(int idCurrencyWallet);
        void SaveChanges();
        bool Add(Wallet wallet);
        void Update(Wallet wallet);
    }
}

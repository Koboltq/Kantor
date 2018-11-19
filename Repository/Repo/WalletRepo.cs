using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.IRepo;
using Repository.Models;

namespace Repository.Repo
{
    public class WalletRepo : IWalletRepo
    {
        private readonly IOglContext _db;

        public WalletRepo(IOglContext db)
        {
            _db = db;
        }

        public bool Add(Wallet wallet)
        {
            if (_db.Wallets.Any(x => x.Currency == wallet.Currency && x.UserId == wallet.UserId))
                return false;
            _db.Wallets.Add(wallet);
            return true;
        }

        public bool DeleteCurrency(int idCurrencyWallet)
        {
            try
            {
                Wallet wallet = _db.Wallets.Find(idCurrencyWallet);
                _db.Wallets.Remove(wallet);
                return true;
            }
            catch
            {
                return false;
            }

        }

        public IQueryable<Wallet> GetCurrenciesWallet(string userId, int? page = 1, int? pageSize = 10)
        {
            var wallet = _db.Wallets
                .Where(x => x.UserId == userId)
                .OrderByDescending(o => o.Currency)
                .Skip((page.Value - 1) * pageSize.Value)
                .Take(pageSize.Value);
            return wallet;
        }

        public Wallet GetCurrencyWalletById(int idCurrencyWallet, string userId)
        {
            Wallet wallet = _db.Wallets.Find(idCurrencyWallet);
            wallet.Money = _db.Wallets.Where(x => x.UserId == userId && x.Currency == "PLN").First().Quantity;
            return wallet;
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }

        public void Update(Wallet wallet)
        {
            _db.Entry(wallet).State = System.Data.Entity.EntityState.Modified;
        }
    }
}

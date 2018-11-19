using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Repository.Models;
using Repository.IRepo;
using Microsoft.AspNet.Identity;
using PagedList;

namespace OGL.Controllers
{
    public class WalletsController : Controller
    {
        private readonly ICurrencyRepo _currencyRepo;
        private readonly IWalletRepo _walletRepo;

        public WalletsController(ICurrencyRepo currencyRepo, IWalletRepo walletRepo)
        {
            _currencyRepo = currencyRepo;
            _walletRepo = walletRepo;
        }

        // GET: Wallets
        public ActionResult Index(int? page)
        {
            int currentPage = page ?? 1;
            int onPage = 10;
            var wallet = _walletRepo.GetCurrenciesWallet(User.Identity.GetUserId());
            return View(wallet.ToPagedList<Wallet>(currentPage, onPage));
        }

        // GET: Wallets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Wallet wallet = _walletRepo.GetCurrencyWalletById((int)id, User.Identity.GetUserId());
            if (wallet == null)
            {
                return HttpNotFound();
            }
            return View(wallet);
        }

        // GET: Wallets/Create
        public ActionResult Create()
        {
            var getUserWallet = _walletRepo.GetCurrenciesWallet(User.Identity.GetUserId());
            var getAvailableCurrencyList = _currencyRepo.GetAvailableCurrency().OrderBy(x => x);
            List<string> listNoAddedWallet = getAvailableCurrencyList.Where(a => !getUserWallet.Any(b => b.Currency == a)).ToList();
            ViewBag.CurrencyCategories = listNoAddedWallet;
            return View();
        }

        // POST: Wallets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Quantity,Currency")] Wallet wallet)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    wallet.UserId = User.Identity.GetUserId();
                    bool isExist = _walletRepo.Add(wallet);
                    if (!isExist)
                        return View(wallet);
                    _walletRepo.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                return View(wallet);
            }
            return View(wallet);
        }

        // GET: Wallets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Wallet wallet = _walletRepo.GetCurrencyWalletById((int)id, User.Identity.GetUserId());
            if (wallet == null)
            {
                return HttpNotFound();
            }
            else if (wallet.UserId != User.Identity.GetUserId() &&
    !(User.IsInRole("Admin") || User.IsInRole("Customer")))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(wallet);
        }

        // POST: Wallets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Quantity,Currency, UserId")] Wallet wallet)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _walletRepo.Update(wallet);
                    _walletRepo.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                ViewBag.Error = true;
                return View(wallet);
            }
            return View(wallet);
        }

        // GET: Wallets/Delete/5
        public ActionResult Delete(int? id, bool? error)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Wallet wallet = _walletRepo.GetCurrencyWalletById((int)id, User.Identity.GetUserId());
            if (wallet == null)
            {
                return HttpNotFound();
            }
            else if (wallet.UserId != User.Identity.GetUserId() &&
    !User.IsInRole("Admin"))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if(error != null)
            {
                ViewBag.Error = true;
            }
            return View(wallet);
        }

        // POST: Wallets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _walletRepo.DeleteCurrency(id);
            try
            {
                _walletRepo.SaveChanges();
            }
            catch
            {
                return RedirectToAction("Delete", new { id = id, error = true });
            }
            return RedirectToAction("Index");
        }


        public ActionResult Buy(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Wallet wallet = _walletRepo.GetCurrencyWalletById((int)id, User.Identity.GetUserId());
            if(wallet == null)
            {
                return HttpNotFound();
            }
            wallet.CurrencyCantor = _currencyRepo.GetCurrencies().FirstOrDefault(x => x.Name == wallet.Currency);

            if(wallet.CurrencyCantor == null)
            {
                return HttpNotFound();
            }
            
            return View(wallet);
        }

        //POST: Wallets/Buy/5
        [HttpPost, ActionName("Buy")]
        [ValidateAntiForgeryToken]
        public ActionResult BuyConfirmed([Bind(Include = "Id,Quantity,Currency, BuyAndSell, CurrencyCantor, UserId, Money")] Wallet wallet)
        {
            decimal purchaseCurrency = wallet.BuyAndSell.BuyQuantity * (decimal)wallet.CurrencyCantor.PurchasePrice;
            //Pobierz ilosc pieniedzy w PLN z portfela
            var getWalletPLN = _walletRepo.GetCurrenciesWallet(User.Identity.GetUserId()).First(x => x.Currency == "PLN");
            if (purchaseCurrency > getWalletPLN.Quantity)
            {
                wallet.Money = getWalletPLN.Quantity;
                ViewBag.Message = $"Nie masz wystarczającej ilości pieniedzy na koncie.";
                return View(wallet);
            }

            if(wallet.CurrencyCantor.Quantity < wallet.BuyAndSell.BuyQuantity)
            {
                wallet.Money = getWalletPLN.Quantity;
                ViewBag.Message = $"Nie masz wystarczającej ilości waluty w kantorze.";
                return View(wallet);
            }

            //zaktualizuj wykupiony kurs w kantorze
            wallet.CurrencyCantor = _currencyRepo.GetCurrencies().FirstOrDefault(x => x.Name == wallet.Currency && x.Quantity > 0);
            wallet.CurrencyCantor.Quantity = Math.Round(wallet.CurrencyCantor.Quantity - wallet.BuyAndSell.BuyQuantity,2);
            _currencyRepo.Update(wallet.CurrencyCantor);
            _currencyRepo.SaveChanges();
            //zaktualizuj portfel o wykupiony kurs
            wallet.Quantity = Math.Round(wallet.Quantity + wallet.BuyAndSell.BuyQuantity,2);
            _walletRepo.Update(wallet);
            _walletRepo.SaveChanges();
            //zaktualizuj portfel o roznice kupionych w PLN - kursu.
            getWalletPLN.Quantity = Math.Round(getWalletPLN.Quantity - purchaseCurrency, 2);
            _walletRepo.Update(getWalletPLN);
            _walletRepo.SaveChanges();

            return RedirectToAction("Index");
            
        }

        public ActionResult Sell(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Wallet wallet = _walletRepo.GetCurrencyWalletById((int)id, User.Identity.GetUserId());
            if (wallet == null)
            {
                return HttpNotFound();
            }
            wallet.CurrencyCantor = _currencyRepo.GetCurrencies().FirstOrDefault(x => x.Name == wallet.Currency);

            if (wallet.CurrencyCantor == null)
            {
                return HttpNotFound();
            }

            return View(wallet);
        }

        //POST: Wallets/Buy/5
        [HttpPost, ActionName("Sell")]
        [ValidateAntiForgeryToken]
        public ActionResult SellConfirmed([Bind(Include = "Id,Quantity,Currency, BuyAndSell, CurrencyCantor, UserId")] Wallet wallet)
        {
            // portfel jest pusty wybranej walucie
            if(wallet.Quantity <= 0)
            {
                ViewBag.Message = "Nie ma dostępnych środków.";
                return View(wallet);
            }
            // pobrana ilosc PLN z kantoru
            var getCurrencyCantorInPLN = _currencyRepo.GetCurrencies().FirstOrDefault(x => x.Name == "PLN");
            // ilosc ile chce uzytkownik sprzedac
            decimal sellQuantity = wallet.BuyAndSell.SellQuantity * (decimal)wallet.CurrencyCantor.SalesPrice;
            //
            if (getCurrencyCantorInPLN == null || sellQuantity > getCurrencyCantorInPLN.Quantity)
            {
                ViewBag.Message = "Nie ma dostępnych ilości stawki: PLN";
                return View(wallet);
            }

            //zaktulizuj kantor o roznice sprzedanyej waluty w PLN 
            getCurrencyCantorInPLN.Quantity = Math.Round(getCurrencyCantorInPLN.Quantity - sellQuantity,2);
            _currencyRepo.Update(getCurrencyCantorInPLN);
            _currencyRepo.SaveChanges();

            //zaktualizuj kantor o sprzedana walute
            var getCurrencyCantor = _currencyRepo.GetCurrencies().FirstOrDefault(x => x.Name == wallet.Currency);
            getCurrencyCantor.Quantity = Math.Round(getCurrencyCantor.Quantity + wallet.BuyAndSell.SellQuantity,2);
            _currencyRepo.Update(getCurrencyCantor);
            _currencyRepo.SaveChanges();

            //zaktualizuj portfel o usunieta ilosc waluty
            var getWalletCurrency = _walletRepo.GetCurrenciesWallet(User.Identity.GetUserId()).First(x => x.Currency == wallet.Currency);
            getWalletCurrency.Quantity = Math.Round(getWalletCurrency.Quantity - wallet.BuyAndSell.SellQuantity, 2);
            _walletRepo.Update(getWalletCurrency);
            _walletRepo.SaveChanges();

            //zaktualizuj portfel o dodatkowe PLN-y
            var getWalletPLN = _walletRepo.GetCurrenciesWallet(User.Identity.GetUserId()).First(x => x.Currency == "PLN");
            getWalletPLN.Quantity = Math.Round(getWalletPLN.Quantity + sellQuantity, 2);
            _walletRepo.Update(getWalletPLN);
            _walletRepo.SaveChanges();

            return RedirectToAction("Index");

            //decimal sell = wallet.BuyAndSell.SellQuantity * (decimal)wallet.CurrencyCantor.SalesPrice;
            //if (sell > wallet.CurrencyCantor.Quantity)
            //{
            //    ViewBag.Message = $"Przekroczono ilość dostępnych zasobów dla kantoru o kursie {wallet.CurrencyCantor.Name}.";
            //    return View(wallet);
            //}
            //else
            //{
            //    wallet.CurrencyCantor = _currencyRepo.GetCurrencies().FirstOrDefault(x => x.Name == "PLN" && x.Quantity > 0);
            //    wallet.CurrencyCantor.Quantity = Math.Round(wallet.CurrencyCantor.Quantity - sell, 2);

            //    _currencyRepo.Update(wallet.CurrencyCantor);
            //    _currencyRepo.SaveChanges();

            //    var getWalletPLN = _walletRepo.GetCurrenciesWallet(User.Identity.GetUserId()).First(x=>x.Currency == "PLN");

            //    getWalletPLN.Quantity = Math.Round(getWalletPLN.Quantity + sell, 2);
            //    _walletRepo.Update(getWalletPLN);
            //    _walletRepo.SaveChanges();

            //    var getWalletCurrency = _walletRepo.GetCurrenciesWallet(User.Identity.GetUserId()).First(x => x.Currency == wallet.Currency);

            //    getWalletCurrency.Quantity = Math.Round(getWalletCurrency.Quantity - wallet.BuyAndSell.SellQuantity, 2);
            //    _walletRepo.Update(getWalletCurrency);
            //    _walletRepo.SaveChanges();

            //    return RedirectToAction("Index");
            //}
        }


    }
}

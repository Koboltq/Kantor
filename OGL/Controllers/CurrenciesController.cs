using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Repository.Models;
using Repository.Repo;
using Repository.IRepo;
using PagedList;
using Microsoft.AspNet.Identity;
using System.Globalization;

namespace OGL.Controllers
{
    public class CurrenciesController : Controller
    {
        private readonly ICurrencyRepo _repo;

        public CurrenciesController(ICurrencyRepo repo)
        {
            _repo = repo;
        }

        // GET: Currencies
        public ActionResult Index(int? page, string sortOrder)
        {
            int currentPage = page ?? 1;
            int onPage = 10;
            var currencies = _repo.GetCurrencies(User.Identity.GetUserId());


            ViewBag.CurrentSort = sortOrder;
            ViewBag.IdSort = String.IsNullOrEmpty(sortOrder) ? "IdAsc" : "";
            ViewBag.AddedDateSort = sortOrder == "AddedDate" ? "AddedDate" : "AddedDateAsc";
            ViewBag.ContentSort = sortOrder == "ContentAsc" ? "Content" : "ContentAsc";
            ViewBag.TitleSort = sortOrder == "TitleAsc" ? "Title" : "TitleAsc";

            switch (sortOrder)
            {
                case "Name":
                    currencies = currencies.OrderByDescending(s => s.Name);
                    break;
                case "NameAsc":
                    currencies = currencies.OrderBy(s => s.Name);
                    break;
                case "AddedDate":
                    currencies = currencies.OrderByDescending(s => s.AddedDate);
                    break;
                case "AddedDateAsc":
                    currencies = currencies.OrderBy(s => s.AddedDate);
                    break;
                case "IdAsc":
                    currencies = currencies.OrderBy(s => s.Id);
                    break;
                default:
                    currencies = currencies.OrderByDescending(s => s.Id);
                    break;
            }

            return View(currencies.ToPagedList<Currency>(currentPage, onPage));
        }

        // GET: Currencies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Currency currency = _repo.GetCurrencyById((int)id);
            if (currency == null)
            {
                return HttpNotFound();
            }
            return View(currency);
        }

        // GET: Currencies/Create
        public ActionResult Create()
        {
            ICurrencyFixerRepo fixerRepo = new CurrencyFixerRepo();
            ViewBag.CurrencyCategories = fixerRepo.GetCurrentRates().Rates.Select(x => x.Code).ToList();
            return View();
        }

        // POST: Currencies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Quantity,CurrencyList")] Currency currency)
        {
            try
            {
                currency.UserId = User.Identity.GetUserId();
                currency.AddedDate = DateTime.Now;
                bool isExist = _repo.Add(currency);
                if (!isExist)
                {
                    ICurrencyFixerRepo fixerRepo = new CurrencyFixerRepo();
                    ViewBag.CurrencyCategories = fixerRepo.GetCurrentRates().Rates.Select(x => x.Code).ToList();
                    return View(currency);
                }
                _repo.SaveChanges();
                return RedirectToAction("Currencies");
            }
            catch(Exception ex)
            {
                return View(currency);
            }
        }

        // GET: Currencies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Currency currency = _repo.GetCurrencyById((int)id);
            if (currency == null)
            {
                return HttpNotFound();
            }
            else if(currency.UserId != User.Identity.GetUserId() &&
                !(User.IsInRole("Admin") || User.IsInRole("Customer")))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(currency);
        }

        // POST: Currencies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Quantity,PurchasePrice,SalesPrice,AddedDate,UpdatedDate,UserId")] Currency currency)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _repo.Update(currency);
                    _repo.SaveChanges();
                }
                catch(Exception ex)
                {
                    ViewBag.Error = true;
                    return View(currency);
                }
            }
            ViewBag.Error = false;
            return View(currency);
        }

        // GET: Currencies/Delete/5
        public ActionResult Delete(int? id, bool? error)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Currency currency = _repo.GetCurrencyById((int)id);
            if (currency == null)
            {
                return HttpNotFound();
            }
            else if(currency.UserId != User.Identity.GetUserId() && 
                !User.IsInRole("Admin"))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (error != null)
                ViewBag.Error = true;

            return View(currency);
        }

        // POST: Currencies/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _repo.DeleteCurrency(id);
            try
            {
                _repo.SaveChanges();
            }
            catch
            {
                return RedirectToAction("Delete", new { id = id, error = true });
            }
            return RedirectToAction("Index");
        }

        public ActionResult Partial(int? page)
        {
            int currentPage = page ?? 1;
            int onPage = 3;
            var currencies = _repo.GetCurrencies(User.Identity.GetUserId());
            currencies = currencies.OrderByDescending(d => d.AddedDate);
            return PartialView("Index", currencies.ToPagedList<Currency>(currentPage, onPage));
        }

        [OutputCache(Duration = 1000)]
        public ActionResult MyClassifields(int? page)
        {
            int currentPage = page ?? 1;
            int onPage = 3;
            string userId = User.Identity.GetUserId();
            var currencies = _repo.GetCurrencies(userId);
            currencies = currencies.OrderByDescending(d => d.AddedDate)
                .Where(o => o.UserId == userId);
            return View(currencies.ToPagedList<Currency>(currentPage, onPage));
        }
    }
}

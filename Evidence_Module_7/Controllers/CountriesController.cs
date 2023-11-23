using Evidence_Module_7.Models;
using Evidence_Module_7.Repositories;
using Evidence_Module_7.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Evidence_Module_7.Controllers
{
    public class CountriesController : Controller
    {
        private readonly CountryDbContext db = new CountryDbContext();
        IRepo repo;
        public CountriesController()
        {
            this.repo = new Repo();
        }
        // GET: Countries
        public ActionResult Index()
        {
            return View(repo.Get());
        }
        public ActionResult Create()
        {
            CountryInputModel c = new CountryInputModel();
            c.Universities.Add(new University { });
            
            return View(c);
        }
        [HttpPost]
        public ActionResult Create(CountryInputModel data, string act = "")
        {
            if (act == "add")
            {
                data.Universities.Add(new University { });
                foreach(var er in ModelState.Values)
                {
                    er.Errors.Clear();
                }

            }
            if (act.StartsWith("remove"))
            {
                int index = int.Parse(act.Substring(act.IndexOf("_") + 1));
                data.Universities.RemoveAt(index);
            }
            if (act == "insert")
            {
                if (ModelState.IsValid)
                {
                    repo.Insert(data);
                    return RedirectToAction("Index");
                }
            }
            return View(data);
        }
        public ActionResult Edit(int id)
        {
            var data = repo.GetById(id);
            ViewBag.CurrentPic = db.Countries.First(o => o.CountryId == id).Picture;
            return View(data);          
        }
        [HttpPost]
        public ActionResult Edit(CountryEditModel data, string act = "")
        {
            if (act == "add")
            {
                data.Universities.Add(new University { });

            }
            if (act.StartsWith("remove"))
            {
                int index = int.Parse(act.Substring(act.IndexOf("_") + 1));
                data.Universities.RemoveAt(index);
            }
            if (act == "update")
            {
                if (ModelState.IsValid)
                {
                    repo.Update(data);
                    return RedirectToAction("Index");
                }
            }
            ViewBag.CurrentPic = db.Countries.First(o => o.CountryId == data.CountryId).Picture;
            return View(data);
        }
        public ActionResult Delete(int id)
        {
            var sql = $"SELECT * FROM Countries WHERE CountryId={id}";
            var c = db.Countries.SqlQuery(sql).FirstOrDefault();
            sql = $"SELECT * FROM  Universities WHERE CountryId={id}";
            c.Universities = db.Database.SqlQuery<University>(sql).ToList();
            return View(c);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DoDelete(int id)
        {
            this.repo.Delete(id);
            return RedirectToAction("Index");
        }

    }
}
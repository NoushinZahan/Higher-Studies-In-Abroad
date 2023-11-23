using Evidence_Module_7.Models;
using Evidence_Module_7.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;

namespace Evidence_Module_7.Repositories
{
    public class Repo : IRepo
    {
        private readonly CountryDbContext db;
        public Repo()
        {
            db = new CountryDbContext();
        }

        public IEnumerable<Country> Get()
        {
               //Eager Loading
            var data = db.Countries.Include(x=>x.Universities).AsNoTracking().ToList();
            return data;
        }

        public CountryEditModel GetById(int id)
        {
            //Explicit Loading
            var x = db.Countries.FirstOrDefault(o => o.CountryId == id);
            db.Entry(x).Collection(o => o.Universities).Load();
            var data = new CountryEditModel
            {
                CountryId = x.CountryId,
                CountryName = x.CountryName,
                Capital = x.Capital,
                IsDeveloped = x.IsDeveloped,
                Universities = x.Universities.ToList()

            };
            return data;
        }

        public void Insert(CountryInputModel data)
        {
            var c = new Country
            {
                CountryName = data.CountryName,
                Capital = data.Capital,
                IsDeveloped = data.IsDeveloped
            };
            string ext = Path.GetExtension(data.Picture.FileName);
            string filename = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ext;
            string savepath = HttpContext.Current.Server.MapPath("~/pictures/") + filename;
            data.Picture.SaveAs(savepath);
            c.Picture = filename;
            foreach (var u in data.Universities)
            {
                c.Universities.Add(u);
            }
            db.Countries.Add(c);
            db.SaveChanges();
        }

        public void Update(CountryEditModel data)
        {
            var c = db.Countries.First(x => x.CountryId == data.CountryId);
            c.CountryName= data.CountryName;
            c.Capital = data.Capital;
            c.IsDeveloped= data.IsDeveloped;
            if (data.Picture != null)
            {
                string ext = Path.GetExtension(data.Picture.FileName);
                string filename = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ext;
                string savepath = HttpContext.Current.Server.MapPath("~/pictures/") + filename;
                data.Picture.SaveAs(savepath);
                c.Picture = filename;
            }
            db.Universities.RemoveRange(db.Universities.Where(x => x.CountryId == data.CountryId).ToList());
            foreach (var item in data.Universities)
            {
                c.Universities.Add(new University
                {
                    CountryId= item.CountryId,
                    UniversityName= item.UniversityName,
                    AdmissionRequirment= item.AdmissionRequirment,
                    EstublishDate= item.EstublishDate,
                    Ranking= item.Ranking
                });
            }
            db.SaveChanges();

        }
        public void Delete(int id)
        {
            var sql = $"DELETE  FROM Countries WHERE CountryId={id}";
            int ra = db.Database.ExecuteSqlCommand(sql);
        }
    }
}
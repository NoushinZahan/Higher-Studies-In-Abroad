using Evidence_Module_7.Models;
using Evidence_Module_7.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evidence_Module_7.Repositories
{
    public interface IRepo
    {
        IEnumerable<Country> Get();
       CountryEditModel GetById(int id);
        void Insert(CountryInputModel data);
        void Update(CountryEditModel data);
        void Delete(int id);
    }
}

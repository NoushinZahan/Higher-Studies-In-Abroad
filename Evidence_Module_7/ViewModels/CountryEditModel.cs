using Evidence_Module_7.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Evidence_Module_7.ViewModels
{
    public class CountryEditModel
    {
        public int CountryId { get; set; }
        [Required, StringLength(40)]
        public string CountryName { get; set; }
        [Required, StringLength(30)]
        public string Capital { get; set; }      
        public HttpPostedFileBase Picture { get; set; }
        public bool IsDeveloped { get; set; }
        public virtual List<University> Universities { get; set; } = new List<University>();
    }
}
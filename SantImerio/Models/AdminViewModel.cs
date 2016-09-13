using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace SantImerio.Models
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "RoleName")]
        public string Name { get; set; }
    }

    public class EditUserViewModel
    {
        public string Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }
        [Display(Name = "Nome")]
        public string Nome { get; set; }
        [Display(Name = "Cognome")]
        public string Cognome { get; set; }
        [Display(Name = "Data di nascita")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataNascita { get; set; }
        [Display(Name = "Anno inizio attività")]
        public string AnnoInizio { get; set; }
        [Display(Name = "Grado")]
        public string Grado { get; set; }
        [Display(Name = "La tua frase")]
        public string Frase { get; set; }
        [Display(Name = "Tokui kata")]
        public string Kata { get; set; }
        [Display(Name = "Maestro")]
        public bool Maestro { get; set; }
        [Display(Name = "Istruttore")]
        public bool Istruttore { get; set; }

        public IEnumerable<SelectListItem> RolesList { get; set; }
    }
}
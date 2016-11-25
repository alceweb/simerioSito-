﻿using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System;

namespace SantImerio.Models
{
    // È possibile aggiungere dati di profilo dell'utente specificando altre proprietà della classe ApplicationUser. Per ulteriori informazioni, visitare http://go.microsoft.com/fwlink/?LinkID=317594.
    public class ApplicationUser : IdentityUser
    {
        [Display(Name ="Nome")]
        public string Nome { get; set; }
         [Display(Name = "Cognome")]
       public string Cognome { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Tenere presente che il valore di authenticationType deve corrispondere a quello definito in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Aggiungere qui i reclami utente personalizzati
            return userIdentity;
        }
    }

    public class Eventi
    {
        [Key]
        public int Evento_Id { get; set; }
        [Display(Name = "Nome evento")]
        public string Titolo { get; set; }
        [Display(Name = "Sottotitolo")]
        public string SottoTitolo { get; set; }
        [Display(Name = "Descrizione")]
        [DataType(DataType.Text)]
        public string Descrizione { get; set; }
        [Display(Name = "Data evento")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/mmyyyy}", ApplyFormatInEditMode = true)]
        public DateTime Data { get; set; }
        [Display(Name = "Data inizio pubblicazione")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/mm/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataI { get; set; }
        [Display(Name = "Data fine pubblicazione")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/mm/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataF { get; set; }
        [Display(Name = "Visualizza nel calendario")]
        public bool Pubblica { get; set; }
        [Display(Name = "Visualizza negli eventi")]
        public bool Galleria { get; set; }
        [Display(Name ="Visualizza nella home")]
        public bool Home { get; set; }
    }

    public class OrariMesseBar
    {
        [Key]
        public int Messe_Id { get; set; }
        public string Tipo { get; set; }
        public string Descrizione { get; set; }

    }

    public class Documenti
    {
        [Key]
        public int Documenti_Id { get; set; }
        [Display(Name ="Titolo documento")]
        public string Titolo { get; set; }
        [Display(Name="Descrizione")]
        public string Descrizione { get; set; }

    }
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public DbSet<SantImerio.Models.Eventi> Eventis { get; set; }
        public DbSet<SantImerio.Models.OrariMesseBar> OrariMesseBars { get; set; }
        public DbSet<SantImerio.Models.Documenti> Documentis { get; set; }

    }
}
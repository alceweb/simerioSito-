using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections;
using System.Collections.Generic;

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
        [Display(Name ="Visualizza il volantino")]
        public bool Volantino { get; set; }
        [Display(Name = "Evento dell'anno pastorale")]
        public bool Pastorale { get; set; }
        public virtual ICollection<Commenti> Commentis { get; set; }
        public virtual ICollection<ImgTitoli> ImgTitolis { get; set; }
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

    public class Incarichi
    {
        [Key]
        public int Incarichi_Id { get; set; }
        [Display(Name ="Incarico")]
        public string Incarico { get; set; }
        [Display(Name ="Nome")]
        public string Nome { get; set; }
        [Display(Name ="Cognome")]
        public string Cognome { get; set; }
        [Display(Name ="Telefono")]
        public string Telefono { get; set; }
        [Display(Name ="Email")]
        [DataType(DataType.EmailAddress, ErrorMessage ="Email non valida")]
        public string Email { get; set; }
        [Display(Name ="Indirizzo")]
        public string Indirizzo { get; set; }
        [Display(Name ="Città")]
        public string Città { get; set; }
        [Display(Name ="CAP")]
        public string Cap { get; set; }
    }

    public class Articoli
    {
        [Key]
        public int Articolo_Id { get; set; }
        [Display(Name = "Titolo")]
        public string Titolo { get; set; }
        [Display(Name = "Sottotitolo")]
        public string SottoTitolo { get; set; }
        [Display(Name = "Testo")]
        [DataType(DataType.Text)]
        public string Testo { get; set; }
        [Display(Name = "Data articolo")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/mmyyyy}", ApplyFormatInEditMode = true)]
        public DateTime Data { get; set; }
        [Display(Name = "Pubblica")]
        public bool Pubblica { get; set; }
        [Display(Name ="Autore")]
        public string Autore { get; set; }
    }

    public class Commenti
    {
        [Key]
        public int Commento_Id { get; set; }
        [Display(Name ="Data commento")]
        public DateTime Data { get; set; }
        public int Evento_Id { get; set; }
        public virtual Eventi Titolo { get; set;}
        [Display(Name ="Commento")]
        public string Commento { get; set; }
        public string UId { get; set; }
        public string Utente { get; set; }
        public string Email { get; set; }
        public virtual ICollection<ComRisp> ComRisps { get; set; }
    }

    public class ComRisp
    {
        [Key]
        public int ComRisp_Id { get; set; }
        [Display(Name = "Data risposta")]
        public DateTime Data { get; set; }
        public int Commento_Id { get; set; }
        public virtual Commenti Commento { get; set; }
        public string Risposta { get; set; }
        public string UId { get; set; }
        public string Utente { get; set; }
        public string Email { get; set; }
    }

    public class Statistiche
    {
        [Key]
        public int Statistiche_Id { get; set; }
        public DateTime Data { get; set; }
        public string Ip { get; set; }
        public string Pagina { get; set; }
        public string UId { get; set; }
        public string UName { get; set; }
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
        public DbSet<SantImerio.Models.Incarichi> Incarichis { get; set; }
        public DbSet<SantImerio.Models.Articoli> Articolis { get; set; }
        public DbSet<SantImerio.Models.Commenti> Commentis { get; set; }
        public DbSet<SantImerio.Models.ComRisp> ComRisps { get; set; }
        public DbSet<SantImerio.Models.Statistiche> Statistiches { get; set; }
        public DbSet<SantImerio.Models.ImgTitoli> ImgTitolis { get; set; }

    }
}
using System.Data.Entity;
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
        [Display(Name = "Pubblica")]
        public bool Pubblica { get; set; }
        [Display(Name = "Galleria")]
        public bool Galleria { get; set; }
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

    }
}
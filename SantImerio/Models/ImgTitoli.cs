using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SantImerio.Models
{
    public class ImgTitoli
    {
        [Key]
        public int ImgTitolo_Id { get; set; }
        public string Img { get; set; }
        [Display(Name ="Titolo immagine")]
        public string ImgTitolo { get; set; }
        public int Evento_Id { get; set; }
        public virtual Eventi Titolo { get; set; }
    }
}
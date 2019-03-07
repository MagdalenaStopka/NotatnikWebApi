using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotatnikWebApi.Models
{
    public class NotatkaViewModel
    {
        public int Id { get; set; }
        public string Tytul { get; set; }
        public string Tresc { get; set; }
        public DateTime DataDodania { get; set; }

        //public void Notatka(int Id)
        //{
        //    DataDodania = DateTime.Now;
        //}
    }
}
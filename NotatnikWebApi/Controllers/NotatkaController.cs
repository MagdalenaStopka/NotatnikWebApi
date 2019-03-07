using NotatnikWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NotatnikWebApi.Controllers
{
    public class NotatkaController : ApiController
    {
        public IHttpActionResult GetAllNotatki()
        {
            IList<NotatkaViewModel> notatki = null;

            using (var dbContext = new NotatnikEntities())
            {
                notatki = dbContext.Notatka
                            .Select(n => new NotatkaViewModel()
                            {
                                Id = n.Id,
                                Tytul = n.Tytul,
                                Tresc = n.Tresc,
                                DataDodania = n.DataDodania
                                //DataDodania = (DateTime?)s.DataDodania ?? DateTime.Now
                            }).ToList<NotatkaViewModel>();
            }
            if (notatki.Count == 0)
            {
                return NotFound();
            }

            return Ok(notatki);
        }

        public IHttpActionResult GetNotatakaById(int id)
        {
            NotatkaViewModel notatka = null;

            using (var dbContext = new NotatnikEntities())
            {
                notatka = dbContext.Notatka
                    .Where(n => n.Id == id)
                    .Select(n => new NotatkaViewModel()
                    {
                        Id = n.Id,
                        Tytul = n.Tytul,
                        Tresc = n.Tresc,
                        DataDodania = n.DataDodania
                    }).FirstOrDefault<NotatkaViewModel>();
            }

            if (notatka == null)
            {
                return NotFound();
            }

            return Ok(notatka);
        }

        public IHttpActionResult PostNewNotatka(NotatkaViewModel notatka)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            using (var dbContext = new NotatnikEntities())
            {
                dbContext.Notatka.Add(new Notatka()
                {
                    Id = notatka.Id,
                    Tresc = notatka.Tresc,
                    Tytul = notatka.Tytul,
                  
                   // DataDodania  =(DateTime.Now)notatka.DataDodania
                    DataDodania=  DateTime.Now

                });

                dbContext.SaveChanges();
            }

            return Ok();
        }

        public IHttpActionResult Delete(int id)
        {
            if (id <= 0)
                return BadRequest("Not a valid notatka id");

            using (var dbContext = new NotatnikEntities())
            {
                var notatka = dbContext.Notatka
                    .Where(n => n.Id == id)
                    .FirstOrDefault();
                
                dbContext.Entry(notatka).State = System.Data.Entity.EntityState.Deleted;
                dbContext.SaveChanges();
            }

            return Ok();
        }
    }
}

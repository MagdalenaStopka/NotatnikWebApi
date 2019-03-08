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
               return BadRequest("Nieprawdiłowe dane");

            using (var dbContext = new NotatnikEntities())
            {
                var entity = new Notatka
                {
                    Tresc = notatka.Tresc,
                    Tytul = notatka.Tytul,
                    DataDodania = DateTime.Now
                };
                if (!string.IsNullOrEmpty(entity.Tytul) && !string.IsNullOrEmpty(entity.Tresc))
                {
                    dbContext.Notatka.Add(entity);
                    dbContext.SaveChanges();
                }
                else
                {
                    return BadRequest("nieprawidłowe dane- tytuł lub treść notatki nie może być pusta");
                }
            }

            return Ok();
        }

        public IHttpActionResult Delete(int id)
        {
            if (id <= 0)
                return BadRequest("Nieprawidłowe id notatki");

            using (var dbContext = new NotatnikEntities())
            {
                var notatka = dbContext.Notatka
                    .Where(n => n.Id == id)
                    .FirstOrDefault();
                if (notatka == null)
                {
                    return NotFound();
                }
                else
                {
                    dbContext.Notatka.Remove(notatka);
                    dbContext.SaveChanges();
                }
            }

            return Ok();
        }

        public IHttpActionResult PatchNotatka(NotatkaViewModel notatkaVM)
        {
            if (!notatkaVM.Id.HasValue)
                return BadRequest("Brak id notatki do edycji");

            using (var dbContext = new NotatnikEntities())
            {
                var notatka = dbContext.Notatka.FirstOrDefault(n => n.Id == notatkaVM.Id.Value);
                if (notatka == null)
                    return NotFound();

                if (!string.IsNullOrEmpty(notatkaVM.Tresc))
                    notatka.Tresc = notatkaVM.Tresc;

                if (!string.IsNullOrEmpty(notatkaVM.Tytul))
                    notatka.Tytul = notatkaVM.Tytul;

                //notatka.DataDodania = DateTime.Now;
                dbContext.SaveChanges();
                return Ok();
            }
        }
    }
}


    

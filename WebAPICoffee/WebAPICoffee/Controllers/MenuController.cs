using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebAPICoffee.Models;
using Newtonsoft.Json;

namespace WebAPICoffee.Controllers
{
    public class MenuController : ApiController
    {
        private CoffeeHouseEntities db = new CoffeeHouseEntities();

        // GET: api/Menu
        public IHttpActionResult GetМеню()
        {
            var меню = db.Меню
                .Select(m => new
                {
                    m.IDТовара,
                    m.Название,
                    m.Категория,
                    m.Описание,
                    m.Цена,
                    m.Доступность,
                    m.ВремяПриготовления,
                    m.ФотоТовара,
                    m.ДатаДобавления,
                    m.ДатаИзменения
                })
                .ToList();

            return Ok(меню);
        }

        // GET: api/Menu/5
        [ResponseType(typeof(Меню))]
        public IHttpActionResult GetМеню(int id)
        {
            var меню = db.Меню
                .Where(m => m.IDТовара == id)
                .Select(m => new
                {
                    m.IDТовара,
                    m.Название,
                    m.Категория,
                    m.Описание,
                    m.Цена,
                    m.Доступность,
                    m.ВремяПриготовления,
                    m.ФотоТовара,
                    m.ДатаДобавления,
                    m.ДатаИзменения
                })
                .FirstOrDefault();

            if (меню == null)
            {
                return NotFound();
            }

            return Ok(меню);
        }

        // POST: api/Menu
        [ResponseType(typeof(Меню))]
        public IHttpActionResult PostМеню(Меню меню)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Меню.Add(меню);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = меню.IDТовара }, new
            {
                меню.IDТовара,
                меню.Название,
                меню.Категория,
                меню.Описание,
                меню.Цена,
                меню.Доступность,
                меню.ВремяПриготовления,
                меню.ФотоТовара,
                меню.ДатаДобавления,
                меню.ДатаИзменения
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
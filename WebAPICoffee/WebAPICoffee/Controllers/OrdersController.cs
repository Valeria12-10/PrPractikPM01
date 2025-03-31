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

namespace WebAPICoffee.Controllers
{
    public class OrdersController : ApiController
    {
        private CoffeeHouseEntities db = new CoffeeHouseEntities();

        // GET: api/Order
        public IHttpActionResult GetЗаказы()
        {
            var заказы = db.Заказы
                .Select(z => new
                {
                    z.IDЗаказа,
                    z.ТипЗаказа,
                    z.Статус,
                    z.ВремяСоздания,
                    z.ВремяЗавершения,
                    z.ИтоговаяСумма,
                    z.СпособОплаты,
                    z.КомментарийКлиента,
                    z.IDСотрудника,
                    ПозицииЗаказа = z.ПозицииЗаказа.Select(p => new
                    {
                        p.IDПозиции,
                        p.IDЗаказа,
                        p.IDТовара,
                        p.Количество,
                        p.Модификация,
                        p.ЦенаНаМомент
                    })
                })
                .ToList();

            return Ok(заказы);
        }

        // GET: api/Orders/5
        [ResponseType(typeof(Заказы))]
        public IHttpActionResult GetЗаказы(int id)
        {
            var заказы = db.Заказы
                  .Where(m => m.IDЗаказа == id)
                .Select(z => new
                {
                    z.IDЗаказа,
                    z.ТипЗаказа,
                    z.Статус,
                    z.ВремяСоздания,
                    z.ВремяЗавершения,
                    z.ИтоговаяСумма,
                    z.СпособОплаты,
                    z.КомментарийКлиента,
                    z.IDСотрудника,
                    ПозицииЗаказа = z.ПозицииЗаказа.Select(p => new
                    {
                        p.IDПозиции,
                        p.IDЗаказа,
                        p.IDТовара,
                        p.Количество,
                        p.Модификация,
                        p.ЦенаНаМомент
                    })
                })
                 .FirstOrDefault();

            if (заказы == null)
            {
                return NotFound();
            }

            return Ok(заказы);
        }

       
        // POST: api/Orders
        [ResponseType(typeof(Заказы))]
        public IHttpActionResult PostЗаказы(Заказы заказы)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Заказы.Add(заказы);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = заказы.IDЗаказа }, заказы);
        }

       
    }
}
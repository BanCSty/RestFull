using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RestFull.Migrations;
using RestFull.Models;

using System.Xml.Linq;

namespace RestFull.Controllers
{
    public class BookingController : Controller
    {
        private readonly SetDbContext _context;

        public BookingController(SetDbContext context)
        {
            _context = context;
        }

        #region Booking

        //GET: ServiceObjects/booking
        public async Task<IActionResult> Index()
        {
            return View(await _context.Orders.ToListAsync());
        }

        //POST: ServiceObjects/order?{id=...}&{amount=...}
        [HttpPost]
        public async Task<IActionResult> OrderAsync(string id, int amount)
        {
            if (_context.serviceObjects == null)
            {
                return Problem("Entity set 'SetDbContext.serviceObjects' is null.");
            }

            //витягиваем объект с БД
            var serviceObject = await _context.serviceObjects
                .FirstOrDefaultAsync(m => m.ID == id);
            if (serviceObject == null)
                return NotFound();


            //При положительном Amount
            if (amount >= 0)
            {
                if (serviceObject.Amount - amount >= 0)
                    serviceObject.Amount = serviceObject.Amount - amount;

                else
                    return Json("Error: Balance exceeded!");
            }
            else
                return BadRequest("Value must be positive");

            //Присваиваем значения для сохранения в бд
            var booking = new Order();
            booking.ID = id;
            booking.NameOrder = serviceObject.Name;
            booking.AmountOrder = amount;

            try
            {
                //Обновляем показатель Amount для услуги
                _context.Update(serviceObject);
                //Сохраняем заказ в БД
                _context.Add(booking);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                //Если не найдено ID в БД
                if (!ServiceObjectExists(serviceObject.ID))
                {
                    throw new ArgumentException("Not found ID");
                }
                else
                {
                    return Problem(e.Message);
                    throw;
                }
            }
            return Json(booking);
        }

        [HttpGet]
        public IActionResult BookingOrder(string id)
        {
            return View();
        }

        #endregion

        #region DeleteOrder
        // GET: ServiceObjects/Delete/{id}
        [HttpGet, ActionName("DeleteOrder")]
        public async Task<IActionResult> DeleteOrderAync(int id)
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }

            var orders = await _context.Orders
                .FirstOrDefaultAsync(m => m.idMain == id);
            if (orders == null)
            {
                return NotFound();
            }

            return View(orders);
        }

        // POST: ServiceObjects/DeleteOrder/{id}
        [HttpPost, ActionName("DeleteOrder")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteOrderConfirmedAsync(int id)
        {
            if (_context.Orders == null)
            {
                return Problem("Entity set 'SetDbContext.order' is null.");
            }
            var orders = await _context.Orders.FindAsync(id);
            if (orders != null)
            {
                _context.Orders.Remove(orders);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //POST: ServiceObjects/DeleteJSON&{id}
        [HttpPost, ActionName("DeleteOrderJSON")]
        public async Task<IActionResult> DeleteOrderWithJSONAsync(int id)
        {
            if (_context.Orders == null)
            {
                return Problem("Entity set 'SetDbContext.serviceObjects'  is null.");
            }
            var orders = await _context.Orders.FindAsync(id);
            if (orders != null)
            {
                _context.Orders.Remove(orders);
            }

            await _context.SaveChangesAsync();
            return Json(orders);
        }
        #endregion

        private bool ServiceObjectExists(string id)
        {
            return _context.serviceObjects.Any(e => e.ID == id);
        }
    }
}

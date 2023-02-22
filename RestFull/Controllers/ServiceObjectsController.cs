using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestFull.Migrations;
using RestFull.Models;


namespace RestFull.Controllers
{
    public class ServiceObjectsController : Controller
    {
        private readonly SetDbContext _context;

        public ServiceObjectsController(SetDbContext context)
        {
            _context = context;
        }

        // GET: ServiceObjects
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _context.serviceObjects.ToListAsync());
        }

        #region Details
        // GET: ServiceObjects/Details/{id}
        [HttpGet, ActionName("Details")]
        public async Task<IActionResult> DetailsAync(string id)
        {
            if (id == null || _context.serviceObjects == null)
            {
                return NotFound();
            }

            var serviceObject = await _context.serviceObjects
                .FirstOrDefaultAsync(m => m.ID == id);
            if (serviceObject == null)
            {
                return NotFound();
            }

            return View(serviceObject);
        }
        //GET ServiceObjects/Read/{id}
        //With JSON response
        [HttpGet, ActionName("Read")]
        public async Task<IActionResult> ReadAsync(string id)
        {
            if (id == null || _context.serviceObjects == null)
            {
                return NotFound();
            }

            var serviceObject = await _context.serviceObjects
                .FirstOrDefaultAsync(m => m.ID == id);
            if (serviceObject == null)
            {
                return NotFound();
            }
            return Json(serviceObject);
        }
        #endregion

        #region Create
        // GET: ServiceObjects/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: ServiceObjects/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Amount,Name")] ServiceObject serviceObject)
        {
            if (_context.serviceObjects == null)
            {
                return Problem("Entity set 'SerDbContext.serviceObjects'  is null.");
            }

            try
            {
                if (serviceObject.Name == null && serviceObject.Amount < 0)
                    throw new ArgumentNullException("Bad value!");
            }
            catch (NullReferenceException e)
            {
                return BadRequest(e.Message);
                throw;
            }
            //т.к Light SQL не поддерживает GUID
            ServiceObject service = new ServiceObject();

            Guid g = Guid.NewGuid();

            service.Name = serviceObject.Name;
            service.Amount = serviceObject.Amount;
            service.ID = g.ToString();

            try
            {
                if (ServiceObjectExists(service.ID))
                    throw new Exception("Id already exists.");

                _context.Add(service);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        //POST: ServiceObject/CreateJSON?{name}&{amount?}
        [HttpPost, ActionName("CreateJSON")]
        public async Task<IActionResult> CreateWithJSONAsync(string name, int amount = 0)
        {
            if (_context.serviceObjects == null)
            {
                return Problem("Entity set 'SerDbContext.serviceObjects'  is null.");
            }

            try
            {
                if (name == null && amount < 0)
                    throw new ArgumentNullException("Bad value!");
            }
            catch (NullReferenceException e)
            {
                return BadRequest(e.Message);
                throw;
            }

            //т.к Light SQL не поддерживает GUID
            ServiceObject service = new ServiceObject();

            Guid g = Guid.NewGuid();

            service.Name = name;
            service.Amount = amount;
            service.ID = g.ToString();

            try
            {
                if (ServiceObjectExists(service.ID))
                    throw new Exception("Id already exists.");

                _context.Add(service);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }

            return Json(service);
        }
        #endregion

        #region Edit
        [HttpGet]
        // GET: ServiceObjects/Edit/{id}
        public async Task<IActionResult> EditAsync(string id)
        {
            if (id == null || _context.serviceObjects == null)
            {
                return NotFound();
            }

            var serviceObject = await _context.serviceObjects.FindAsync(id);
            if (serviceObject == null)
            {
                return NotFound();
            }
            return View(serviceObject);
        }

        // POST: ServiceObjects/Edit/{id}
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(string id, [Bind("ID,Amount,Name")] ServiceObject serviceObject)
        {
            if (id != serviceObject.ID)
            {
                return NotFound();
            }

            try
            {
                _context.Update(serviceObject);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceObjectExists(serviceObject.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        //POST services/EditJSON?{id}&{name?}&{amount?}
        [HttpPost, ActionName("EditJSON")]
        public async Task<IActionResult> EditWithJSONAsync(string id, string? name, int amount = 0)
        {
            if (_context.serviceObjects == null)
            {
                return Problem("Entity set 'SetDbContext.serviceObjects' is null.");
            }

            try
            {
                if (name == null && amount < 0)
                    throw new ArgumentNullException("Bad value!");
            }
            catch (NullReferenceException e)
            {
                return BadRequest(e.Message);
                throw;
            }

            var serviceObject = await _context.serviceObjects
                .FirstOrDefaultAsync(m => m.ID == id);

            serviceObject.Amount = amount;

            try
            {
                _context.Update(serviceObject);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceObjectExists(serviceObject.ID))
                {
                    throw new ArgumentException("Not found ID");
                }
                else
                {
                    throw;
                }
            }
            return Json(serviceObject);
        }
        #endregion

        #region Delete
        // GET: ServiceObjects/Delete/{id}
        [HttpGet, ActionName("Delete")]
        public async Task<IActionResult> DeleteAync(string id)
        {
            if (id == null || _context.serviceObjects == null)
            {
                return NotFound();
            }

            var serviceObject = await _context.serviceObjects
                .FirstOrDefaultAsync(m => m.ID == id);
            if (serviceObject == null)
            {
                return NotFound();
            }

            return View(serviceObject);
        }

        // POST: ServiceObjects/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedAsync(string id)
        {
            if (_context.serviceObjects == null)
            {
                return Problem("Entity set 'SetDbContext.serviceObjects'  is null.");
            }
            var serviceObject = await _context.serviceObjects.FindAsync(id);
            if (serviceObject != null)
            {
                _context.serviceObjects.Remove(serviceObject);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //POST: ServiceObject/DeleteJSON&{id}
        [HttpPost, ActionName("DeleteJSON")]
        public async Task<IActionResult> DeleteWithJSONAsync(string id)
        {
            if (_context.serviceObjects == null)
            {
                return Problem("Entity set 'SetDbContext.serviceObjects'  is null.");
            }
            var serviceObject = await _context.serviceObjects.FindAsync(id);
            if (serviceObject != null)
            {
                _context.serviceObjects.Remove(serviceObject);
            }

            await _context.SaveChangesAsync();
            return Json(serviceObject);
        }
        #endregion

        //Переадресация на другой контроллер
        [HttpGet]
        public IActionResult Booking(string id)
        {
            return RedirectToAction("BookingOrder", "Booking",new { id });
        }

        //Проверка существования объекта по ID в БД
        private bool ServiceObjectExists(string id)
        {
            return _context.serviceObjects.Any(e => e.ID == id);
        }
    }
}

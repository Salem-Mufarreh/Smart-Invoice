using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Smart_Invoice.Data;
using Smart_Invoice.Models.Warehouse;
using Smart_Invoice.Utility;

namespace Smart_Invoice.Areas.Accountant.Controllers
{
    [Area("Accountant")]
    public class WarehousesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WarehousesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Accountant/Warehouses
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Warehouses.Include(w => w.WarehouseProducts);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Accountant/Warehouses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Warehouses == null)
            {
                return NotFound();
            }

            var warehouse = await _context.Warehouses
                .Include(w => w.WarehouseProducts)
                .FirstOrDefaultAsync(m => m.WarehouseId == id);
            if (warehouse == null)
            {
                return NotFound();
            }

            return View(warehouse);
        }

        // GET: Accountant/Warehouses/Create
        public IActionResult Create()
        {
            var warehouse = _context.Warehouses.OrderByDescending(w => w.WarehouseCode).FirstOrDefault();
            if (warehouse != null && warehouse.WarehouseCode != null)
            {
                int wCode = int.Parse(warehouse.WarehouseCode.ToString());
                int nextCode = wCode + 1;
                ViewBag.WarehouseCode = nextCode.ToString("D3");
            }
            return View();
        }

        // POST: Accountant/Warehouses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WarehouseId,WarehouseName,WarehouseCode,Address,Capacity")] Warehouse warehouse)
        {
            if (ModelState.IsValid)
            {
                warehouse.Status = SD.WarehouseActive;
                warehouse.AvailableSpace = warehouse.Capacity;
                warehouse.OccupancyRate = 0;
                _context.Add(warehouse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var Wwarehouse = _context.Warehouses.OrderByDescending(w => w.WarehouseCode).FirstOrDefault();
                if (Wwarehouse != null && Wwarehouse.WarehouseCode != null)
                {
                    int wCode = int.Parse(Wwarehouse.WarehouseCode.ToString());
                    int nextCode = wCode + 1;
                    ViewBag.WarehouseCode = nextCode.ToString("D3");
                }
            }
            return View(warehouse);
        }

        // GET: Accountant/Warehouses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Warehouses == null)
            {
                return NotFound();
            }

            var warehouse = await _context.Warehouses.FindAsync(id);
            if (warehouse == null)
            {
                return NotFound();
            }
            
            return View(warehouse);
        }

        // POST: Accountant/Warehouses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("WarehouseId,WarehouseName,WarehouseCode,Address,Capacity,Status")] Warehouse warehouse)
        {
            if (id != warehouse.WarehouseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var oldWarehouse = await _context.Warehouses.FindAsync(id);
                    if (oldWarehouse != null)
                    {
                        oldWarehouse.WarehouseName = warehouse.WarehouseName ?? oldWarehouse.WarehouseName;
                        oldWarehouse.OccupancyRate = warehouse.OccupancyRate ?? oldWarehouse.OccupancyRate;
                        oldWarehouse.Address = warehouse.Address ?? oldWarehouse.Address;
                        oldWarehouse.Capacity = warehouse.Capacity ?? oldWarehouse.Capacity;
                        oldWarehouse.Status = warehouse.Status ?? oldWarehouse.Status;
                        _context.Update(oldWarehouse);

                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WarehouseExists(warehouse.WarehouseId))
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
            return View(warehouse);
        }

        // GET: Accountant/Warehouses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Warehouses == null)
            {
                return NotFound();
            }

            var warehouse = await _context.Warehouses
                .Include(w => w.WarehouseProducts)
                .FirstOrDefaultAsync(m => m.WarehouseId == id);
            if (warehouse == null)
            {
                return NotFound();
            }

            return View(warehouse);
        }

        // POST: Accountant/Warehouses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Warehouses == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Warehouses'  is null.");
            }
            var warehouse = await _context.Warehouses.FindAsync(id);
            if (warehouse != null)
            {
                _context.Warehouses.Remove(warehouse);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WarehouseExists(int id)
        {
          return (_context.Warehouses?.Any(e => e.WarehouseId == id)).GetValueOrDefault();
        }
    }
}

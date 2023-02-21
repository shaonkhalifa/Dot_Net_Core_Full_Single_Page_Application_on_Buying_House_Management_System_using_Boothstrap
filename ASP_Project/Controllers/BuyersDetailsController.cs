using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASP_Project.Models;

namespace ASP_Project.Controllers
{
    public class BuyersDetailsController : Controller
    {
        private readonly BuyingHouseDbContex _context;

        public BuyersDetailsController(BuyingHouseDbContex context)
        {
            _context = context;
        }

        // GET: BuyersDetails
        public async Task<IActionResult> Index()
        {
            ViewBag.orders =await _context.Order.ToListAsync();
            return PartialView( _context.Buyers.OrderByDescending(x => x.BuyerId).ToList());
            //return View(await _context.Buyers.ToListAsync());
        }

        // GET: BuyersDetails/Create
        public IActionResult Create()
        {
            return PartialView(new Buyer());
        }

 
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BuyerId,BuyerName,BuyerAddress")] Buyer buyer)
        {
            //if (ModelState.IsValid && (c.CustomerName != null && c.CustomerAddress != null))
            //{
            //    db.Customers.Add(c);
            //    db.SaveChanges();
            //    return PartialView("_success");
            //}
            //return PartialView("_error");
            if (ModelState.IsValid && (buyer.BuyerName!=null && buyer.BuyerAddress!=null))
            {
                _context.Add(buyer);
                await _context.SaveChangesAsync();
                return Ok("success");
            }
            return Ok("failed");
        }

        // GET: BuyersDetails/Edit/5
        public async Task<IActionResult> Edit(int id)
        {

            return PartialView(await _context.Buyers.FirstAsync(x => x.BuyerId == id));
            //if (id == null || _context.Buyers == null)
            //{
            //    return NotFound();
            //}

            //var buyer = await _context.Buyers.FindAsync(id);
            //if (buyer == null)
            //{
            //    return NotFound();
            //}
            //return View(buyer);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BuyerId,BuyerName,BuyerAddress")] Buyer buyer)
        {


            if (ModelState.IsValid)
            {
                _context.Update(buyer);
                await _context.SaveChangesAsync();
                return Ok("success");
            }
            return Ok("failed");
            //if (id != buyer.BuyerId)
            //{
            //    return NotFound();
            //}

            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        _context.Update(buyer);
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (!BuyerExists(buyer.BuyerId))
            //        {
            //            return NotFound();
            //        }
            //        else
            //        {
            //            throw;
            //        }
            //    }
            //    return RedirectToAction(nameof(Index));
            //}
            //return View(buyer);
        }

        // GET: BuyersDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                Buyer b = new Buyer() { BuyerId = (int)id };
                _context.Remove(b);
                await _context.SaveChangesAsync();
                return Ok("success");
            }
            return Ok("failed");
            //if (id == null || _context.Buyers == null)
            //{
            //    return NotFound();
            //}

            //var buyer = await _context.Buyers
            //    .FirstOrDefaultAsync(m => m.BuyerId == id);
            //if (buyer == null)
            //{
            //    return NotFound();
            //}

            //return View(buyer);
        }

        // POST: BuyersDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Buyers == null)
            {
                return Problem("Entity set 'BuyingHouseDbContex.Buyers'  is null.");
            }
            var buyer = await _context.Buyers.FindAsync(id);
            if (buyer != null)
            {
                _context.Buyers.Remove(buyer);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BuyerExists(int id)
        {
          return _context.Buyers.Any(e => e.BuyerId == id);
        }
    }
}

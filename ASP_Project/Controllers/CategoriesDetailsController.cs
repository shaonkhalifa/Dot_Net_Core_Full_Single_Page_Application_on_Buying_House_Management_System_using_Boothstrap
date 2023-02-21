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
    public class CategoriesDetailsController : Controller
    {
        private readonly BuyingHouseDbContex _context;

        public CategoriesDetailsController(BuyingHouseDbContex context)
        {
            _context = context;
        }

        // GET: CategoriesDetails
        public async Task<IActionResult> Index()
        {
            ViewBag.products = _context.Products.ToList();
            return PartialView(await _context.Categories.OrderByDescending(x => x.CategoryId).ToListAsync());
            //return View(await _context.Categories.ToListAsync());
        }



        // GET: CategoriesDetails/Create
        public IActionResult Create()
        {
            return PartialView(new Category());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,CategoryName")] Category c)
        {

            if (ModelState.IsValid && (c.CategoryName != null))
            {
                _context.Categories.Add(c);
                await _context.SaveChangesAsync();
                return Ok("success");
            }
            return Ok("failed");

            //if (ModelState.IsValid)
            //{
            //    _context.Add(category);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //return View(category);
        }

      
        public async Task<IActionResult> Edit(int? id)
        {


            return PartialView(await _context.Categories.FirstAsync(x => x.CategoryId == id));
            //if (id == null || _context.Categories == null)
            //{
            //    return NotFound();
            //}

            //var category = await _context.Categories.FindAsync(id);
            //if (category == null)
            //{
            //    return NotFound();
            //}
            //return View(category);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit( Category c)
        {

            if (ModelState.IsValid && (c.CategoryName != null))
            {
                _context.Update(c);
               await _context.SaveChangesAsync();
                return Ok("success");
            }
            return Ok("failed");

            //if (id != category.CategoryId)
            //{
            //    return NotFound();
            //}

            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        _context.Update(category);
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (!CategoryExists(category.CategoryId))
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
            //return View(category);
        }

        // GET: CategoriesDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {


            if (id != null)
            {
                Category c = new Category() { CategoryId = (int)id };
                _context.Remove(c);
                await _context.SaveChangesAsync();
                return Ok("success");
            }
            return Ok("failed");
            //if (id == null || _context.Categories == null)
            //{
            //    return NotFound();
            //}

            //var category = await _context.Categories
            //    .FirstOrDefaultAsync(m => m.CategoryId == id);
            //if (category == null)
            //{
            //    return NotFound();
            //}

            //return View(category);
        }

        //// POST: CategoriesDetails/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    if (_context.Categories == null)
        //    {
        //        return Problem("Entity set 'BuyingHouseDbContex.Categories'  is null.");
        //    }
        //    var category = await _context.Categories.FindAsync(id);
        //    if (category != null)
        //    {
        //        _context.Categories.Remove(category);
        //    }
            
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool CategoryExists(int id)
        {
          return _context.Categories.Any(e => e.CategoryId == id);
        }
    }
}

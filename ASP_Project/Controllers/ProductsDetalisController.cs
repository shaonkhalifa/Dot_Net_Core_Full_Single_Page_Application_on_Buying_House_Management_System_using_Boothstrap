using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASP_Project.Models;
using ASP_Project.ViewModels;

namespace ASP_Project.Controllers
{
    public class ProductsDetalisController : Controller
    {
        private readonly BuyingHouseDbContex _context;
        private readonly IWebHostEnvironment _he;

        public ProductsDetalisController(BuyingHouseDbContex context, IWebHostEnvironment he)
        {
            _context = context;
            _he = he;
        }

        // GET: ProductsDetalis
        public async Task<IActionResult> Index()
        {

            ViewBag.orderDetails = await _context.orderDetails.ToListAsync();
            return PartialView(_context.Products.OrderByDescending(x => x.ProductId).Include(m=>m.Category).ToList());
        
        }



        // GET: ProductsDetalis/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
      
            return PartialView(new ProductVM());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductVM pvm)
        {
            ModelState.Remove("PicturePath");
            if (ModelState.IsValid)
            {
                Product pr = new Product()
                {
                    ProductName = pvm.ProductName,
                    CategoryId = pvm.CategoryId,
                    Price = pvm.Price,

                    ManufacturingDate = pvm.ManufacturingDate,
                    //PicturePath = filePath,
                    InStock = pvm.InStock
                };
                if (pvm.Picture != null)
                {
                  



                    var file = pvm.Picture;
                    string webroot = _he.WebRootPath;
                    string folder = "Images";
                    string imgFileName = Path.GetFileName(pvm.Picture.FileName);
                    string fileToSave = Path.Combine(webroot, folder, imgFileName);
                    if (file != null)
                    {
                        using (var strem = new FileStream(fileToSave, FileMode.Create))
                        {
                            pvm.Picture.CopyTo(strem);
                            pr.PicturePath = "/" + folder + "/" + imgFileName;
                        }

                    }

                    _context.Products.Add(pr);
                    await _context.SaveChangesAsync();
                    return Ok("success");
                }
                else
                {
                    pr.PicturePath = "/" + "Images" + "/" + "default.jpg";
                }
            }
            return Ok("failed");

        }

        // GET: ProductsDetalis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                
                var pr = await _context.Products.FindAsync(id);

                ProductVM pvm = new ProductVM()
                {
                    ProductId = pr.ProductId,
                    ProductName = pr.ProductName,
                    CategoryId = pr.CategoryId,
                    Price = pr.Price,
                    ManufacturingDate = pr.ManufacturingDate,
                    PicturePath = pr.PicturePath,
                    InStock = pr.InStock
                };
                 ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
             
                return PartialView(pvm);
            }
            return View();



        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductVM productVM)
        {
            //if (id != product.ProductId)
            //{
            //    return NotFound();
            //}
            ModelState.Remove("PicturePath");
            if (ModelState.IsValid)
            {
                Product pr = new Product()
                {
                    ProductId = productVM.ProductId,
                    ProductName = productVM.ProductName,
                    CategoryId = productVM.CategoryId,
                    Price = productVM.Price,

                    ManufacturingDate = productVM.ManufacturingDate,

                    InStock = productVM.InStock
                };
                if (productVM.Picture != null)
                {
                  

                    var file = productVM.Picture;
                    string webroot = _he.WebRootPath;
                    string folder = "Images";
                    string imgFileName = Path.GetFileName(productVM.Picture.FileName);
                    string fileToSave = Path.Combine(webroot, folder, imgFileName);
                    if (file != null)
                    {
                        using (var strem = new FileStream(fileToSave, FileMode.Create))
                        {
                            productVM.Picture.CopyTo(strem);
                            pr.PicturePath = "/" + folder + "/" + imgFileName;
                        }

                    }
                    //var ext = Path.GetExtension(productVM.Picture.FileName);
                    //var fileName = Path.Combine("~/Images/", Guid.NewGuid().ToString() + ext);
                    //productVM.Picture.SaveAs(Server.MapPath(fileName));
                    
                    _context.Update(pr);
                    await _context.SaveChangesAsync();
                    return Ok("success");
                }
               
            }
            else
            {
                //ModelState.Remove(productVM.PicturePath);
                Product pr = new Product()
                {
                    ProductId = productVM.ProductId,
                    ProductName = productVM.ProductName,
                    CategoryId = productVM.CategoryId,
                    Price = productVM.Price,
                    ManufacturingDate = productVM.ManufacturingDate,
                    PicturePath = productVM.PicturePath,
                    InStock = productVM.InStock
                };
                _context.Update(pr);
                await _context.SaveChangesAsync();
                return PartialView("_success");
            }
            return Ok("failed");
        }

        // GET: ProductsDetalis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var pr = _context.Products.Find(id);
            if (pr != null)
            {
                _context.Remove(pr);
               await _context.SaveChangesAsync();
                return Ok("success");
            }
            return Ok("failed");

        }

   

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}

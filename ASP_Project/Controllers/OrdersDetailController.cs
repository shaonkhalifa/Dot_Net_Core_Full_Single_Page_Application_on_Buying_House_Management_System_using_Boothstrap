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
    public class OrdersDetailController : Controller
    {
        private readonly BuyingHouseDbContex _context;

        public OrdersDetailController(BuyingHouseDbContex context)
        {
            _context = context;
        }

        // GET: OrdersDetail
        public async Task<IActionResult> Index()
        {

            return PartialView(await _context.Order.OrderByDescending(x => x.OrderId).Include(y=>y.OrderDetails).ThenInclude(r=>r.Product).Include(z=>z.Buyer).ToListAsync());
        }



        // GET: OrdersDetail/Create
        public IActionResult Create()
        {
            //ViewData["BuyerId"] = new SelectList(_context.Buyers, "BuyerId", "BuyerAddress");
            //return View();
            ViewBag.buyer = new SelectList(_context.Buyers, "BuyerId", "BuyerName");
            ViewBag.products = new SelectList(_context.Products, "ProductId", "ProductName");

            return PartialView();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order order, int[] singleProductId, int[] SingleProductQuantity)
        {
            ModelState.Remove("Buyer");
            if (ModelState.IsValid)
            {
                if (order.BuyerId != 0 && singleProductId.Count() > 0 && SingleProductQuantity.Count() > 0)
                {
                    //Buyer b = order.Buyer;
                    Order or = new Order()
                    {
                        BuyerId = order.BuyerId,


                    };
                    //_context.Order.Add(or);
                    for (int i = 0; i < singleProductId.Length; i++)
                    {
                        OrderDetail od = new OrderDetail()
                        {
                            Order=or,
                            OrderId = or.OrderId,
                            ProductId = singleProductId[i],
                            Price = _context.Products.Find(singleProductId[i]).Price,
                            Quantity = SingleProductQuantity[i]
                        };
                        _context.orderDetails.Add(od);
                    }
                    await _context.SaveChangesAsync();
                    return Ok("success");
                }
            }
            ViewBag.buyer = new SelectList(_context.Buyers, "BuyerId", "BuyerName");
            ViewBag.products = new SelectList(_context.Products, "ProductId", "ProductName");

            return Ok("failed");


        }

        public ActionResult Edit(int id)
        {
            //Order currentOrder = db.Orders.First(x => x.OrderId == id);

            Order currentOrder = _context.Order.Where(x => x.OrderId == id).Include(x => x.Buyer).Single();
            var orderDetailsList = _context.orderDetails.Where(x => x.OrderId == currentOrder.OrderId);

            ViewBag.customers = new SelectList(_context.Buyers, "ProductId", "ProductName");
            ViewBag.products = _context.Products.ToList();

            OrderVM orderVM = new OrderVM() { Order = currentOrder };

            foreach (var item in orderDetailsList)
            {
                orderVM.OrderDetails.Add(new OrderDetailVM()
                {
                    OrderDetailsId = item.OrderDetailId,
                    OrderId = item.OrderId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price
                });
            }

            return PartialView(orderVM);
        }

        [HttpPost]
        public ActionResult Edit(OrderVM orderVM, int[] ProductId, int[] Quantity)
        {
            if (orderVM.OrderDetails != null || (ProductId != null && ProductId.Length > 0))
            {

                if (orderVM.OrderDetails.Count() > 0)
                {
                    foreach (var item in orderVM.OrderDetails)
                    {
                        OrderDetail postedOrder = new OrderDetail()
                        {
                            OrderDetailId = item.OrderDetailsId,
                            OrderId = orderVM.Order.OrderId,
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                            Price = _context.Products.Find(item.ProductId).Price
                        };

                        if (item.Delete == true)
                        {
                            _context.Entry(postedOrder).State = EntityState.Deleted;
                            continue;
                        }

                        _context.Entry(postedOrder).State = EntityState.Modified;
                    }
                    _context.SaveChanges();
                }


                //Add newly added items
                if (ProductId != null && ProductId.Length > 0)
                {
                    for (int i = 0; i < ProductId.Length; i++)
                    {
                        if (ProductId[i] == 0)
                        {
                            continue;
                        }
                        OrderDetail orderDetail = new OrderDetail()
                        {
                            OrderId = orderVM.Order.OrderId,
                            ProductId = ProductId[i],
                            Quantity = Quantity[i],
                            Price = _context.Products.Find(ProductId[i]).Price,
                        };

                        _context.orderDetails.Add(orderDetail); ;
                    }
                    _context.SaveChanges();
                }
                return Ok("success");
            }

            return Ok("failed");
        }





        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(OrderVM orderVM, int[] ProductId, int[] Quantity)
        //{
        //    if (orderVM.OrderDetails != null || (ProductId != null && ProductId.Length > 0))
        //    {

        //        if (orderVM.OrderDetails.Count() > 0)
        //        {
        //            foreach (var item in orderVM.OrderDetails)
        //            {
        //                OrderDetail postedOrder = new OrderDetail()
        //                {
        //                    OrderDetailId = item.OrderDetailsId,
        //                    OrderId = orderVM.Order.OrderId,
        //                    ProductId = item.ProductId,
        //                    Quantity = item.Quantity,
        //                    Price = _context.Products.Find(item.ProductId).Price
        //                };

        //                if (item.Delete == true)
        //                {
        //                    _context.Entry(postedOrder).State = EntityState.Deleted;
        //                    continue;
        //                }

        //                _context.Entry(postedOrder).State = EntityState.Modified;
        //            }
        //           await _context.SaveChangesAsync();
        //        }


        //        //Add newly added items
        //        if (ProductId != null && ProductId.Length > 0)
        //        {
        //            for (int i = 0; i < ProductId.Length; i++)
        //            {
        //                if (ProductId[i] == 0)
        //                {
        //                    continue;
        //                }
        //                OrderDetail orderDetail = new OrderDetail()
        //                {
        //                    OrderId = orderVM.Order.OrderId,
        //                    ProductId = ProductId[i],
        //                    Quantity = Quantity[i],
        //                    Price = _context.Products.Find(ProductId[i]).Price,
        //                };

        //                _context.orderDetails.Add(orderDetail); ;
        //            }
        //            await _context.SaveChangesAsync();
        //        }
        //        return Ok("success");
        //    }

        //    return Ok("failed");
        //if (orderVM.OrderDetails.Count() >= 0 || (ProductId != null && Quantity != null))
        //{
        //    if (orderVM.OrderDetails.Count() == 0)
        //    {
        //        var orderDetailsList = _context.orderDetails.Where(x => x.OrderId == orderVM.Order.OrderId);
        //        foreach (var item in orderDetailsList)
        //        {
        //            _context.Remove(item);
        //        }
        //        await _context.SaveChangesAsync();
        //    }
        //    if (orderVM.OrderDetails.Count() > 0)
        //    {
        //        //Update productDetails if exists
        //        foreach (var item in orderVM.OrderDetails)
        //        {
        //            _context.Update(item);
        //        }

        //        //delete removed items
        //        var removedItem = _context.orderDetails.Where(x => x.OrderId == orderVM.Order.OrderId).ToList().Except(orderVM.OrderDetails).ToList();

        //        foreach (var item in removedItem)
        //        {
        //            _context.Remove(item);
        //        }

        //        await _context.SaveChangesAsync();
        //    }

        //    //Add newly added items
        //    if (ProductId != null)
        //    {
        //        for (int i = 0; i < ProductId.Length; i++)
        //        {
        //            if (String.IsNullOrEmpty(ProductId[i].ToString()) && String.IsNullOrEmpty(Quantity[i].ToString()))
        //            {
        //                continue;
        //            }
        //            OrderDetail orderDetail = new OrderDetail()
        //            {
        //                OrderId = orderVM.Order.OrderId,
        //                ProductId = ProductId[i],
        //                Quantity = Quantity[i],
        //                Price = _context.Products.Find(ProductId[i]).Price,
        //            };

        //            _context.orderDetails.Add(orderDetail); ;
        //        }
        //        await _context.SaveChangesAsync();
        //    }
        //    return PartialView("_success");
        //}
        //return PartialView("_error");




        //if (id != order.OrderId)
        //{
        //    return NotFound();
        //}

        //if (ModelState.IsValid)
        //{
        //    try
        //    {
        //        _context.Update(order);
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!OrderExists(order.OrderId))
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
        //ViewData["BuyerId"] = new SelectList(_context.Buyers, "BuyerId", "BuyerAddress", order.BuyerId);
        //return View(order);
    

        // GET: OrdersDetail/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var order = _context.Order.Find(id);
            if (order != null)
            {
                var orderDetail = _context.orderDetails.Where(x => x.OrderId == id);
                if ((orderDetail != null) && (orderDetail.Count() > 0))
                {
                    foreach (var item in orderDetail)
                    {
                        _context.Update(item);
                    }
                    await _context.SaveChangesAsync();

                    _context.RemoveRange(order);
                    await _context.SaveChangesAsync();
                    return PartialView("_success");
                }
                return Ok("success");
            }
            return Ok("failed");



            //if (id == null || _context.Order == null)
            //{
            //    return NotFound();
            //}

            //var order = await _context.Order
            //    .Include(o => o.Buyer)
            //    .FirstOrDefaultAsync(m => m.OrderId == id);
            //if (order == null)
            //{
            //    return NotFound();
            //}

            //return View(order);
        }

        //// POST: OrdersDetail/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    if (_context.Order == null)
        //    {
        //        return Problem("Entity set 'BuyingHouseDbContex.Order'  is null.");
        //    }
        //    var order = await _context.Order.FindAsync(id);
        //    if (order != null)
        //    {
        //        _context.Order.Remove(order);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}



        public IActionResult SingleProductEntry()
        {
            ViewBag.products = new SelectList(_context.Products, "ProductId", "ProductName");
            return PartialView();
        }
        public IActionResult SingleProductEditEntry()
        {
            //ViewBag.customers = new SelectList(_context.Customers, "CustomerId", "CustomerName");
            //ViewBag.products = new SelectList(db.Products, "ProductId", "ProductName");

            ViewBag.buyer = new SelectList(_context.Buyers, "BuyerId", "BuyerName");
            ViewBag.products = new SelectList(_context.Products, "ProductId", "ProductName");

            return PartialView(new OrderDetail());
        }
        public JsonResult GetFee(int id)
        {
            var t = _context.Products.FirstOrDefault(x => x.ProductId == id);
            return Json(t == null ? 0 : t.Price);
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.OrderId == id);
        }
    }
}

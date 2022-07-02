using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Internet_veikals.Models;
using Microsoft.AspNetCore.Authorization;

namespace Internet_veikals.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrderProductsController : Controller
    {
        private readonly SimpleContext _context;

        public OrderProductsController(SimpleContext context)
        {
            _context = context;
        }

        // GET: OrderProducts
        public async Task<IActionResult> Index()
        {
            var simpleContext = _context.OrderProducts.Include(o => o.Order).Include(o => o.Product);
            return View("~/Views/Admin/OrderProducts/index.cshtml",await simpleContext.ToListAsync());
        }

        // GET: OrderProducts/Details/5
        public async Task<IActionResult> Details(int? idO, int? idP)
        {
            if (idO == null || idP == null)
            {
                return NotFound();
            }

            var orderProduct = await _context.OrderProducts
                .Include(o => o.Order)
                .Include(o => o.Product)
                .FirstOrDefaultAsync(m => m.ProductId == idP && m.OrderId == idO);
            if (orderProduct == null)
            {
                return NotFound();
            }

            return View("~/Views/Admin/OrderProducts/Details.cshtml",orderProduct);
        }

        // GET: OrderProducts/Create
        public IActionResult Create()
        {
            ViewData["OrderId"] = new SelectList(_context.Orders, nameof(Order.Id), nameof(Order.Id));
            ViewData["ProductId"] = new SelectList(_context.Products, nameof(Product.Id), nameof(Product.ProductName));
            return View("~/Views/Admin/OrderProducts/Create.cshtml");
        }

        // POST: OrderProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,OrderId,Qty")] OrderProduct orderProduct)
        {
            try 
            {
                if (ModelState.IsValid)
                {
                    _context.Add(orderProduct);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ViewData["OrderId"] = new SelectList(_context.Orders, nameof(Order.Id), nameof(Order.Id), orderProduct.OrderId);
                ViewData["ProductId"] = new SelectList(_context.Products, nameof(Product.Id), nameof(Product.ProductName), orderProduct.ProductId);
                return View("~/Views/Admin/OrderProducts/Create.cshtml", orderProduct);
            }
            catch (DbUpdateException)
            {
                var simpleContext = _context.OrderProducts.Include(o => o.Order).Include(o => o.Product);
                return View("~/Views/Admin/OrderProducts/index.cshtml", await simpleContext.ToListAsync());
            }
        }

        // GET: OrderProducts/Edit/5
        public async Task<IActionResult> Edit(int? idO, int? idP)
        {
            if (idO == null || idP == null)
            {
                return NotFound();
            }

            var orderProduct = await _context.OrderProducts.FindAsync(idP,idO);
            if (orderProduct == null)
            {
                return NotFound();
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, nameof(Order.Id), nameof(Order.Id), orderProduct.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Products, nameof(Product.Id), nameof(Product.ProductName), orderProduct.ProductId);
            return View("~/Views/Admin/OrderProducts/Edit.cshtml",orderProduct);
        }

        // POST: OrderProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("ProductId,OrderId,Qty")] OrderProduct orderProduct)
        {


            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderProductExists(orderProduct.ProductId))
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
            ViewData["OrderId"] = new SelectList(_context.Orders, nameof(Order.Id), nameof(Order.Id), orderProduct.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Products, nameof(Product.Id), nameof(Product.ProductName), orderProduct.ProductId);
            return View("~/Views/Admin/OrderProducts/Edit.cshtml", orderProduct);
        }

        // GET: OrderProducts/Delete/5
        public async Task<IActionResult> Delete(int? idO, int? idP)
        {
            if (idO == null || idP == null)
            {
                return NotFound();
            }

            var orderProduct = await _context.OrderProducts
                .Include(o => o.Order)
                .Include(o => o.Product)
                .FirstOrDefaultAsync(m => m.ProductId == idP && m.OrderId == idO);
            if (orderProduct == null)
            {
                return NotFound();
            }

            return View("~/Views/Admin/OrderProducts/Delete.cshtml",orderProduct);
        }

        // POST: OrderProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed([Bind("ProductId,OrderId,Qty")] OrderProduct orderProduct)
        {
            _context.OrderProducts.Remove(orderProduct);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderProductExists(int id)
        {
            return _context.OrderProducts.Any(e => e.ProductId == id);
        }
    }
}

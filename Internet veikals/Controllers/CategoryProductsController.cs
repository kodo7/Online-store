using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Internet_veikals.Models;
using Microsoft.AspNetCore.Authorization;
using Npgsql;

namespace Internet_veikals.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryProductsController : Controller
    {
        private readonly SimpleContext _context;

        public CategoryProductsController(SimpleContext context)
        {
            _context = context;
        }

        // GET: CategoryProducts
        public async Task<IActionResult> Index()
        {
            var simpleContext = _context.CategoryProducts.Include(c => c.Category).Include(c => c.Product);
            return View("~/Views/Admin/CategoryProducts/index.cshtml",await simpleContext.ToListAsync());
        }

        // GET: CategoryProducts/Details/5
        public async Task<IActionResult> Details(int? idC, int? idP)
        {
            if (idC == null || idP == null)
            {
                return NotFound();
            }

            var categoryProduct = await _context.CategoryProducts
                .Include(c => c.Category)
                .Include(c => c.Product)
                .FirstOrDefaultAsync(m => m.ProductId == idP && m.CategoryId == idC);
            if (categoryProduct == null)
            {
                return NotFound();
            }

            return View("~/Views/Admin/CategoryProducts/Details.cshtml",categoryProduct);
        }

        // GET: CategoryProducts/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, nameof(Category.Id), nameof(Category.CategoryName));
            ViewData["ProductId"] = new SelectList(_context.Products, nameof(Product.Id), nameof(Product.ProductName));
            return View("~/Views/Admin/CategoryProducts/Create.cshtml");
        }

        // POST: CategoryProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,CategoryId")] CategoryProduct categoryProduct)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(categoryProduct);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ViewData["CategoryId"] = new SelectList(_context.Categories, nameof(Category.Id), nameof(Category.CategoryName), categoryProduct.CategoryId);
                ViewData["ProductId"] = new SelectList(_context.Products, nameof(Product.Id), nameof(Product.ProductName), categoryProduct.ProductId);
                return View("~/Views/Admin/CategoryProducts/Create.cshtml", categoryProduct);
            }
            catch(DbUpdateException)
            {
                var simpleContext = _context.CategoryProducts.Include(c => c.Category).Include(c => c.Product);
                return View("~/Views/Admin/CategoryProducts/index.cshtml", await simpleContext.ToListAsync());
            }

        }

        // GET: CategoryProducts/Edit/5
        public async Task<IActionResult> Edit(int? idC, int? idP)
        {
            if (idC == null || idP == null)
            {
                return NotFound();
            }

            var categoryProduct = await _context.CategoryProducts.FindAsync(idP,idC);
            if (categoryProduct == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, nameof(Category.Id), nameof(Category.CategoryName), categoryProduct.CategoryId);
            ViewData["ProductId"] = new SelectList(_context.Products, nameof(Product.Id), nameof(Product.ProductName), categoryProduct.ProductId);
            return View("~/Views/Admin/CategoryProducts/Edit.cshtml", categoryProduct);
        }

        // POST: CategoryProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("ProductId,CategoryId")] CategoryProduct categoryProduct)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categoryProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryProductExists(categoryProduct.ProductId, categoryProduct.CategoryId))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, nameof(Category.Id), nameof(Category.CategoryName), categoryProduct.CategoryId);
            ViewData["ProductId"] = new SelectList(_context.Products, nameof(Product.Id), nameof(Product.ProductName), categoryProduct.ProductId);
            return View("~/Views/Admin/CategoryProducts/Edit.cshtml", categoryProduct);
        }

        // GET: CategoryProducts/Delete/5
        public async Task<IActionResult> Delete(int? idC, int? idP)
        {
            if (idC == null || idP == null)
            {
                return NotFound();
            }

            var categoryProduct = await _context.CategoryProducts
                .Include(c => c.Category)
                .Include(c => c.Product)
                .FirstOrDefaultAsync(m => m.ProductId == idP && m.CategoryId == idC);
            if (categoryProduct == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, nameof(Category.Id), nameof(Category.CategoryName), categoryProduct.CategoryId);
            ViewData["ProductId"] = new SelectList(_context.Products, nameof(Product.Id), nameof(Product.ProductName), categoryProduct.ProductId);
            return View("~/Views/Admin/CategoryProducts/Delete.cshtml", categoryProduct);
        }

        // POST: CategoryProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed([Bind("ProductId,CategoryId")] CategoryProduct categoryProduct)
        {
            _context.CategoryProducts.Remove(categoryProduct);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryProductExists(int idC,int idP)
        {
            return _context.CategoryProducts.Any(e => e.ProductId == idP && e.CategoryId == idC);
        }
    }
}

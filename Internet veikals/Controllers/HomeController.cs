using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Internet_veikals.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using X.PagedList;

namespace Internet_veikals.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SimpleContext _context;

        public HomeController(ILogger<HomeController> logger, SimpleContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index(int? page)
        {

            return View(await HomeViewModel.CreateModel(page,4,_context));
        }


        public IActionResult AddToCart(int productId,string url = "Index")
        {
            if(HttpContext.Session.GetObject<List<Item>>("cart") == null || HttpContext.Session.GetObject<List<Item>>("cart").Count == 0)
            {
                List<Item> cart = new List<Item>();
                var product = _context.Products.Find(productId);
                cart.Add(new Item()
                {
                    Product = product,
                    Quantity = 1
                });
                HttpContext.Session.SetObject("cart", cart);
            }
            else
            {
                List<Item> cart = HttpContext.Session.GetObject<List<Item>>("cart");
                var product = _context.Products.Find(productId);
                var count = cart.Count();
                for (int i = 0; i < count; i++)
                {
                    if(cart[i].Product.Id == productId)
                    {
                        int prevQty = cart[i].Quantity;
                        cart.Remove(cart[i]);
                        cart.Add(new Item()
                        {
                            Product = product,
                            Quantity = prevQty+1
                        });
                        break;
                    }
                    else
                    {
                        var prd = cart.Where(x => x.Product.Id == productId).SingleOrDefault();
                        if(prd == null)
                        {
                            cart.Add(new Item()
                            {
                                Product = product,
                                Quantity = 1
                            });
                        }
                    }
                }

                HttpContext.Session.SetObject("cart", cart);
            }

            return Redirect(url);
        }
        public ActionResult DecreaseQty(int productId)
        {
            if (HttpContext.Session.GetObject<List<Item>>("cart") != null)
            {
                List<Item> cart = HttpContext.Session.GetObject<List<Item>>("cart");
                var product = _context.Products.Find(productId);
                foreach (var item in cart)
                {
                    if (item.Product.Id == productId)
                    {
                        int prevQty = item.Quantity;
                        if (prevQty > 0)
                        {
                            cart.Remove(item);
                            var added = new Item()
                            {
                                Product = product,
                                Quantity = prevQty - 1
                            };
                            if (added.Quantity != 0)
                                cart.Add(added);
                        }
                        break;
                    }
                }
                HttpContext.Session.SetObject("cart", cart);
            }
            return Redirect("Checkout");
        }
        public IActionResult RemoveFromCart(int productId)
        {
            List<Item> cart = HttpContext.Session.GetObject<List<Item>>("cart");
            foreach(var item in cart)
            {
                if(item.Product.Id==productId)
                {
                    cart.Remove(item);
                    break;
                }
            }

            HttpContext.Session.SetObject("cart", cart);

           return Redirect("Index");
        }
        public IActionResult Checkout()
        {
            return View();
        }
        public IActionResult CheckoutDetails()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CheckoutDetails([Bind("Id,Country,Address,City,PostCode,PhoneNumber,Email,UserId,ShippingType")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                List<Item> cart = HttpContext.Session.GetObject<List<Item>>("cart");
                foreach(Item p in cart)
                {
                    OrderProduct op = new OrderProduct() { OrderId=order.Id,ProductId=p.Product.Id,Qty=p.Quantity };
                    _context.Add(op);
                }
                await _context.SaveChangesAsync();
                HttpContext.Session.SetObject("cart", null);
                return RedirectToAction(nameof(Thanks));
            }
            return View();
        }
        public IActionResult Thanks()
        {
            return View();
        }
        public async Task<IActionResult> Browse(int? page)
        {
            if (HttpContext.Session.GetObject<List<Product>>("products") == null)
                return View(await HomeViewModel.CreateModel(page, 12, _context));
            else
                return View(await HomeViewModel.CreateModel(page, 12, _context, HttpContext.Session.GetObject<List<Product>>("products")));
        }
        public IActionResult SelectCategory(int categoryId)
        {
            List<CategoryProduct> categoryProducts = new List<CategoryProduct>();
            List<Product> products = new List<Product>();
            foreach (CategoryProduct cp in _context.CategoryProducts)
            {
                if(categoryId == cp.CategoryId)
                {
                    categoryProducts.Add(cp);
                }
            }
            foreach (CategoryProduct cp in categoryProducts)
            {
                foreach(Product p in _context.Products)
                    if (cp.ProductId == p.Id)
                        products.Add(p);
            }
            HttpContext.Session.SetObject("products", products);
            return Redirect("Browse");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

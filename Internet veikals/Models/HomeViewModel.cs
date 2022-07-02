using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using X.PagedList;


namespace Internet_veikals.Models
{
    public class HomeViewModel
    {
        public ICollection<Product> products;
        public ICollection<Category> categories;
        public ICollection<CategoryProduct> categoryProducts;
        public IPagedList<Product> productsPages;
        public ICollection<Stock> stocks;
        public IPagedList<Product> productsByCategoryPaged;
        public ICollection<Order> orders;
        public Order order;
        public static async Task<HomeViewModel> CreateModel(int? page, int pageSize, SimpleContext context)
        {
            HomeViewModel model = new HomeViewModel();
            model.products = await context.Products.ToListAsync();
            model.categoryProducts = await context.CategoryProducts.ToListAsync();
            model.categories = await context.Categories.ToListAsync();
            model.stocks = await context.Stock.ToListAsync();
            model.orders = await context.Orders.ToListAsync();
            model.productsPages = await model.products.ToPagedListAsync(page ?? 1, pageSize);
            model.order = new Order();
            return model;
        }
        public static async Task<HomeViewModel> CreateModel(int? page, int pageSize, SimpleContext context, List<Product> products)
        {
            HomeViewModel model = new HomeViewModel();
            model.products = await context.Products.ToListAsync();
            model.categoryProducts = await context.CategoryProducts.ToListAsync();
            model.categories = await context.Categories.ToListAsync();
            model.stocks = await context.Stock.ToListAsync();
            model.orders = await context.Orders.ToListAsync();
            model.productsPages = products.ToPagedList(page ?? 1, pageSize);
            model.order = new Order();
            return model;
        }

    }
}

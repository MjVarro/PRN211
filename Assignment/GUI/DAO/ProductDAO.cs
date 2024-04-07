using Assignment.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.DAO
{
    public class ProductDAO
    {
        private static ProductDAO instance;
        public static ProductDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ProductDAO();
                }
                return instance;
            }

        }
        private readonly NorthwindContext _context;

        ProductDAO()
        {
            _context = new NorthwindContext();
        }

        public List<Product> GetProducts()
        {
            var products = _context.Products
        .Include(p => p.Supplier)
        .Include(p => p.Category)
        .ToList();

            return products;
        }
        public Product GetProduct(int id)
        {
            var products = _context.Products
        .Include(p => p.Supplier)
        .Include(p => p.Category)
        .Where(o => o.ProductId == id)
        .FirstOrDefault();

            return products;
        }


    }
}

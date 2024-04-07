
using Assignment.DAO;
using Assignment.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Assignment.Controllers
{
    public class ProductController : Controller
    {
     
        private readonly CookieCartService _cartService;

        public ProductController(CookieCartService cartService)
        {
            _cartService = cartService;
        }

        // GET: ProductController
        public ActionResult Index(int? page, string sortBy, string searchQuery)
        {
            int pageSize = 4; // Number of products per page
            int pageNumber = page ?? 1; // Default page number is 1
            sortBy = sortBy ?? "name"; // Default sorting is by name
            searchQuery = searchQuery ?? ""; // Default search query is empty

            // Get the list of products
            var products = ProductDAO.Instance.GetProducts();

            // Apply search filter
           
            products = products.Where(p => p.ProductName.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)).ToList();
        

            // Apply sorting
            if (sortBy == "name")
            {
                products = products.OrderBy(p => p.ProductName).ToList();
            }
            else if (sortBy == "price")
            {
                products = products.OrderBy(p => p.UnitPrice).ToList();
            }

            var paginatedProducts = products.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            var cart = _cartService.GetCart();

            ListModel list = new ListModel()
            {
                Cart = cart,
                Products = paginatedProducts,
                AmountCart = cart.Sum(x => x.Quantity),
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(products.Count / (double)pageSize),
                SortBy = sortBy,
                SearchQuery = searchQuery
            };

            return View(list);
        }

        // GET: ProductController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddToCart(int productId)
        {
            var product = ProductDAO.Instance.GetProduct(productId);

            if (product != null)
            {
                var cart = _cartService.GetCart();
                var existingItem = cart.FirstOrDefault(item => item.ProductId == productId);

                if (existingItem != null)
                {
                    existingItem.Quantity++;
                }
                else
                {
                    cart.Add(new OrderDetail
                    {
                        ProductId = productId,
                        Quantity = 1,
                        UnitPrice = product.UnitPrice ?? 0 // Set the unit price here
                    });
                }

                _cartService.UpdateCart(cart);
            }

            return RedirectToAction("Index");
        }

        public IActionResult CartView()
        {
            var cart = _cartService.GetCart();
            return View(cart);
        }

        [HttpPost]
        public IActionResult CartView(Customer customer)
        {
            try {
                Order order = new Order();
                customer.CustomerId = GenerateRandomString(5);
                order.Customer = customer;
                order.CustomerId = customer.CustomerId;
                order.OrderDetails = _cartService.GetCart();
                OrderDAO.Instance.AddOrder(order);
                _cartService.ResetCart();
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }   

           
            return RedirectToAction("CartView");
        
        }

        static string GenerateRandomString(int length)
        {
            Random random = new Random();
            string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            char[] randomChars = new char[length];

            for (int i = 0; i < length; i++)
            {
                int index = random.Next(0, characters.Length);
                randomChars[i] = characters[index];
            }
            return new string(randomChars);
        }



    }
    public class ListModel
    {
        public List<Product> Products { get; set; }
        public List<OrderDetail> Cart { get; set; }
        public int AmountCart { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public string SortBy { get; set; }
        public string SearchQuery { get; set; }

    }


}

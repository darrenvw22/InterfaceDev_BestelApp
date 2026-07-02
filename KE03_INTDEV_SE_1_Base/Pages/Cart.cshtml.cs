using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using KE03_INTDEV_SE_1_Base.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;

namespace KE03_INTDEV_SE_1_Base.Pages
{
    public class CartModel : PageModel
    {
        private readonly IProductRepository _productRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IOrderRepository _orderRepository;

        public List<Product> Items { get; set; } = new();
        public List<Customer> Customers { get; set; } = new();

        [BindProperty]
        public int SelectedCustomerId { get; set; }

        public CartModel(IProductRepository productRepository, ICustomerRepository customerRepository, IOrderRepository orderRepository)
        {
            _productRepository = productRepository;
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;
        }

        public void OnGet()
        {
            LoadCart();
            Customers = _customerRepository.GetAllCustomers().ToList();
        }

        public IActionResult OnPostRemove(int productId)
        {
            var cart = HttpContext.Session.GetObject<List<int>>("Cart") ?? new List<int>();
            cart.Remove(productId);
            HttpContext.Session.SetObject("Cart", cart);
            return RedirectToPage();
        }

        public IActionResult OnPostPlaceOrder()
        {
            var cart = HttpContext.Session.GetObject<List<int>>("Cart") ?? new List<int>();
            if (!cart.Any() || SelectedCustomerId == 0)
            {
                TempData["Message"] = "Selecteer een klant en voeg minimaal één product toe.";
                return RedirectToPage();
            }

            var order = new Order { CustomerId = SelectedCustomerId, OrderDate = DateTime.Now };
            foreach (var id in cart)
            {
                var product = _productRepository.GetProductById(id);
                if (product != null)
                {
                    order.Products.Add(product);
                }
            }

            _orderRepository.AddOrder(order);
            HttpContext.Session.Remove("Cart");
            TempData["Message"] = "Order geplaatst.";
            return RedirectToPage("/OrderHistory");
        }

        private void LoadCart()
        {
            var cart = HttpContext.Session.GetObject<List<int>>("Cart") ?? new List<int>();
            Items = cart.Select(id => _productRepository.GetProductById(id)).Where(p => p != null).Select(p => p!).ToList();
        }
    }
}

using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using KE03_INTDEV_SE_1_Base.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;

namespace KE03_INTDEV_SE_1_Base.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;

        public IList<Customer> Customers { get; set; }
        public IList<Product> Products { get; set; }

        [BindProperty]
        public string? SearchTerm { get; set; }

        public IndexModel(ILogger<IndexModel> logger, ICustomerRepository customerRepository, IProductRepository productRepository)
        {
            _logger = logger;
            _customerRepository = customerRepository;
            _productRepository = productRepository;
            Customers = new List<Customer>();
            Products = new List<Product>();
        }

        public void OnGet()
        {
            Customers = _customerRepository.GetAllCustomers().ToList();                            
            Products = _productRepository.GetAllProducts().ToList();
            _logger.LogInformation($"getting all {Customers.Count} customers and {Products.Count} products");
        }

        public IActionResult OnPostAddToCart(int productId)
        {
            var cart = HttpContext.Session.GetObject<List<int>>("Cart") ?? new List<int>();
            cart.Add(productId);
            HttpContext.Session.SetObject("Cart", cart);
            TempData["Message"] = "Product toegevoegd aan winkelwagen.";
            return RedirectToPage();
        }

        public IActionResult OnPostSearch()
        {
            Customers = _customerRepository.GetAllCustomers().ToList();
            var all = _productRepository.GetAllProducts().ToList();
            if (string.IsNullOrWhiteSpace(SearchTerm))
            {
                Products = all;
            }
            else
            {
                Products = all.Where(p => p.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) || (p.Description != null && p.Description.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))).ToList();
            }
            return Page();
        }
    }
}

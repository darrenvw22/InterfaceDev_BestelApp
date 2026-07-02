using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KE03_INTDEV_SE_1_Base.Pages
{
    public class OrderHistoryModel : PageModel
    {
        private readonly IOrderRepository _orderRepository;

        public List<Order> Orders { get; set; } = new();

        public OrderHistoryModel(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public void OnGet()
        {
            Orders = _orderRepository.GetAllOrders().ToList();
        }
    }
}

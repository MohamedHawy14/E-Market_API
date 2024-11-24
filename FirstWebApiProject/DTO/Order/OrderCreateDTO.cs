using FirstWebApiProject.DTO.OrderItem;
using FirstWebApiProject.Models;
using System.ComponentModel.DataAnnotations;

namespace FirstWebApiProject.DTO.Order
{
    public class OrderCreateDTO
    {
        [Required(ErrorMessage = "Order Date is required")]
        public DateTime OrderDate { get; set; }

        [Required(ErrorMessage = "Total Amount is required")]
        [DataType(DataType.Currency)]
        public decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "Shipping Address is required")]
        [MaxLength(1000, ErrorMessage = "Address can't exceed 1000 characters")]
        [MinLength(10, ErrorMessage = "Address must be at least 10 characters")]
        public string ShippingAddress { get; set; }

        [Required(ErrorMessage = "Order Status is required")]
        public OrderStatus OrderStatus { get; set; }

        public List<OrderItemCreateDTO> OrderItems { get; set; } = new List<OrderItemCreateDTO>();
    }

}

using FirstWebApiProject.Models;
using System.ComponentModel.DataAnnotations;

namespace FirstWebApiProject.DTO.Order
{
    public class OrderWithOrderItemsDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Order Date is Required"), Display(Name = "Order Date")]
        public DateTime OrderDate { get; set; }

        [Required(ErrorMessage = "Total Amount is Required"), Display(Name = "Total Amount")]
        [DataType(DataType.Currency)]

        public decimal TotalAmount { get; set; }

        [Display(Name = "Shipping Address")]
        [MaxLength(1000, ErrorMessage = "Address Can't be a maximum of 1000 numbers")]
        [MinLength(10, ErrorMessage = "Address Can't be a minimum of 10 numbers")]

        public string? ShippingAddress { get; set; }

        [Display(Name = "Order Status")]
        public OrderStatus OrderStatus { get; set; }

        [Display(Name = "Order Items")]
        public List<string> OrderItemsList { get; set; }
    }
}

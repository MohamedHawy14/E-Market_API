using FirstWebApiProject.Models;
using System.ComponentModel.DataAnnotations;

namespace FirstWebApiProject.DTO.Payment
{
    public class PaymentCreateDTO
    {
        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Payment Date is required")]
        public DateTime PaymentDate { get; set; }

        [Required(ErrorMessage = "Payment Status is required")]
        public PaymentStatus PaymentStatus { get; set; }

        [Required(ErrorMessage = "Order ID is required")]
        public int OrderId { get; set; }
    }
}

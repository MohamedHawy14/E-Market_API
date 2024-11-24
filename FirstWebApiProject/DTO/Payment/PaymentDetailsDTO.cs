using FirstWebApiProject.Models;

namespace FirstWebApiProject.DTO.Payment
{
    public class PaymentDetailsDTO
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public int OrderId { get; set; }
    }
}

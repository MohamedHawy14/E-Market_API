using System.ComponentModel.DataAnnotations;

namespace FirstWebApiProject.DTO.OrderItem
{
    public class OrderItemCreateDTO
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
    }

}

using System.ComponentModel.DataAnnotations.Schema;

namespace FirstWebApiProject.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        #region Many2One-Order

        [ForeignKey("Order")]
        public int? OrderID { get; set; }
        public Order? Order { get; set; }
        #endregion
    }
}

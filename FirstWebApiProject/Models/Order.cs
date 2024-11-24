namespace FirstWebApiProject.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string? ShippingAddress { get; set; }
        public bool OrderStatus { get; set; } 

        #region One2Many-OrderItem
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();

        #endregion

        #region One2One-Payment
        public virtual Payment? Payment { get; set; } 
        #endregion
    }
}

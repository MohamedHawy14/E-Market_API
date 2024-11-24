namespace FirstWebApiProject.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }

        #region One2Many-Order
        public virtual ICollection<Order> Orders { get; set; } = new HashSet<Order>(); 
        #endregion

    }
}

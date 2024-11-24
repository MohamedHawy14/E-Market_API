namespace FirstWebApiProject.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public bool PaymentStatus { get; set; }

        #region One2One-Order
        public int? OrderId { get; set; } // Foreign Key
        public virtual Order? Order { get; set; } // Navigation property
        #endregion

    }
}

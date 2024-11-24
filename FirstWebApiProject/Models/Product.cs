using System.ComponentModel.DataAnnotations.Schema;

namespace FirstWebApiProject.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public string Description { get; set; } = null!;

        public int Quantity { get; set; }

        #region Many2One-Category
        [ForeignKey("Category")]
        public int? CategoryID { get; set; }
        public Category? Category { get; set; }
        #endregion

        #region One2Many-ProductReview
        public virtual ICollection<ProductReview> ProductReviews { get; set; } = new HashSet<ProductReview>(); 
        #endregion

    }
}

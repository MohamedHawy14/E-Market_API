using System.ComponentModel.DataAnnotations.Schema;

namespace FirstWebApiProject.Models
{
    public class ProductReview
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime ReviewDate { get; set; }
        #region Many2One-Product

        [ForeignKey("Product")]
        public int? ProductID { get; set; }
        public Product? Product { get; set; } 
        #endregion
    }
}

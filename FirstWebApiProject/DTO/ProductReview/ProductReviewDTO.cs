using System.ComponentModel.DataAnnotations;

namespace FirstWebApiProject.DTO.ProductReview
{
    public class ProductReviewDTO
    {
        public int Id { get; set; }
        [Range(1,5)]
        public int Rating { get; set; }
        public string? Comment { get; set; }

        [Display(Name = "Review Date")]
        public DateTime ReviewDate { get; set; }
    }
}

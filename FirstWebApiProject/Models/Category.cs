namespace FirstWebApiProject.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        #region One2Many- Product
        public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();

        #endregion    
    }
}

using FirstWebApiProject.Models;

namespace FirstWebApiProject.DTO.Category
{
    public class CategoryWithListOfProductsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> ProductsListDTO { get; set; }
    }

}

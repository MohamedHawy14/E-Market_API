using FirstWebApiProject.Interfaces;
using FirstWebApiProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FirstWebApiProject.Controllers
{
    [Route("Product/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        [HttpGet]
        public IActionResult GetAllProducts()
        {
            var products = unitOfWork.Repositry<Product>().GetALL();
            return Ok(products);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var product = unitOfWork.Repositry<Product>().GetById(id);
            return Ok(product);
        }
        [HttpGet("{name:alpha}")]
        public IActionResult GetByName(string name)
        {
            var product = unitOfWork.Repositry<Product>().GetByName(name);
            return Ok(product);
        }
        [HttpPost]
        public IActionResult AddProduct(Product product)
        {
            unitOfWork.Repositry<Product>().Add(product);
            unitOfWork.Complete();
            return CreatedAtAction("GetById", new {Id=product.Id }, product);
        }
        [HttpPut("{id:int}")]
        public IActionResult UpdateProduct(Product product , int id)
        {
            var products = unitOfWork.Repositry<Product>().GetById(id);
            if (products != null)
            {
                products.Name = product.Name;
                products.Price = product.Price;
                products.Description = product.Description;
                products.Quantity = product.Quantity;
                products.CategoryID = product.CategoryID;
                unitOfWork.Complete();
                return NoContent();
            }
            else
            {
                return  NotFound("Product Not Found");
            }
        }
        [HttpDelete("{id:int}")]
        public IActionResult DeleteProduct(int id)
        {
            var department = unitOfWork.Repositry<Product>().GetById(id);
            unitOfWork.Repositry<Product>().Delete(department);
            unitOfWork.Complete();
            return NoContent();
        }
    }
}

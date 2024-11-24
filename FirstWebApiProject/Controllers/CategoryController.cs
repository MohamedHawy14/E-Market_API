using FirstWebApiProject.DTO;
using FirstWebApiProject.DTO.Cat;
using FirstWebApiProject.DTO.Category;
using FirstWebApiProject.Interfaces;
using FirstWebApiProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FirstWebApiProject.Controllers
{
    [Route("Category/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        [HttpGet]
        public IActionResult GetAllCategories()
        {
            var categories = unitOfWork.Repositry<Category>().GetALL();
            var categoriesDTO = categories.Select(c => new CategoryDTO { Id = c.Id, Name = c.Name });
            return Ok(categoriesDTO);
        }
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var category = unitOfWork.Repositry<Category>()
                           .GetFirstOrDefault(
                               c => c.Id == id,          // Filter: Get category with specific ID
                               includeProperties: "Products" // Include related products
                           );

            if (category == null)
            {
                return NotFound(new { Message = "Category not found." });
            }

            var result = new CategoryWithListOfProductsDTO
            {
                Id = category.Id,
                Name = category.Name,
                ProductsListDTO = category.Products.Select(p => p.Name).ToList()
            };

            return Ok(result);



        }
        [HttpGet("{name:alpha}")]
        public IActionResult GetByName(string name)
        {
            var Category = unitOfWork.Repositry<Category>().GetByName(name);
            if (Category == null)
            {
                return NotFound(new { Message = "Category not found." });
            }
            var Result = new CategoryDTO { Id = Category.Id, Name = Category.Name };
            return Ok(Result);
        }
        [HttpGet("ProductsNumber")]
        public IActionResult GetCategoryWithSumProducts()
        {
            var categories = unitOfWork.Repositry<Category>().Include(c => c.Products).ToList();
            var categoriesDTo = categories.Select(c => new GetCategoryWithSumProductsDTO
            {
                Id = c.Id,
                Name = c.Name,
                ProductNumbers = c.Products.Count()
            });
            return Ok(categoriesDTo);
        }
        [HttpPost]
        public IActionResult CreateNewCategory(CategoryDTO categoryDto)
        {
            if (categoryDto == null)
            {
                return BadRequest("Category data is required.");
            }

            // Map the DTO to the Category entity
            var category = new Category
            {
                Id=categoryDto.Id,
                Name = categoryDto.Name
            };

            try
            {
                // Add the new category using the repository
                unitOfWork.Repositry<Category>().Add(category);

                // Save changes to the database
                unitOfWork.Complete();

                // Return a success response with the created category's data
                return CreatedAtAction(nameof(GetById), new { id = category.Id }, categoryDto);
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut]
        public IActionResult EditCategory(int id, CategoryDTO categoryDTO)
        {
            // Step 1: Validate input
            if (categoryDTO == null)
            {
                return BadRequest("Category data is required.");
            }

            // Step 2: Retrieve the existing category from the database
            var category = unitOfWork.Repositry<Category>().GetById(id);

            if (category == null)
            {
                return NotFound($"Category with ID {id} not found.");
            }

            // Step 3: Update category properties
            category.Name = categoryDTO.Name;

            try
            {
                // Step 4: Save changes to the database
                unitOfWork.Complete();

                // Step 5: Return the updated category
                var updatedCategoryDTO = new CategoryDTO
                {
                    Id = category.Id,
                    Name = category.Name
                };

                // Returning the updated category with HTTP 200 OK
                return Ok(updatedCategoryDTO);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the update
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete]
        public IActionResult DeleteCategory(int id)
        {
            // Step 1: Retrieve the existing category from the database
            var category = unitOfWork.Repositry<Category>().GetById(id);

            if (category == null)
            {
                // If the category doesn't exist, return NotFound (404)
                return NotFound($"Category with ID {id} not found.");
            }

            try
            {
                // Step 2: Create a CategoryDTO to return before deletion
                var categoryDto = new CategoryDTO
                {
                    Id = category.Id,
                    Name = category.Name
                };

                // Step 3: Delete the category from the repository
                unitOfWork.Repositry<Category>().Delete(category);

                // Step 4: Save changes to the database
                unitOfWork.Complete();

                // Step 5: Return a successful response (204 No Content) along with the deleted CategoryDTO
                return Ok(categoryDto); // 200 OK and return the deleted CategoryDTO
            }
            catch (Exception ex)
            {
                // Handle any exceptions that might occur during the delete operation
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



    }
}

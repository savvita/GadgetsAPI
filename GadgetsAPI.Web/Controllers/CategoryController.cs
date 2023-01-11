using DBAccess.Repositories;
using GadgetsAPI.Web.Exceptions;
using GadgetsAPI.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace GadgetsAPI.Web.Controllers
{
    [ApiController]
    [Route("categories")]
    public class CategoryController : ControllerBase
    {
        private ICategoryRepository repository;
        public CategoryController(ICategoryRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet("")]
        public async Task<List<Category>> GetAllCategories()
        {
            var models = await repository.GetAllCategoriesAsync();

            var categories = new List<Category>();

            foreach (var model in models)
            {
                categories.Add(new Category()
                {
                    Id = model.Id,
                    CategoryName = model.CategoryName
                });
            }

            return categories;
        }


        [HttpGet("{id:int}")]
        public async Task<Category> GetCategoryById(int id)
        {
            var model = await repository.GetCategoryAsync(id);

            if (model == null)
            {
                throw new CategoryNotFoundException(id);
            }

            return new Category()
            {
                Id = model.Id,
                CategoryName = model.CategoryName
            };
        }

        [HttpPost()]
        public async Task<Category> AddCategory(string name)
        {
            if(string.IsNullOrWhiteSpace(name))
            {
                throw new BadRequestException();
            }

            var model = await repository.AddCategoryAsync(new DBAccess.Models.CategoryModel()
            {
                CategoryName = name
            });

            return new Category()
            {
                Id = model.Id,
                CategoryName = model.CategoryName
            };
        }

        [HttpPut]
        public async Task<Category> UpdateCategory(Category category)
        {
            if (string.IsNullOrWhiteSpace(category.CategoryName))
            {
                throw new BadRequestException();
            }

            var model = await repository.UpdateCategoryAsync(new DBAccess.Models.CategoryModel()
            {
                Id = category.Id,
                CategoryName = category.CategoryName
            });

            return new Category()
            {
                Id = model.Id,
                CategoryName = model.CategoryName
            };
        }

        [HttpDelete("{id:int}")]
        public async Task<bool> RemoveCategoryById(int id)
        {
            return await repository.RemoveCategoryAsync(id);
        }
    }
}

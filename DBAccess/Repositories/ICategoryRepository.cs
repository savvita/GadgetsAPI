using DBAccess.Models;

namespace DBAccess.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<CategoryModel>> GetAllCategoriesAsync();
        Task<CategoryModel?> GetCategoryAsync(int id);
        Task<CategoryModel?> GetCategoryAsync(string name);
        Task<CategoryModel> AddCategoryAsync(CategoryModel category);
        Task<CategoryModel> UpdateCategoryAsync(CategoryModel category);
        Task<bool> RemoveCategoryAsync(int id);
    }
}

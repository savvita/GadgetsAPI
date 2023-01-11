using DBAccess.Models;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace DBAccess.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private DBConfig configuration;

        public CategoryRepository(DBConfig configuration)
        {
            this.configuration = configuration;
        }

        public async Task<CategoryModel> AddCategoryAsync(CategoryModel category)
        {
            using IDbConnection connection = new SqlConnection(configuration.ConnectionString);

            int id = await connection.QueryFirstAsync<int>("insert into Categories values (@CategoryName); select SCOPE_IDENTITY();", category);

            category.Id = id;

            return category;
        }

        public async Task<IEnumerable<CategoryModel>> GetAllCategoriesAsync()
        {
            using IDbConnection connection = new SqlConnection(configuration.ConnectionString);

            return await connection.QueryAsync<CategoryModel>("select * from Categories");
        }

        public async Task<CategoryModel?> GetCategoryAsync(int id)
        {
            using IDbConnection connection = new SqlConnection(configuration.ConnectionString);

            return await connection.QueryFirstOrDefaultAsync<CategoryModel>("select * from Categories where Id = @Id", new { Id = id });
        }

        public async Task<CategoryModel?> GetCategoryAsync(string name)
        {
            using IDbConnection connection = new SqlConnection(configuration.ConnectionString);

            return await connection.QueryFirstOrDefaultAsync<CategoryModel>("select * from Categories where CategoryName = @CategoryName", new { CategoryName = name });
        }

        public async Task<bool> RemoveCategoryAsync(int id)
        {
            using IDbConnection connection = new SqlConnection(configuration.ConnectionString);

            await connection.ExecuteAsync("update Gadgets set CategoryId = null where CategoryId = @Id", new { Id = id });

            return (await connection.ExecuteAsync("delete Categories where Id = @Id", new { Id = id })) != 0;
        }

        public async Task<CategoryModel> UpdateCategoryAsync(CategoryModel category)
        {
            using IDbConnection connection = new SqlConnection(configuration.ConnectionString);
            await connection.ExecuteAsync("update Categories set CategoryName = @CategoryName where Id = @Id", category);

            return category;
        }
    }
}

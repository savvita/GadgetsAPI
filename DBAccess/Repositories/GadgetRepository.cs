using Dapper;
using DBAccess.Models;
using System.Data;
using System.Data.SqlClient;

namespace DBAccess.Repositories
{
    public class GadgetRepository : IGadgetRepository
    {
        private DBConfig configuration;

        public GadgetRepository(DBConfig configuration)
        {
            this.configuration = configuration;
        }

        public async Task<GadgetModel> AddGadgetAsync(GadgetModel gadget)
        {
            using IDbConnection connection = new SqlConnection(configuration.ConnectionString);

            int id = await connection.QueryFirstAsync<int>("insert into Gadgets values (@CategoryId, @ProducerId, @Model, @Price); select SCOPE_IDENTITY();", gadget);

            gadget.Id = id;

            return gadget;
        }

        public async Task<IEnumerable<GadgetModel>> GetAllGadgetsAsync()
        {
            return await GetAllGadgetsAsync(null, null, null, null);
        }

        public async Task<IEnumerable<GadgetModel>> GetAllGadgetsAsync(List<int>? categoryIds, List<int>? producerIds, decimal? minPrice, decimal? maxPrice)
        {
            string sql = @"select * from Gadgets left join Categories on Gadgets.CategoryId = Categories.Id
                        left join Producers on Gadgets.ProducerId = Producers.Id";

            DynamicParameters parameters = new DynamicParameters();
            List<string> filters = new List<string>();

            if(categoryIds != null)
            {
                filters.Add("CategoryId in @CategoryIds");
                parameters.Add("@CategoryIds", categoryIds);
            }

            if (producerIds != null)
            {
                filters.Add("ProducerId in @ProducerIds");
                parameters.Add("@ProducerIds", producerIds);
            }

            if (minPrice != null)
            {
                filters.Add("Price >= @MinPrice");
                parameters.Add("@MinPrice", minPrice);
            }

            if (maxPrice != null)
            {
                filters.Add("Price <= @MaxPrice");
                parameters.Add("@MaxPrice", maxPrice);
            }

            if(filters.Count > 0)
            {
                sql += $" where {string.Join(" and ", filters)}";
            }

            using IDbConnection connection = new SqlConnection(configuration.ConnectionString);

            return await connection.QueryAsync<GadgetModel, CategoryModel, ProducerModel, GadgetModel>(
                sql,
                (g, c, p) =>
                {
                    GadgetModel gadget = new GadgetModel()
                    {
                        Id = g.Id,
                        Model = g.Model,
                        Price = g.Price,
                        CategoryId = g.CategoryId,
                        ProducerId = g.ProducerId
                    };

                    if (c != null && c.CategoryName != null)
                    {
                        gadget.Category = new CategoryModel()
                        {
                            Id = c.Id,
                            CategoryName = c.CategoryName
                        };
                    }

                    if (p != null && p.ProducerName != null)
                    {
                        gadget.Producer = new ProducerModel()
                        {
                            Id = p.Id,
                            ProducerName = p.ProducerName
                        };
                    }

                    return gadget;
                }, parameters);
        }


        public async Task<GadgetModel?> GetGadgetAsync(int id)
        {
            using IDbConnection connection = new SqlConnection(configuration.ConnectionString);

            var gadget = await connection.QueryFirstOrDefaultAsync<GadgetModel>("select * from Gadgets where Id = @Id", new { Id = id });

            if(gadget != null)
            {
                if (gadget.CategoryId != null)
                {
                    gadget.Category = await connection.QueryFirstOrDefaultAsync<CategoryModel>("select * from Categories where Id = @Id", 
                        new { Id = gadget.CategoryId });
                }

                if (gadget.ProducerId != null)
                {
                    gadget.Producer = await connection.QueryFirstOrDefaultAsync<ProducerModel>("select * from Producers where Id = @Id", 
                        new { Id = gadget.ProducerId });
                }
            }

            return gadget;
        }

        public async Task<bool> RemoveGadgetAsync(int id)
        {
            using IDbConnection connection = new SqlConnection(configuration.ConnectionString);

            return (await connection.ExecuteAsync("delete Gadgets where Id = @Id", new { Id = id })) != 0;
        }

        public async Task<GadgetModel> UpdateGadgetAsync(GadgetModel gadget)
        {
            using IDbConnection connection = new SqlConnection(configuration.ConnectionString);
            await connection.ExecuteAsync("update Gadgets set CategoryId = @CategoryId, ProducerId = @ProducerId, Model = @Model, Price = @Price where Id = @Id",
                gadget);

            return gadget;
        }
    }
}

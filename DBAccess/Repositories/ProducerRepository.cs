using Dapper;
using DBAccess.Models;
using System.Data;
using System.Data.SqlClient;

namespace DBAccess.Repositories
{
    public class ProducerRepository : IProducerRepository
    {
        private DBConfig configuration;

        public ProducerRepository(DBConfig configuration)
        {
            this.configuration = configuration;
        }

        public async Task<ProducerModel> AddProducerAsync(ProducerModel producer)
        {
            using IDbConnection connection = new SqlConnection(configuration.ConnectionString);

            int id = await connection.QueryFirstAsync<int>("insert into Producers values (@ProducerName); select SCOPE_IDENTITY();", producer);

            producer.Id = id;

            return producer;
        }

        public async Task<IEnumerable<ProducerModel>> GetAllProducersAsync()
        {
            using IDbConnection connection = new SqlConnection(configuration.ConnectionString);

            return await connection.QueryAsync<ProducerModel>("select * from Producers");
        }

        public async Task<ProducerModel?> GetProducerAsync(int id)
        {
            using IDbConnection connection = new SqlConnection(configuration.ConnectionString);

            return await connection.QueryFirstOrDefaultAsync<ProducerModel>("select * from Producers where Id = @Id", new { Id = id });
        }

        public async Task<ProducerModel?> GetProducerAsync(string name)
        {
            using IDbConnection connection = new SqlConnection(configuration.ConnectionString);

            return await connection.QueryFirstOrDefaultAsync<ProducerModel>("select * from Producers where ProducerName = @ProducerName", new { ProducerName = name });
        }

        public async Task<bool> RemoveProducerAsync(int id)
        {
            using IDbConnection connection = new SqlConnection(configuration.ConnectionString);

            await connection.ExecuteAsync("update Gadgets set ProducerId = null where ProducerId = @Id", new { Id = id });

            return (await connection.ExecuteAsync("delete Producers where Id = @Id", new { Id = id })) != 0;
        }

        public async Task<ProducerModel> UpdateProducerAsync(ProducerModel producer)
        {
            using IDbConnection connection = new SqlConnection(configuration.ConnectionString);
            await connection.ExecuteAsync("update Producers set ProducerName = @ProducerName where Id = @Id", producer);

            return producer;
        }
    }
}

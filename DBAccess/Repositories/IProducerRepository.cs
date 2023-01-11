using DBAccess.Models;

namespace DBAccess.Repositories
{
    public interface IProducerRepository
    {
        Task<IEnumerable<ProducerModel>> GetAllProducersAsync();
        Task<ProducerModel?> GetProducerAsync(int id);
        Task<ProducerModel?> GetProducerAsync(string name);
        Task<ProducerModel> AddProducerAsync(ProducerModel producer);
        Task<ProducerModel> UpdateProducerAsync(ProducerModel producer);
        Task<bool> RemoveProducerAsync(int id);
    }
}

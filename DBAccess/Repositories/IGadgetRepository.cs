using DBAccess.Models;

namespace DBAccess.Repositories
{
    public interface IGadgetRepository
    {
        Task<IEnumerable<GadgetModel>> GetAllGadgetsAsync();
        Task<IEnumerable<GadgetModel>> GetAllGadgetsAsync(List<int>? categoryId, List<int>? producerId, decimal? minPrice, decimal? maxPrice);
        Task<GadgetModel?> GetGadgetAsync(int id);
        Task<GadgetModel> AddGadgetAsync(GadgetModel gadget);
        Task<GadgetModel> UpdateGadgetAsync(GadgetModel gadget);
        Task<bool> RemoveGadgetAsync(int id);
    }
}

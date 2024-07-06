using MediacApi.Data.Entities;
using MediacApi.DTOs.Subscribes;

namespace MediacApi.Services.IRepositories
{
    public interface ISubscribeRepo
    {
        public Task<IEnumerable<SubscribeDetailsDto>> GetAllSubscribers(Guid blogId);

        public Task AddSubscriber(Guid blogId, string userId);

        public Task RemoveSubscriber(Guid blogId, string userId);

        
    }
}

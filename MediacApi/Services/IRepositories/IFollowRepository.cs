using MediacApi.DTOs.Followers;

namespace MediacApi.Services.IRepositories
{
    public interface IFollowRepository
    {
        public Task<bool> FollowAsync(string authorEmail);

        public Task UnFollowAsync(string authorEmail);

        public Task<IEnumerable<getFollowerDto>> getLogedInFollowersAsync();
    }
}

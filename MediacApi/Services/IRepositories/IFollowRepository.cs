using MediacApi.DTOs.Followers;

namespace MediacApi.Services.IRepositories
{
    public interface IFollowRepository
    {
        public Task<bool> FollowAsync(string authorEmail);

        public Task FollowWithUser(string userName, string authorEmail);

        public Task UnFollowAsync(string authorEmail);

        public Task<IEnumerable<getFollowerDto>> getLogedInFollowersAsync();

        public Task<IEnumerable<getFollowerDto>> getFollowerAsync(string Id);
    }
}

using MediacApi.DTOs.Account;

namespace MediacApi.Services.IRepositories
{
    public interface iUserRepository
    {
        public Task<IEnumerable<getUsersDto>> getAllUsers();
    }
}

using MediacApi.Data.Entities;


namespace MediacBack.Services.IRepositories
{
    public interface IBlogRepository
    {
        public Task AddBlog(Blog blog);

        public Task<IEnumerable<Blog>> GetAll();

        public Task followBlog(Guid id);

        public Task UnfollowBlog(Guid id);

        public Task<bool> DeleteBlog(Guid id);
    }
}

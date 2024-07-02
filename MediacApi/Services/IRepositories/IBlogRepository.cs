using MediacApi.Data.Entities;
using MediacApi.DTOs.Blogs;


namespace MediacBack.Services.IRepositories
{
    public interface IBlogRepository
    {
        public Task AddBlog(Blog blog);

        public Task<IEnumerable<getBlogDto>> GetAll();

        public Task<Blog> getBlog(Guid id);

        public Task followBlog(Guid id);

        public Task UnfollowBlog(Guid id);

        public Task<bool> DeleteBlog(Guid id);
    }
}

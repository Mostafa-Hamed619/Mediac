using MediacApi.Data.Entities;

namespace MediacBack.Services.IRepositories
{
    public interface IPostRepository
    {
        public Task<IEnumerable<Post>> getAllPosts();

        public Task<Post> getPostAsync(Guid id);

        public Task AddPostAsync(Post post,Guid BlogId);

        public Task DeletePostAsync(Guid id);

        public Task UpdatePostAsync(Post post);

        public Task<Guid> GetBlogId(string blogName);

        public Task<int> getPostCount();

        public Task<IEnumerable<Post>> Paginationposts(int page);
    }
}

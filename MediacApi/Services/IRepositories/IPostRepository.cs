﻿using MediacApi.Data.Entities;
using MediacApi.DTOs.Posts;
using MediacBack.DTOs.Posts;

namespace MediacBack.Services.IRepositories
{
    public interface IPostRepository
    {
        public Task<IEnumerable<Post>> getAllPosts();

        public Task<Post> getPostAsync(Guid id);

        public Task AddPostAsync(Post post);

        public Task DeletePostAsync(Guid id);

        public Task UpdatePostAsync(Post post);

        public Task<Guid> GetBlogId(string blogName);

        public Task<int> getPostCount();

        public Task<IEnumerable<getPostPagingDto>> Paginationposts(int page);

        public Task<Guid> Getid(string Name);

        public Task<IEnumerable<PostBlogSearch>> postBlogSearches(string postBlog);

    }
}

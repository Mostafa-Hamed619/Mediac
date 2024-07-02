using MediacApi.Services.IRepositories;

namespace MediacApi.Services.MockRepositories
{
    public class MockIhttpAccessor : ihttpAccessor
    {
        private readonly IHttpContextAccessor context;

        public MockIhttpAccessor(IHttpContextAccessor context)
        {
            this.context = context;
        }
        public IHttpContextAccessor GetContext()
        {
            return context;
        }
    }
}

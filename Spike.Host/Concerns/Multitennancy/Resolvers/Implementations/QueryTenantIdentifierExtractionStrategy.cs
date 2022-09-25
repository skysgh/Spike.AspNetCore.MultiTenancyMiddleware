using Microsoft.Extensions.Primitives;

namespace Spike.Host.Concerns.Multitennancy.Resolvers.Implementations
{
    public class QueryTenantIdentifierExtractionStrategy : ITenantIdentifierExtractionStrategy
    {
        private IHttpContextAccessor _httpContextAccessor;

        public QueryTenantIdentifierExtractionStrategy(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<string> GetTenantIdentifierAsync()
        {
            var context = _httpContextAccessor.HttpContext;

            if (context == null)
            {
                return null;
            }
            StringValues tmp;
            if (!context.Request.Query.TryGetValue("tenant", out tmp)){
                return null;
            }
            return tmp.ToString();
        }
    }
}

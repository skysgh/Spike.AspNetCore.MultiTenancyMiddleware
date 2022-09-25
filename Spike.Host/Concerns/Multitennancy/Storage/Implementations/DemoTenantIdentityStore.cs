using Spike.Host.Concerns.Multitennancy.Messages;

namespace Spike.Host.Concerns.Multitennancy.Implementations.Storage.Implementations
{
    public class DemoTenantIdentityStore
        : ITenantIdentityTenantStore<TenantIdentity>
    {

        public bool Enabled { get; set; } = true;

        public string Name { get; } = "Demo";

        /// <summary>
        /// Get a tenant for a given identifier
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public async Task<TenantIdentity> GetTenantAsync(string identifier)
        {
            var tenants = new[]
                {
                new TenantIdentity{
                    Id = new Guid(
                        "70fdb4c0-5888-4295-bf40-ebee0e3cd8f3"),
                    Identifiers = {"foo" }
                    },
                new TenantIdentity{
                    Id = new Guid(
                        "80fdb3c0-5888-4295-bf40-ebee0e3cd8f3"),
                    Identifiers = {null,"Default", "localhost" }
                    },
                new TenantIdentity{
                    Id = new Guid(
                        "30fdb7c0-5888-4295-bf40-ebee0e3cd8f3"),
                    Identifiers = {"bar" }
                    },
                };
            
            var tenant = tenants.SingleOrDefault(t => t.Identifiers.Contains(identifier));

            return await Task.FromResult(tenant);
        }
    }
}

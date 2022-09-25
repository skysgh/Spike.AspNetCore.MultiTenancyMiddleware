using Spike.Host.Concerns.Multitennancy.Implementations.Storage;
using Spike.Host.Concerns.Multitennancy.Messages;
using Spike.Host.Concerns.Multitennancy.Resolvers;

namespace Spike.Host.Concerns.Multitennancy.Services.Implementations
{
    //public class TenantIdentityService :
    //    TenantIdentityService<TenantIdentity>
    //{
    //    public TenantIdentityService(
    //        )
    //        :
    //        base(tenantStore)
    //    { }
    //}


    /// <summary>
    /// Service to retrieve information
    /// about the identity of a Tenant
    /// (Note that it doesn't return 
    /// a system entity -- just information 
    /// as to its Identity).
    /// </summary>
    /// <typeparam name="TTenantIdentity"></typeparam>
    public class TenantIdentityService<TTenantIdentity> :
        ITenantIdentityService<TTenantIdentity>
        where TTenantIdentity : TenantIdentity
    {
        private readonly ITenantIdentifierExtractionStrategy[] _tenantResolutionStrategies;
        private readonly ITenantIdentityTenantStore<TTenantIdentity>[] _tenantStores;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tenantResolutionStrategy"></param>
        /// <param name="tenantStore"></param>
        public TenantIdentityService(
            IServiceProvider serviceProvider
            )
        {
            _tenantResolutionStrategies
                = serviceProvider
                .GetServices<ITenantIdentifierExtractionStrategy>()
                .ToArray();

            _tenantStores =
                serviceProvider
                .GetServices<ITenantIdentityTenantStore<TTenantIdentity>>()
                .ToArray();
                
        }


        public async Task<TTenantIdentity> GetCurrentTenantIdentityAsync()
        {
            // Get the Tenant identifier

            string? tenantIdentifier = null;
            foreach (var strategy in _tenantResolutionStrategies)
            {
                tenantIdentifier = await strategy.GetTenantIdentifierAsync();
                if (tenantIdentifier != null)
                {
                    break;
                }
            }
            // Note that it may be null,
            // which is fine, as long as one entry has a Null
            // for the entry (or "Default")
            if (tenantIdentifier == null)
            {
                tenantIdentifier = "Default";
            }

            // Pass to the Store/Db/whatever
            // to retrieve the id
            foreach(var store in _tenantStores)
            {
                var r = await store.GetTenantAsync(tenantIdentifier);
                    if (r != null)
                {
                    return r;
                }
            }
            return null;

        }
    }
}


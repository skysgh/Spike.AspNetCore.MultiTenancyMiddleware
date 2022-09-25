using Spike.Host.Concerns.Multitennancy.Messages;

namespace Spike.Host.Concerns.Multitennancy.Implementations.Storage.Implementations
{
    public class UnFinishedTenantInfoStore :
        ITenantIdentityTenantStore<TenantIdentity>
    {
        public bool Enabled { get; set; } = true;
        public string Name { get; } = "UnFinished";


        public Task<TenantIdentity> GetTenantAsync(string identifier)
        {
            throw new NotImplementedException();
        }
    }
}

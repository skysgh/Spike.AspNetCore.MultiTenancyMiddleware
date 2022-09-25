using Spike.Host.Concerns.Multitennancy.Messages;

namespace Spike.Host.Concerns.Multitennancy.Services
{
    /// <summary>
    /// Generic Contract for service to retrieve
    /// information as to the (cacheable)
    /// *identity* of a tenancy.
    /// <para>
    /// the key information is the datastore 
    /// Identity of the Tenancy so that it can be retrieved
    /// if/as required.
    /// </para>
    /// </summary>
    /// <typeparam name="TTenantIdentity"></typeparam>
    public interface ITenantIdentityService<TTenantIdentity> 
        where TTenantIdentity : TenantIdentity
    {
        /// <summary>
        /// Get information as to the 
        /// current tenant.
        /// <para>
        /// Note that it does NOT return the System Entity
        /// for the Tenancy.
        /// </para>
        /// </summary>
        /// <returns></returns>
        Task<TTenantIdentity> GetCurrentTenantIdentityAsync();
    }
}
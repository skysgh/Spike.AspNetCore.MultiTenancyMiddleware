using Spike.Host.Concerns.Multitennancy.Messages;

namespace Spike.Host.Concerns.Multitennancy.Implementations.Storage
{
    /// <summary>
    /// Contract for a service
    /// to return a 
    /// <see cref="TenantIdentity"/>
    /// from storage somewhere.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITenantIdentityTenantStore<T> where T : TenantIdentity
    {
        string Name { get; }
        /// <summary>
        /// Whether Storage is Enabled or not.
        /// </summary>
        bool Enabled { get; set; }

        Task<T> GetTenantAsync(string identifier);
    }
}

using Spike.Host.Concerns.Multitennancy.Implementations.Storage.Implementations;
using Spike.Host.Concerns.Multitennancy.Implementations.Storage;
using Spike.Host.Concerns.Multitennancy.Messages;

namespace Spike.Host.Concerns.Multitennancy.Builder.ExtensionMethods
{
    /// <summary>
    /// Extension Methods to be invoked
    /// at startup to 
    /// add Multitenancy Middleware 
    /// after the app has been built.
    /// </summary>
    public static class MultiTenancyServiceCollectionExtensions
    {
        /// <summary>
        /// Add the services (default tenant class)
        /// <para>
        /// Usage:
        /// <code>
        /// <![CDATA[
        /// builder.Services.AddMultiTenancy()
        ///                 .WithResolutionStrategy
        ///                     <HostBasedMultiTenancyTenantIdentifierResolutionStrategy>()()
        ///                 .WithResolutionStrategy
        ///                     <OtherMultiTenancyTenantIdentifierResolutionStrategy>()
        ///                 .WithStore<InMemoryMultiTenancyTenantStore>()
        ///                 .WithStore<DbContextMultiTenancyTenantStore>()
        ///                 ;
        /// ]]>
        /// </code>
        /// </para>
        /// <para>
        /// Internally, the extension method is 
        /// creating a <see cref="MultiTenancyBuilder{T}"/>
        /// which is in turn registering within 
        /// <see cref="IServiceCollection"/>
        /// the given implementations of 
        /// <see cref="IMultiTenancyTenantIdentifierResolutionStrategy"/>
        /// </para>
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static MultiTenancyBuilder<TenantIdentity> AddTenantIdentityDetermination(
            this IServiceCollection services)
        {
            return new MultiTenancyBuilder<TenantIdentity>(services);
        }

        /// <summary>
        /// Add the services (application specific tenant class)
        /// <para>
        /// Usage:
        /// <code>
        /// <![CDATA[
        /// builder.Services.AddMultiTenancy()
        ///                 .WithResolutionStrategy
        ///                     <HostBasedMultiTenancyTenantIdentifierResolutionStrategy>()()
        ///                 .WithResolutionStrategy
        ///                     <OtherMultiTenancyTenantIdentifierResolutionStrategy>()
        ///                 .WithStore()
        ///                 ;
        /// ]]>
        /// </code>
        /// </para>
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static MultiTenancyBuilder<T> AddMultiTenancy<T>(
            this IServiceCollection services)
            where T : TenantIdentity
        {
            return new MultiTenancyBuilder<T>(services);
        }

        

        public static IServiceProvider
            ChangeStateOfTenantInfoStore<TTenantIdentityTenantStore, TTenantIdentity> 
            (
            this IServiceProvider services, bool state)
            where TTenantIdentityTenantStore : ITenantIdentityTenantStore<TTenantIdentity>
            where TTenantIdentity : TenantIdentity
        {
                // Turn off things 
                // that need integration
                // info until we are sure 
                // those dependencies are met:
                var x =
                    services
                    .GetServices<ITenantIdentityTenantStore<TenantIdentity>>()
                    .SingleOrDefault(x => x.GetType() == typeof(UnFinishedTenantInfoStore))
                    !.Enabled = false;

            return services;
        }
    }
}

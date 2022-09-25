using Microsoft.Extensions.DependencyInjection.Extensions;
using Spike.Host.Concerns.Multitennancy.Builder.ExtensionMethods;
using Spike.Host.Concerns.Multitennancy.Implementations.Storage;
using Spike.Host.Concerns.Multitennancy.Messages;
using Spike.Host.Concerns.Multitennancy.Resolvers;
using Spike.Host.Concerns.Multitennancy.Services;

namespace Spike.Host.Concerns.Multitennancy.Builder
{
    /// <summary>
    /// Class developed by 
    /// <see cref="MultiTenancyServiceCollectionExtensions.AddTenantIdentityDetermination"/>
    /// to apply changes to 
    /// <see cref="IServiceCollection"/>
    /// before App is built from it
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MultiTenancyBuilder<T> where T : TenantIdentity
    {
        private readonly IServiceCollection _services;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="services"></param>
        public MultiTenancyBuilder(IServiceCollection services)
        {
            _services = services;
        }

        /// <summary>
        /// Register the tenant resolver implementation
        /// </summary>
        /// <typeparam name="TStrategy"></typeparam>
        /// <param name="lifetime"></param>
        /// <returns></returns>
        public MultiTenancyBuilder<T> WithResolutionStrategy<TStrategy>(
            ServiceLifetime lifetime = ServiceLifetime.Singleton)
            where TStrategy : class, ITenantIdentifierExtractionStrategy
        {
            //It may have already been added before, 
            // but we're going to need it (or at least the
            // Strategy is going to need it to get to the Url):
            _services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // using the base method (rather than AddSingleton)
            // so that the lifetime can be adjusted as need be:
            _services.Add(
                ServiceDescriptor.Describe(
                    typeof(ITenantIdentifierExtractionStrategy),
                    typeof(TStrategy), lifetime));

            // So that additional Strategies can be added:
            return this;
        }

        /// <summary>
        /// Method to Register the tenant store implementation
        /// </summary>
        /// <typeparam name="TTenancyStoreImplementation"></typeparam>
        /// <param name="lifetime"></param>
        /// <returns></returns>
        public MultiTenancyBuilder<T> WithStore<TTenancyStoreImplementation>(
            ServiceLifetime lifetime = ServiceLifetime.Singleton)
            where TTenancyStoreImplementation : class, ITenantIdentityTenantStore<T>
        {

            // using the base method (rather than AddSingleton)
            // so that the lifetime can be adjusted as need be:
            _services.Add(
                 ServiceDescriptor.Describe(
                     typeof(ITenantIdentityTenantStore<T>),
                     typeof(TTenancyStoreImplementation),
                     lifetime));
            // So that additional Strategies can be added:
            return this;
        }


        public MultiTenancyBuilder<T> WithService<TService>(
            ServiceLifetime lifetime = ServiceLifetime.Singleton)
            where TService : class, ITenantIdentityService<TenantIdentity>
        {
            _services.TryAdd(
                 ServiceDescriptor.Describe(
                     typeof(ITenantIdentityService<T>),
                     typeof(TService),
                     lifetime));
            // So that additional Strategies can be added:
            return this;
        }

    }
}


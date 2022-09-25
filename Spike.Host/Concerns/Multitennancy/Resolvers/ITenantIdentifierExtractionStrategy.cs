namespace Spike.Host.Concerns.Multitennancy.Resolvers
{
    /// <summary>
    /// Contract for implementations 
    /// of strategies to extract Tenancy
    /// identifier from current context (url/etc).
    /// <para>
    /// Usage is:
    /// <code>
    /// <![CDATA[
    /// services.AddMultiTenancy()
    ///     .WithResolutionStrategy
    ///     <SomeExampleMultiTenancyTenantIdentifierResolutionStrategy>();
    /// ]]>
    /// </code>
    /// </para>
    /// <para>
    /// Note that later
    /// 
    /// </para>
    /// </summary>
    public interface ITenantIdentifierExtractionStrategy
    {

        /// <summary>
        /// Asynchronous
        /// method to determine Tenancy
        /// via this single strategy.
        /// </summary>
        /// <returns></returns>
        Task<string> GetTenantIdentifierAsync();
    }
}

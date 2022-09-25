using Microsoft.Extensions.Configuration;
using Spike.Host.Concerns.Multitennancy.Builder;

namespace Spike.Host.Concerns.Multitennancy.Resolvers.Implementations
{
    /// <summary>
    /// An implementation of the 
    /// <see cref="ITenantIdentifierExtractionStrategy"/>
    /// to determine the tenancy identity from the 
    /// DNS.
    /// <para>
    /// Usage is:
    /// <code>
    /// <![CDATA[
    /// services.AddMultiTenancy()
    ///     .WithResolutionStrategy
    ///     <HostBasedMultiTenancyTenantIdentifierResolutionStrategy>();
    ///     .WithStorage
    ///     <InMemoryMultiTenancyTenantStore>();
    /// ]]>
    /// </code>
    /// </para>
    /// </summary>
    public class HostBasedTenantIdentifierExtractionStrategy 
        : ITenantIdentifierExtractionStrategy
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public HostBasedTenantIdentifierExtractionStrategy(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Get the tenant identifier
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<string?> GetTenantIdentifierAsync()
        {
            var context = _httpContextAccessor.HttpContext;

            if (context == null)
            {
                return null;
            }
            //This is the whole host, minus ports.
            // including the subhost, but not isolated to it.
            var fullHostName = context.Request.Host.Host;

            if (Uri.CheckHostName(fullHostName) != UriHostNameType.Dns)
            {
                return null;
            }

            string defaultDomainName = "TODO";// configuration.DomainName;


            bool isDefaultDomain = false;
            if ((!string.IsNullOrEmpty(defaultDomainName))
                &&
               (fullHostName.EndsWith(defaultDomainName)))
            {
                isDefaultDomain = true;
            }


            // TODO: WARNING:
            //throw new Warning("Think we have a config error, No Provided DomainName.")

            var parts = fullHostName.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 1)
            {
                //It's only a localhost
                //with no subdomains.
                return null;
            }
            var lastpart = parts[parts.Length - 1];
            if (parts.Length == (lastpart == "localhost" ? 1 : 2))
            {
                // It's only a "corp.com" domain:
                // but maybe someone else will find something?
                return null;
            }

            // Solve for easiest condition:
            var firstpart = parts[0];
            if (firstpart == "www")
            {
                return null;
            }
            // But after that, it gets tricky as one can have
            // "sub.someorg.com",
            // but also:
            // "someorg.co.nz",
            // "someorg.parliament.nz",
            // "www.someorg.co.nz",
            // so it's not automatically "the first one"...
            // it's just best guess at that point:
            var potentialSecondTier = parts[parts.Length - 1];
            // This is crap, but without assistance from an 
            // external registry not seeing how it can be made 
            // better right now.
            List<string> knowns = new List<string>();
            knowns.AddRange(new[] { "co", "net", "org" });
            if (lastpart == "nz")
            {
                knowns.AddRange(new[] { "cri", "govt", "health", "iwi", "mil", "parliament" });
                knowns.AddRange(new[] { "school" });
                knowns.AddRange(new[] { "ac", "geek", "gen", "kiwi", "maori" });
            }
            if (knowns.Contains(potentialSecondTier))
            {
                // Not enough parts to have 4th tier:
                if (parts.Length <= 3)
                {
                    return null;
                }
            }

            //It's best guess I guess, for now?
            return firstpart;
        }
    }
}

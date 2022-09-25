namespace Spike.Host.Concerns.Multitennancy.Messages
{
    /// <summary>
    /// Minimal cacheable information
    /// regarding tenancy identity for
    /// resolution purposes.
    /// <para>
    /// Has to be small in size to be 
    /// fast therefore cacheable
    /// so as to not hit a database 
    /// on every request.
    /// </para>
    /// <para>
    /// Data that is needed is
    /// - the Id of the System Entity that
    /// contains more info as to the Entity.
    /// - the name(s) associated to this Tenant.
    /// (there can be more than one).
    /// </para>
    /// </summary>
    public class TenantIdentity // IHasGuidId, IHasIdentifier
    {
        /// <summary>
        /// The tenant Datastore Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The full domain that might belong to a tenant.
        /// <para>
        /// eg: <c>ibm.com</c>, or <c>internationalbusinessmachines.com</c>
        /// </para>
        /// </summary>
        public List<string> DomainNames { get; } = new List<string>();

        /// <summary>
        /// The unique tenant identifiers (ie Key)
        /// findable on the DNS/Header/etc.
        /// <para>
        /// eg: "ibm"
        /// </para>
        /// </summary>
        public List<string> Identifiers { get; } = new List<string>();

        ///// <summary>
        ///// Tenant items
        ///// </summary>
        //public Dictionary<string, object> Items { get; private set; } = new Dictionary<string, object>();
    }
}

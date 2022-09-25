using Microsoft.AspNetCore.Mvc;
using Spike.Host.Concerns.Multitennancy.Messages;
using Spike.Host.Concerns.Multitennancy.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Spike.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        public ITenantIdentityService<TenantIdentity> TenantIdentityService { get; }

        public ValuesController(ITenantIdentityService<TenantIdentity> tenantIdentityService)
        {
            TenantIdentityService = tenantIdentityService;
        }

        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {

            var r = await TenantIdentityService
                    .GetCurrentTenantIdentityAsync();

            return new[] { r.Id.ToString() };
            
        }

    }
}

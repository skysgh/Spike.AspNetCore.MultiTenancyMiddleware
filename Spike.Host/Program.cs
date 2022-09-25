using Spike.Host.Concerns.Multitennancy.Builder.ExtensionMethods;
using Spike.Host.Concerns.Multitennancy.Implementations.Storage;
using Spike.Host.Concerns.Multitennancy.Implementations.Storage.Implementations;
using Spike.Host.Concerns.Multitennancy.Messages;
using Spike.Host.Concerns.Multitennancy.Resolvers.Implementations;
using Spike.Host.Concerns.Multitennancy.Services.Implementations;

namespace Spike.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllersWithViews();

            builder.Services.AddTenantIdentityDetermination()
                .WithResolutionStrategy
                    <HostBasedTenantIdentifierExtractionStrategy >()
                .WithStore
                    <DemoTenantIdentityStore>()
                .WithStore
                    <UnFinishedTenantInfoStore>()
                .WithService<TenantIdentityService<TenantIdentity>>()
                    ;

            var app = builder.Build();

            var demoMode = true;
            if (demoMode)
            {
                // Turn off things 
                // that need integration
                // info until we are sure 
                // those dependencies are met:
                app.Services
                    .ChangeStateOfTenantInfoStore
                    <ITenantIdentityTenantStore<TenantIdentity>,
                    TenantIdentity>(
                    false);
            }

                
                
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();


            app.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action=Index}/{id?}");

            app.MapFallbackToFile("index.html");

            

            app.Run();
        }
    }
}
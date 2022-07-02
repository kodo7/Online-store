using System;
using Internet_veikals.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(Internet_veikals.Areas.Identity.IdentityHostingStartup))]
namespace Internet_veikals.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<Internet_veikalsContext>(options =>
                    options.UseNpgsql(
                        context.Configuration.GetConnectionString("Default")));
            });
        }
    }
}
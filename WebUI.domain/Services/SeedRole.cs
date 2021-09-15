using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OnlineBanking.Domain.Entities;

namespace WebUI.domain.Services
{
    public static class SeedRole
    {

        private static RoleManager<AppRole> _roleManager;

        public static async void EnsurePopulated(IApplicationBuilder app)
        {
            var context = app.ApplicationServices
                .CreateScope().ServiceProvider
                .GetRequiredService<AppDbContext>();
            if ((await context.Database.GetPendingMigrationsAsync()).Any()) await context.Database.MigrateAsync();

            _roleManager = app.ApplicationServices
                .CreateScope().ServiceProvider
                .GetRequiredService<RoleManager<AppRole>>();

            await Create("Admin");
            await Create("Customer");
               
        }

        public static async Task Create(string name)
        {
            if (!await _roleManager.RoleExistsAsync(name))
            {
                var role = new AppRole { Name = name };
                await _roleManager.CreateAsync(role);
            }
        }
    }
}
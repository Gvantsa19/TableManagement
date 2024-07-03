using Microsoft.EntityFrameworkCore;
using TMS.Infrastructure.Persistence;

namespace TMS.API.Extensions
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            using TMSDbContext dbContext = scope.ServiceProvider.GetRequiredService<TMSDbContext>();

            dbContext.Database.Migrate();
        }
    }
}

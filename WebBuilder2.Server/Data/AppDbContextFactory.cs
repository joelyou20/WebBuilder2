using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace WebBuilder2.Server.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer("Server=webbuilder2.cb5u2cbpj1rj.us-east-1.rds.amazonaws.com,1433;Database=webbuilder2;User Id=admin;Password=J992#431c$;TrustServerCertificate=True");

        return new AppDbContext(optionsBuilder.Options);
    }
}

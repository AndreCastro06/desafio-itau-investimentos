using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using InvestmentControlApi.Infrastructure.Data;

public class InvestmentDbContextFactory : IDesignTimeDbContextFactory<InvestmentDbContext>
{
    public InvestmentDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<InvestmentDbContext>();
        optionsBuilder.UseMySql(
            "server=localhost;port=3306;database=sistema_investimentos;user=root;password=",
            new MySqlServerVersion(new Version(8, 0, 36))
        );

        return new InvestmentDbContext(optionsBuilder.Options);
    }
}
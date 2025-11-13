using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using TicketHub.Data;

public class TicketHubContextFactory : IDesignTimeDbContextFactory<TicketHubContext>
{
    public TicketHubContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TicketHubContext>();

        // Azure SQL connection string with retry resiliency enabled
        optionsBuilder.UseSqlServer(
            "Server=tcp:nscc-0531643-mssql-server.database.windows.net,1433;" +
            "Initial Catalog=nscc-0531643-mssql-database;" +
            "Persist Security Info=False;" +
            "User ID=nsccadmin05;" +
            "Password=Fierdonick00227;" +
            "MultipleActiveResultSets=False;" +
            "Encrypt=True;" +
            "TrustServerCertificate=False;" +
            "Connection Timeout=30;",
            sqlOptions => sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,              // Number of retries
                maxRetryDelay: TimeSpan.FromSeconds(10), // Delay between retries
                errorNumbersToAdd: null        // Additional SQL error codes to retry
            )
        );

        return new TicketHubContext(optionsBuilder.Options);
    }
}
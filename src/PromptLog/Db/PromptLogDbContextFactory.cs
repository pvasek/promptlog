using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace PromptLog.Db;

public class PromptLogDbContextFactory: IDesignTimeDbContextFactory<PromptLogDbContext>
{
    public PromptLogDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<PromptLogDbContext>();
        var secretsId = "promptlog-app";
        var config = new ConfigurationManager().AddUserSecrets(secretsId).Build();
        var connectionString = config["DbConnectionString"];
        optionsBuilder.UseSqlServer(connectionString);
        return new PromptLogDbContext(optionsBuilder.Options);
    }
}
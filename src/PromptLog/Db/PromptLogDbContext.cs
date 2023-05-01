using Microsoft.EntityFrameworkCore;
using PromptLog.Entities;

namespace PromptLog.Db;

public class PromptLogDbContext: DbContext
{
    public PromptLogDbContext(DbContextOptions<PromptLogDbContext> options)
        : base(options)
    {
    }

    public DbSet<Prompt> Prompts => Set<Prompt>();
}
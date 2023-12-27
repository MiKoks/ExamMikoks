using DAL.EF.App;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DAL.EF.App;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder
            .UseSqlite("Data Source=/Users/mihke/Desktop/code/icd0021-22-23-s-2/ExamMikoks/app.db");

        return new ApplicationDbContext(optionsBuilder.Options);
    }

    
    
    
    /*public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        // does not actually connect to db
        optionsBuilder.UseNpgsql("Host=localhost:5450;Database=StudyGroup-db;Username=postgres;Password=postgres");
        return new ApplicationDbContext(optionsBuilder.Options);
    }*/
}

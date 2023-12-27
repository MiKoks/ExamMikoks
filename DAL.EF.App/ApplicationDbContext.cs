using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF.App;

public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, Guid>
{
//24feb/0:14:00 jätk
    //public DbSet<Courses> Courses { get; set; } = default!;
    public DbSet<Property> Properties { get; set; } = default!;
    public DbSet<Apartment> Apartments { get; set; } = default!;
    public DbSet<Lease> Leases { get; set; } = default!;
    public DbSet<Service> Services { get; set; } = default!;
    public DbSet<LeaseService> LeaseServices { get; set; } = default!;
    public DbSet<Bill> Bills { get; set; } = default!;
    public DbSet<ServiceConsumption> ServiceConsumptions { get; set; } = default!;
    
    
    //public DbSet<AppRefreshToken> AppRefreshTokens { get; set; } = default!;
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        //AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // let the initial stuff run
        base.OnModelCreating(builder);

        // disable cascade delete
        foreach (var foreignKey in builder.Model
                     .GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
        }
        
        builder.Entity<LeaseService>()
            .HasKey(ls => new { ls.LeaseId, ls.ServiceId });

        builder.Entity<LeaseService>()
            .HasOne(ls => ls.Lease)
            .WithMany(l => l.LeaseServices)
            .HasForeignKey(ls => ls.LeaseId);

        builder.Entity<LeaseService>()
            .HasOne(ls => ls.Service)
            .WithMany(s => s.LeaseServices)
            .HasForeignKey(ls => ls.ServiceId);
        builder.Entity<Apartment>()
            .HasOne(a => a.CurrentLease)
            .WithOne(l => l.Apartment)
            .HasForeignKey<Apartment>(a => a.CurrentLeaseId)
            .IsRequired(false); // Use IsRequired(false) if the lease can be null.

    }

}
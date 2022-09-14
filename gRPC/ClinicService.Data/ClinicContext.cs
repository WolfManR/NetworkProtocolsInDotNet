using Microsoft.EntityFrameworkCore;

namespace ClinicService.Data;

public class ClinicContext : DbContext
{
    public ClinicContext(DbContextOptions<ClinicContext> options) : base(options)
    {

    }

    // virtual keyword need for testing purposes
    // name of db set property in db context used as table name, if no overrides with table attribute or configurations set 
    public virtual DbSet<Client> Clients { get; set; } = null!;
    public virtual DbSet<Pet> Pets { get; set; } = null!;
    public virtual DbSet<Consultation> Consultations { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Client>(b =>
        {
            b.Property(t => t.Document).HasMaxLength(50);
            b.Property(t => t.Surname).HasMaxLength(255);
            b.Property(t => t.FirstName).HasMaxLength(255);
            b.Property(t => t.Patronymic).HasMaxLength(255);
        });

        modelBuilder.Entity<Consultation>(b =>
        {
            b.HasOne(t => t.Pet)
             .WithMany(t => t.Consultations)
             .HasForeignKey(t => t.PetId)
             .OnDelete(DeleteBehavior.NoAction);

            b.HasOne(t => t.Client)
             .WithMany(t => t.Consultations)
             .HasForeignKey(t => t.ClientId);
        });

        modelBuilder.Entity<Pet>(b =>
        {
            b.Property(t => t.Name).HasMaxLength(20);

            b.HasOne(t => t.Client)
             .WithMany(t => t.Pets)
             .HasForeignKey(t => t.ClientId);
        });
    }
}
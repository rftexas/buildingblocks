using address.domain;
using Microsoft.EntityFrameworkCore;

namespace address.persistence;

public class AddressContext : DbContext
{
    public DbSet<Address> Address { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(b =>
        {
            b.HasKey(a => a.AddressId);


            b.Property(a => a.AddressId).HasConversion<AddressId.EfCoreValueConverter>().ValueGeneratedOnAdd();
            b.Property(a => a.Street).HasMaxLength(250).IsRequired();
            b.Property(a => a.City).HasMaxLength(150).IsRequired();
            b.Property(a => a.State).HasMaxLength(50).IsRequired();
            b.Property(a => a.PostalCode).HasMaxLength(15).IsRequired();
        });
    }
}

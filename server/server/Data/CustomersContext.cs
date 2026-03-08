using Microsoft.EntityFrameworkCore;
using server.Models;

namespace server.Data;

public class CustomersContext(DbContextOptions<CustomersContext> options) : DbContext(options)
{
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<CustomerLog> CustomerLogs => Set<CustomerLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasIndex(c => new { c.Name, c.Address }).IsUnique(); 
        });

        modelBuilder.Entity<CustomerLog>(entity =>
        {
            entity.HasOne<Customer>()
                .WithMany()
                .HasForeignKey(l => l.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(l => l.Action)
                .HasConversion<string>();
        });
    }
}


using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using OrderService.Application.Interfaces;
using OrderService.Application.Saga;
using OrderService.Domain.Entities;

namespace OrderService.Infrastructure.Persistence;

public class OrderContext : SagaDbContext, IOrderContext
{
    public OrderContext(DbContextOptions<OrderContext> options)
    : base(options)
    { }
    protected override IEnumerable<ISagaClassMap> Configurations
    {
        get { yield return new OrderStateMap(); }
    }

    public DbSet<Order> Orders => Set<Order>();

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
      
        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.LogTo(Console.WriteLine);
    }

}

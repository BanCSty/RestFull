﻿using Microsoft.EntityFrameworkCore;
using RestFull.Models;

namespace RestFull.Migrations
{
    public class SetDbContext : DbContext
    {
        public DbSet<ServiceObject> serviceObjects { get; set; }
        public DbSet<Order> Orders { get; set; }

        public SetDbContext(DbContextOptions<SetDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ServiceObject>(entity =>
            {
                entity.Property(e => e.ID);
                entity.Property(e => e.Name).HasMaxLength(100);
                entity.Property(e => e.Amount);

                entity.HasData(new ServiceObject
                {
                    ID = "0f8fad5b-d9cb-469f-a165-70867728950e",
                    Name = "Blade",
                    Amount = 15
                });
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.idMain);
                entity.Property(e => e.ID);
                entity.Property(e => e.NameOrder).HasMaxLength(100);
                entity.Property(e => e.AmountOrder);

                entity.HasData(new Order
                {
                    idMain = 1,
                    ID = "0f8fad5b-d9cb-469f-a165-70867728950e",
                    AmountOrder = 2,
                    NameOrder = "Blade"
                });
            });
        }
    }
}

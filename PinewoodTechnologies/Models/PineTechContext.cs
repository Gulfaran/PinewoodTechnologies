using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PinewoodTechnologies.Models;

public partial class PineTechContext : DbContext
{
    public PineTechContext()
    {
    }

    public PineTechContext(DbContextOptions<PineTechContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //Todo : Add the connection string for the table
        optionsBuilder.UseSqlServer();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3213E83F71B68A3F");

            entity.ToTable("Customer");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.City)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("city");
            entity.Property(e => e.Contactnumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("contactnumber");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Extra)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("extra");
            entity.Property(e => e.Forename)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("forename");
            entity.Property(e => e.House)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("house");
            entity.Property(e => e.Organisationname)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("organisationname");
            entity.Property(e => e.Postcode)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("postcode");
            entity.Property(e => e.Street)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("street");
            entity.Property(e => e.Surname)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("surname");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

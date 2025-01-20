using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CurrencyXchange.Models;

public partial class CurrencyXchangeContext : DbContext
{
    public CurrencyXchangeContext()
    {
    }

    public CurrencyXchangeContext(DbContextOptions<CurrencyXchangeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Currency> Currencies { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserTransaction> UserTransactions { get; set; }

    public virtual DbSet<Wallet> Wallets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-AG6MD1C\\SQLEXPRESS;Initial Catalog=CurrencyXchange;Trusted_Connection=true;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Currency>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Currency_Id");

            entity.ToTable("Currency");

            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK_Users_UserId");

            entity.Property(e => e.Address).HasMaxLength(150);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Mobile).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(150);

            entity.HasOne(d => d.Currency).WithMany(p => p.Users)
                .HasForeignKey(d => d.CurrencyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_CurrencyId");
        });

        modelBuilder.Entity<UserTransaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Transactions");

            entity.ToTable("UserTransaction");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 3)");
            entity.Property(e => e.Balance).HasColumnType("decimal(18, 3)");
            entity.Property(e => e.Time).HasColumnType("datetime");
            entity.Property(e => e.TransactionType).HasMaxLength(100);

            entity.HasOne(d => d.Currency).WithMany(p => p.UserTransactions)
                .HasForeignKey(d => d.CurrencyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CurrencyId_Foreign");

            entity.HasOne(d => d.User).WithMany(p => p.UserTransactions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserTrans__UserI__37A5467C");

            entity.HasOne(d => d.Wallet).WithMany(p => p.UserTransactions)
                .HasForeignKey(d => d.WalletId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserTrans__Walle__38996AB5");
        });

        modelBuilder.Entity<Wallet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Wallet__3214EC07A2B84444");

            entity.ToTable("Wallet");

            entity.Property(e => e.Balance).HasColumnType("decimal(18, 3)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

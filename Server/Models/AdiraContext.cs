using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ADIRA.Server.Models;

public partial class AdiraContext : DbContext
{
    public AdiraContext()
    {
    }

    public AdiraContext(DbContextOptions<AdiraContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DepartmentL> DepartmentLs { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeeDataFromFile> EmployeeDataFromFiles { get; set; }

    public virtual DbSet<EntityL> EntityLs { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RoleL> RoleLs { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=LAPTOP-3HG7NRMH\\SQLEXPRESS;Integrated Security=true;Encrypt=false;Initial Catalog=Adira;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DepartmentL>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Departme__3214EC074466835C");

            entity.ToTable("Department_L");

            entity.Property(e => e.CreatedDate).HasColumnType("date");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.InActiveDate).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC078C0DA9AF");

            entity.ToTable("Employee");

            entity.Property(e => e.Email)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.HasOne(d => d.Department).WithMany(p => p.Employees)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK__Employee__Depart__59FA5E80");

            entity.HasOne(d => d.Entity).WithMany(p => p.Employees)
                .HasForeignKey(d => d.EntityId)
                .HasConstraintName("FK__Employee__Entity__59063A47");

            entity.HasOne(d => d.Role).WithMany(p => p.Employees)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__Employee__RoleId__5812160E");

            entity.HasOne(d => d.User).WithMany(p => p.Employees)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Employee__UserId__571DF1D5");
        });

        modelBuilder.Entity<EmployeeDataFromFile>(entity =>
        {
            entity.HasKey(e => e.EmployeeDataFromFileId).HasName("PK__Employee__560C15B26986B2F3");

            entity.ToTable("EmployeeDataFromFile");

            entity.Property(e => e.Department)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Entity)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Id)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<EntityL>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Entity_L__3214EC0746FE9380");

            entity.ToTable("Entity_L");

            entity.Property(e => e.CreatedDate).HasColumnType("date");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.InActiveDate).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Role__8AFACE1A22CF90FD");

            entity.ToTable("Role");

            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<RoleL>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Role_L");

            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C496B79FA");

            entity.Property(e => e.BlockExpirationDate).HasColumnType("datetime");
            entity.Property(e => e.IsBlocked).HasDefaultValueSql("((0))");
            entity.Property(e => e.LastLoginDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LoginAttempts).HasDefaultValueSql("((0))");
            entity.Property(e => e.Password)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(500)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

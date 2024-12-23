using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using QDApps.Models;
using QDApps.Models.WhereItAppModels;
using QDApps.Models.WhereItAppModels.ViewModels;

namespace QDApps.Context;

public partial class QdappsContext : DbContext
{
    public QdappsContext()
    {
    }

    public QdappsContext(DbContextOptions<QdappsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<ItemTag> ItemTags { get; set; }
    public virtual DbSet<ItemTagNames> ItemTagNames { get; set; }

    public virtual DbSet<Stash> Stashes { get; set; }
    public virtual DbSet<StashTags> StashTags { get; set; }
    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<QDApps.Models.TimeZone> TimeZones { get; set; }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<ViewItems> ViewItems { get; set; }
    public virtual DbSet<ViewStashes> ViewStashes { get; set; }
    public virtual DbSet<ViewTags> ViewTags { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=QDApps;Trusted_Connection=True;encrypt=false;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");

            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("AspNetUserRoles");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.ProviderKey).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.Name).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PK_ItemId");

            entity.ToTable("Items", "wia");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.ItemName).HasMaxLength(200);

            entity.HasOne(d => d.Stash).WithMany(p => p.Items)
                .HasForeignKey(d => d.StashId)
                .HasConstraintName("FK_Items_StashId");
        });

        modelBuilder.Entity<ItemTag>(entity =>
        {
            entity.HasKey(e => e.ItemTagId).HasName("PK_ItemTagId");

            entity.ToTable("ItemTags", "wia");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Item).WithMany(p => p.ItemTags)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ItemTags_ItemId");

            entity.HasOne(d => d.Tag).WithMany(p => p.ItemTags)
                .HasForeignKey(d => d.TagId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ItemTags_TagId");
        });


        modelBuilder.Entity<ItemTagNames>(entity =>
        {
            entity.ToView("ItemTagNames", "wia");
            entity.HasNoKey();


        });
        OnModelCreatingPartial(modelBuilder);

        modelBuilder.Entity<Stash>(entity =>
        {
            entity.HasKey(e => e.StashId).HasName("PK_StashId");

            entity.ToTable("Stashes", "wia");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.StashName).HasMaxLength(200);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.Stashes)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Stashes_UserId");
        });

        modelBuilder.Entity<StashTags>(entity =>
        {
            entity.ToView("StashTags", "wia");
            entity.HasNoKey();
           

        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.TagId).HasName("PK_TagId");

            entity.ToTable("Tags", "wia");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.TagName).HasMaxLength(200);
            entity.Property(e => e.TagColor).HasMaxLength(10);
            entity.Property(e => e.TagDescription).HasMaxLength(2000);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.Tags)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tags_UserId");
        });

        modelBuilder.Entity<QDApps.Models.TimeZone>(entity =>
        {
            entity.HasKey(e => e.TimeZoneId).HasName("PK_TimeZoneId");

            entity.Property(e => e.TimeZoneName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Utcoffset).HasColumnName("UTCOffset");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK_UserId");

            entity.Property(e => e.AspNetUserId).HasMaxLength(450);
            entity.Property(e => e.TimeZoneId).HasDefaultValue(6);
            entity.Property(e => e.UserName).HasMaxLength(300);

            entity.HasOne(d => d.AspNetUser).WithMany(p => p.Users)
                .HasForeignKey(d => d.AspNetUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_AspNetUserId");

            entity.HasOne(d => d.TimeZone).WithMany(p => p.Users)
                .HasForeignKey(d => d.TimeZoneId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_TimeZoneId");
        });

        modelBuilder.Entity<ViewItems>(entity =>
        {
            entity.ToView("ViewItems", "wia");
            entity.HasNoKey();


        });

        modelBuilder.Entity<ViewStashes>(entity =>
        {
            entity.ToView("ViewStashes", "wia");
            entity.HasNoKey();


        });

        modelBuilder.Entity<ViewTags>(entity =>
        {
            entity.ToView("ViewTags", "wia");
            entity.HasNoKey();


        });
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

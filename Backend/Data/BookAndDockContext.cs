using System;
using System.Collections.Generic;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data;

public partial class BookAndDockContext : DbContext
{
    public BookAndDockContext(DbContextOptions<BookAndDockContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<DockingSpot> DockingSpots { get; set; }

    public virtual DbSet<Guide> Guides { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<Port> Ports { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Bookings_pkey");

            entity.HasIndex(e => e.DockingSpotId, "IX_Bookings_DockingSpotId");

            entity.HasIndex(e => e.PaymentMethodId, "IX_Bookings_PaymentMethodId");

            entity.HasIndex(e => e.SailorId, "IX_Bookings_SailorId");

            entity.Property(e => e.Id).HasDefaultValueSql("nextval('bookings_id_seq'::regclass)");
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.IsPaid).HasDefaultValue(false);

            entity.HasOne(d => d.DockingSpot).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.DockingSpotId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DockingSpotId");

            entity.HasOne(d => d.PaymentMethod).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.PaymentMethodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PaymentMethodId");

            entity.HasOne(d => d.Sailor).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.SailorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SailorId");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Comments_pkey");

            entity.HasIndex(e => e.CreatedBy, "IX_Comments_CreatedBy");

            entity.HasIndex(e => e.GuideId, "IX_Comments_GuideId");

            entity.Property(e => e.Id).HasDefaultValueSql("nextval('comments_id_seq'::regclass)");
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Comments)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK_CreatedBy");

            entity.HasOne(d => d.Guide).WithMany(p => p.Comments)
                .HasForeignKey(d => d.GuideId)
                .HasConstraintName("FK_GuideId");
        });

        modelBuilder.Entity<DockingSpot>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("DockingSpots_pkey");

            entity.HasIndex(e => e.OwnerId, "IX_DockingSpots_OwnerId");

            entity.HasIndex(e => e.PortId, "IX_DockingSpots_PortId");

            entity.Property(e => e.Id).HasDefaultValueSql("nextval('dockingspots_id_seq'::regclass)");
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.IsAvailable).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.Owner).WithMany(p => p.DockingSpots)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OwnerId");

            entity.HasOne(d => d.Port).WithMany(p => p.DockingSpots)
                .HasForeignKey(d => d.PortId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PortId");
        });

        modelBuilder.Entity<Guide>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Guides_pkey");

            entity.HasIndex(e => e.CreatedBy, "IX_Guides_CreatedBy");

            entity.Property(e => e.Id).HasDefaultValueSql("nextval('guides_id_seq'::regclass)");
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.IsApproved).HasDefaultValue(false);
            entity.Property(e => e.Title).HasMaxLength(100);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Guides)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK_AuthorId");
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Images_pkey");

            entity.HasIndex(e => e.CreatedBy, "IX_Images_CreatedBy");

            entity.HasIndex(e => e.GuideId, "IX_Images_GuideId");

            entity.Property(e => e.Id).HasDefaultValueSql("nextval('images_id_seq'::regclass)");
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Images)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK_CreatedBy");

            entity.HasOne(d => d.Guide).WithMany(p => p.Images)
                .HasForeignKey(d => d.GuideId)
                .HasConstraintName("FK_GuideId");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Locations_pkey");

            entity.HasIndex(e => e.DockingSpotId, "IX_Locations_DockingSpotId");

            entity.HasIndex(e => e.PortId, "IX_Locations_PortId");

            entity.Property(e => e.CreatedOn).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Town).HasMaxLength(100);

            entity.HasOne(d => d.DockingSpot).WithMany(p => p.Locations)
                .HasForeignKey(d => d.DockingSpotId)
                .HasConstraintName("FK_DockingSpotId");

            entity.HasOne(d => d.Port).WithMany(p => p.Locations)
                .HasForeignKey(d => d.PortId)
                .HasConstraintName("FK_PortId");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Notifications_pkey");

            entity.HasIndex(e => e.CreatedBy, "IX_Notifications_CreatedBy");

            entity.Property(e => e.Id).HasDefaultValueSql("nextval('notifications_id_seq'::regclass)");
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK_CreatedBy");
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PaymentMethods_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("nextval('paymentmethods_id_seq'::regclass)");
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Name).HasMaxLength(20);
        });

        modelBuilder.Entity<Port>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Ports_pkey");

            entity.HasIndex(e => e.OwnerId, "IX_Ports_OwnerId");

            entity.Property(e => e.Id).HasDefaultValueSql("nextval('port_id_seq'::regclass)");
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.IsApproved).HasDefaultValue(false);
            entity.Property(e => e.Name).HasMaxLength(255);

            entity.HasOne(d => d.Owner).WithMany(p => p.Ports)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OwnerId");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Reviews_pkey");

            entity.HasIndex(e => e.DockId, "IX_Reviews_DockId");

            entity.HasIndex(e => e.UserId, "IX_Reviews_UserId");

            entity.Property(e => e.Id).HasDefaultValueSql("nextval('reviews_id_seq'::regclass)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Dock).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.DockId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PortId");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_CreatedBy");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Roles_pkey");

            entity.HasIndex(e => e.Name, "Roles_Name_key").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("nextval('roles_id_seq'::regclass)");
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Name).HasMaxLength(20);
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Services_pkey");

            entity.HasIndex(e => e.DockingSpotId, "IX_Services_DockingSpotId");

            entity.HasIndex(e => e.PortId, "IX_Services_PortId");

            entity.Property(e => e.Id).HasDefaultValueSql("nextval('services_id_seq'::regclass)");
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.IsAvailable).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Price).HasPrecision(10, 2);

            entity.HasOne(d => d.DockingSpot).WithMany(p => p.Services)
                .HasForeignKey(d => d.DockingSpotId)
                .HasConstraintName("FK_DockingSpotId");

            entity.HasOne(d => d.Port).WithMany(p => p.Services)
                .HasForeignKey(d => d.PortId)
                .HasConstraintName("FK_PortId");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Users_pkey");

            entity.HasIndex(e => e.Email, "IDX_Users_Email").IsUnique();

            entity.HasIndex(e => e.RoleId, "IX_Users_RoleId");

            entity.HasIndex(e => e.Email, "Users_Email_key").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("nextval('users_id_seq'::regclass)");
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.Surname).HasMaxLength(50);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_RoleId");
        });
        modelBuilder.HasSequence("bookings_id_seq");
        modelBuilder.HasSequence("comments_id_seq");
        modelBuilder.HasSequence("dockingspots_id_seq");
        modelBuilder.HasSequence("guides_id_seq");
        modelBuilder.HasSequence("images_id_seq");
        modelBuilder.HasSequence("notifications_id_seq");
        modelBuilder.HasSequence("paymentmethods_id_seq");
        modelBuilder.HasSequence("port_id_seq");
        modelBuilder.HasSequence("reviews_id_seq");
        modelBuilder.HasSequence("roles_id_seq");
        modelBuilder.HasSequence("services_id_seq");
        modelBuilder.HasSequence("users_id_seq");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

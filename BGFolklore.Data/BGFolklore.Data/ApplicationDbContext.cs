using BGFolklore.Data.Models;
using BGFolklore.Data.Models.Calendar;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BGFolklore.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        private const int GuidMaxLength = 36;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<PublicEvent> PublicEvents { get; set; }
        public DbSet<Town> Towns { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<Feedback> Feedback { get; set; }
        public DbSet<Rating> Rating { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            OnIdentityCreating(builder);
            OnUserCreating(builder);
            OnPublicEventCreating(builder);
            OnFeedbackCreating(builder);
            OnRatingCreating(builder);

            builder.Entity<Town>().HasIndex(t => t.AreaId);

            base.OnModelCreating(builder);
        }

        private void OnUserCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasMany(u => u.PublicEvents)
                .WithOne(pe => pe.Owner)
                .HasForeignKey(pe => pe.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private void OnIdentityCreating(ModelBuilder builder)
        {
            builder.Entity<IdentityUserLogin<string>>().Property(login => login.LoginProvider).HasMaxLength(GuidMaxLength);
            builder.Entity<IdentityUserLogin<string>>().Property(login => login.ProviderKey).HasMaxLength(GuidMaxLength);

            builder.Entity<IdentityUserRole<string>>().Property(userRole => userRole.UserId).HasMaxLength(GuidMaxLength);
            builder.Entity<IdentityUserRole<string>>().Property(userRole => userRole.RoleId).HasMaxLength(GuidMaxLength);

            builder.Entity<User>().Property(user => user.Id).HasMaxLength(GuidMaxLength);
            builder.Entity<IdentityRole>().Property(role => role.Id).HasMaxLength(GuidMaxLength);

            builder.Entity<IdentityRoleClaim<string>>().Property(rc => rc.RoleId).HasMaxLength(GuidMaxLength);
            builder.Entity<IdentityUserClaim<string>>().Property(uc => uc.UserId).HasMaxLength(GuidMaxLength);

            builder.Entity<IdentityUserToken<string>>().Property(userToken => userToken.UserId).HasMaxLength(GuidMaxLength);
            builder.Entity<IdentityUserToken<string>>().Property(userToken => userToken.LoginProvider).HasMaxLength(GuidMaxLength);
            builder.Entity<IdentityUserToken<string>>().Property(userToken => userToken.Name).HasMaxLength(GuidMaxLength);
        }

        private void OnPublicEventCreating(ModelBuilder builder)
        {
            builder.Entity<PublicEvent>().Property(pe => pe.OwnerId).HasMaxLength(GuidMaxLength);

            builder.Entity<PublicEvent>()
                .HasOne(pe => pe.Owner)
                .WithMany(u => u.PublicEvents)
                .HasForeignKey(pe => pe.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<PublicEvent>()
                .HasOne(pe => pe.Town)
                .WithMany(t => t.PublicEvents)
                .HasForeignKey(pe => pe.TownId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<PublicEvent>()
                .HasOne(pe => pe.Status)
                .WithMany()
                .HasForeignKey(pe => pe.StatusId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private void OnFeedbackCreating(ModelBuilder builder)
        {
            builder.Entity<Feedback>()
                .HasOne(f => f.Owner)
                .WithMany()
                .HasForeignKey(f => f.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Feedback>()
                .HasOne(f=>f.Status)
                .WithMany()
                .HasForeignKey(f => f.StatusId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Feedback>()
                .HasOne(f => f.Event)
                .WithMany(pe => pe.Feedbacks)
                .HasForeignKey(f => f.EventId)
                .OnDelete(DeleteBehavior.Restrict);
        }
        private void OnRatingCreating(ModelBuilder builder)
        {
            builder.Entity<Rating>()
                .HasOne(r => r.Owner)
                .WithMany()
                .HasForeignKey(r => r.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Rating>()
                .HasOne(r => r.Event)
                .WithMany()
                .HasForeignKey(r => r.EventId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

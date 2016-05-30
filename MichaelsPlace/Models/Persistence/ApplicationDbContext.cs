using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Ninject;

namespace MichaelsPlace.Models.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", false)
        {
        }

        public DbSet<Case> Cases { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<Situation> Situations { get; set; }

        public DbSet<Item> Items { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<Organization> Organizations { get; set; }

        public DbSet<Event> Events { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public ApplicationDbContext ConfiguredForFastQueries()
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.AutoDetectChangesEnabled = false;
            Configuration.ProxyCreationEnabled = false;
            return this;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Case>().HasRequired(c => c.CreatedBy).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Case>().HasKey(c => c.Id).Property(c => c.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<Case>().HasMany(c => c.CaseUsers).WithRequired(u => u.Case).WillCascadeOnDelete(false);
            modelBuilder.Entity<Case>().HasMany(c => c.CaseItems).WithRequired(u => u.Case).WillCascadeOnDelete(false);

            //modelBuilder.Entity<Item>().HasRequired(i => i.CreatedBy).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Item>().HasMany(i => i.Situations).WithMany();

            modelBuilder.Entity<Situation>().HasMany(s => s.Losses).WithMany();
            modelBuilder.Entity<Situation>().HasMany(s => s.Mourners).WithMany();
            modelBuilder.Entity<Situation>().HasMany(s => s.Demographics).WithMany();

            //modelBuilder.Entity<Notification>().HasRequired(n => n.CreatedBy).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Comment>().HasRequired(n => n.CaseItem).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Invitation>().HasRequired(n => n.Case).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Invitation>().HasOptional(n => n.Invitee).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationUser>().HasMany(u => u.UserCaseItems).WithRequired(uci => uci.User).WillCascadeOnDelete(false);
            modelBuilder.Entity<ApplicationUser>().HasMany(u => u.CaseUsers).WithRequired(cu => cu.User).WillCascadeOnDelete(false);
        }

        public override int SaveChanges()
        {
            try
            {
                OnSaving();
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                throw ClarifyValidationErrors(ex);
            }
        }

        public override Task<int> SaveChangesAsync()
        {
            try
            {
                OnSaving();
                return base.SaveChangesAsync();
            }
            catch (DbEntityValidationException ex)
            {
                throw ClarifyValidationErrors(ex);
            }
        }

        private Exception ClarifyValidationErrors(DbEntityValidationException ex)
        {
            var errors = ex.EntityValidationErrors.SelectMany(e => e.ValidationErrors)
                           .Select(e => $"{e.PropertyName}: {e.ErrorMessage}");

            var message = $"Validation errors: {string.Join("; ", errors)}";

            return new Exception(message, ex);
        }

        private void OnSaving()
        {
            var situationChanges = ChangeTracker.Entries<Situation>().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var situationChange in situationChanges)
            {
                situationChange.Entity.UpdateMemento();
            }

            var newEntitys = ChangeTracker.Entries().Where(e => e.State == EntityState.Added && e.Entity is ICreated)
                                          .Select(e => e.Entity)
                                          .OfType<ICreated>();
            
            foreach (var newEntity in newEntitys)
            {
                newEntity.CreatedUtc = DateTimeOffset.UtcNow;
                if (string.IsNullOrEmpty(newEntity.CreatedBy))
                {
                    newEntity.CreatedBy = ClaimsPrincipal.Current?.Identity?.Name;
                }
                if (string.IsNullOrEmpty(newEntity.CreatedBy))
                {
                    newEntity.CreatedBy = "Anonymous";
                }
            }
        }
    }
}
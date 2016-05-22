using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MichaelsPlace.Models.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", false)
        {
        }

        public IDbSet<Case> Cases { get; set; }

        public IDbSet<Tag> Tags { get; set; }

        public IDbSet<Situation> Situations { get; set; }

        public IDbSet<Item> Items { get; set; }

        public IDbSet<Notification> Notifications { get; set; }

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

            modelBuilder.Entity<Case>().HasRequired(c => c.CreatedBy).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Case>().HasKey(c => c.Id).Property(c => c.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<Case>().HasMany(c => c.CaseUsers).WithRequired(u => u.Case).WillCascadeOnDelete(false);
            modelBuilder.Entity<Case>().HasMany(c => c.CaseItems).WithRequired(u => u.Case).WillCascadeOnDelete(false);

            modelBuilder.Entity<Item>().HasRequired(i => i.CreatedBy).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Item>().HasMany(i => i.Situations).WithMany();

            modelBuilder.Entity<Situation>().HasMany(s => s.Losses).WithMany();
            modelBuilder.Entity<Situation>().HasMany(s => s.Mourners).WithMany();
            modelBuilder.Entity<Situation>().HasMany(s => s.Demographics).WithMany();

            modelBuilder.Entity<Notification>().HasRequired(n => n.CreatedBy).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Comment>().HasRequired(n => n.CaseItem).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Invitation>().HasRequired(n => n.Case).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Invitation>().HasOptional(n => n.Invitee).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationUser>().HasMany(u => u.UserCaseItems).WithRequired(uci => uci.User).WillCascadeOnDelete(false);
            modelBuilder.Entity<ApplicationUser>().HasMany(u => u.CaseUsers).WithRequired(cu => cu.User).WillCascadeOnDelete(false);
        }
    }
}
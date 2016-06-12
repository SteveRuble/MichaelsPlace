using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reactive.Disposables;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Elmah;
using EntityFramework.DynamicFilters;
using JetBrains.Annotations;
using MichaelsPlace.Infrastructure;
using MichaelsPlace.Utilities;
using Microsoft.AspNet.Identity.EntityFramework;
using Ninject;
using Serilog;

namespace MichaelsPlace.Models.Persistence
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
    }

    /// <summary>
    /// Interface which allows reads of data, but does not expose any SaveChanges functionality.
    /// </summary>
    public interface IDbSetAdapter
    {
        IDbSet<TEntity> Set<TEntity>() where TEntity : class;
    }

    /// <summary>
    /// Entity Framework <see cref="DbContext"/> for this application.
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IDbSetAdapter
    {
        private ILogger _logger;

        private IMessageBus _messageBus;

        private bool _saving;

        [Inject]
        [CanBeNull]
        public IMessageBus MessageBus
        {
            get { return _messageBus; }
            set { _messageBus = value; }
        }

        [Inject]
        [CanBeNull]
        public ILogger Logger
        {
            get { return _logger;}
            set
            {
                if (value == null)
                {
                    _logger = null;
                    Database.Log = null;
                }
                else
                {
                    _logger = value.ForContext(typeof(ApplicationDbContext));
                    if (_logger != null)
                    {
                        Database.Log = m => _logger.Debug(m);
                    }
                }
            }
        }

        /// <summary>
        /// Create new instance which will use the DefaultConnection connection string.
        /// </summary>
        public ApplicationDbContext() : base("DefaultConnection", false)
        {
            Initialize();
        }

        /// <summary>
        /// Create new instance which will use the provided connection, which must be opened and closed by the caller.
        /// </summary>
        public ApplicationDbContext(DbConnection connection) : base(connection, false)
        {
            Initialize();
        }

        private void Initialize()
        {
            SoftDeleteControl = new DbContextFilterControl(Constants.EntityFrameworkFilters.SoftDelete, this, true);
        }

        public DbContextFilterControl SoftDeleteControl { get; private set; }

        /// <summary>
        /// Table of <see cref="Person"/> records, which represent individuals we know about (and which may or may not have logins).
        /// </summary>
        public DbSet<Person> People { get; set; }

        /// <summary>
        /// Cases, which are the primary domain object and represent a collection of articles and to-dos associated with one or more users.
        /// </summary>
        public DbSet<Case> Cases { get; set; }

        /// <summary>
        /// Tags are used to define situations.
        /// </summary>
        public DbSet<Tag> Tags { get; set; }

        /// <summary>
        /// Situations describe the ways in which other entities are related - how a user is related to a case, or a case to an item.
        /// </summary>
        public DbSet<Situation> Situations { get; set; }

        /// <summary>
        /// Items are pieces of content which are presented to users as part of a case.
        /// </summary>
        public DbSet<Item> Items { get; set; }

        /// <summary>
        /// Notifications are messages or comments that are produced as users interact with the application.
        /// </summary>
        public DbSet<Notification> Notifications { get; set; }

        /// <summary>
        /// Preferences for user communication and application behavior.
        /// </summary>
        public DbSet<UserPreference> UserPreferences { get; set; }

        /// <summary>
        /// Organizations are collections of users which have pre-defined case patterns and user roles.
        /// </summary>
        public DbSet<Organization> Organizations { get; set; }

        /// <summary>
        /// Historical events record important changes to other domain objects, which may or may not result in notifications.
        /// </summary>
        public DbSet<HistoricalEvent> HistoricalEvents { get; set; }

        /// <summary>
        /// Addresses are used to communicate with real-world entities.
        /// </summary>
        public DbSet<Address> Addresses { get; set; }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <returns></returns>
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        /// <summary>
        /// Returns <c>this</c> instance with lazy-loading, change detection, and proxy creation disabled.
        /// After this method is called, the instance should only be used for doing queries, not for updates, as 
        /// it will no longer enforce consistence by validation.
        /// </summary>
        /// <returns></returns>
        public ApplicationDbContext ConfiguredForFastQueries()
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.AutoDetectChangesEnabled = false;
            Configuration.ProxyCreationEnabled = false;
            return this;
        }

        /// <summary>
        /// Configures the relationships between tables.
        /// </summary>
        /// <param name="modelBuilder"></param>
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

            modelBuilder.Entity<Person>().HasMany(u => u.PersonCaseItems).WithRequired(uci => uci.Person).WillCascadeOnDelete(false);
            modelBuilder.Entity<Person>().HasMany(u => u.PersonCases).WithRequired(cu => cu.Person).WillCascadeOnDelete(false);
            modelBuilder.Entity<Person>().HasOptional(u => u.ApplicationUser).WithRequired(cu => cu.Person).WillCascadeOnDelete(false);

            modelBuilder.Entity<Organization>().HasMany(o => o.Cases).WithOptional(c => c.Organization).WillCascadeOnDelete(false);
            modelBuilder.Entity<Organization>().HasMany(o => o.People).WithOptional(c => c.Organization).WillCascadeOnDelete(false);
            modelBuilder.Entity<Organization>().HasMany(o => o.Situations).WithMany();

            modelBuilder.Filter(Constants.EntityFrameworkFilters.SoftDelete, (ISoftDelete e) => e.IsDeleted != true);
        }

        /// <summary>
        /// Saves all changes made in this context to the underlying database.
        /// </summary>
        /// <returns>
        /// The number of state entries written to the underlying database. This can include
        /// state entries for entities and/or relationships. Relationship state entries are created for
        /// many-to-many relationships and relationships where there is no foreign key property
        /// included in the entity class (often referred to as independent associations).
        /// </returns>
        /// <exception cref="T:System.Data.Entity.Infrastructure.DbUpdateException">An error occurred sending updates to the database.</exception>
        /// <exception cref="T:System.Data.Entity.Infrastructure.DbUpdateConcurrencyException">
        /// A database command did not affect the expected number of rows. This usually indicates an optimistic
        /// concurrency violation; that is, a row has been changed in the database since it was queried.
        /// </exception>
        /// <exception cref="T:System.Data.Entity.Validation.DbEntityValidationException">
        /// The save was aborted because validation of entity property values failed.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        /// An attempt was made to use unsupported behavior such as executing multiple asynchronous commands concurrently
        /// on the same context instance.</exception>
        /// <exception cref="T:System.ObjectDisposedException">The context or connection have been disposed.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        /// Some error occurred attempting to process entities in the context either before or after sending commands
        /// to the database.
        /// </exception>
        public override int SaveChanges()
        {
            var added = OnSaving();
            int result = base.SaveChanges();
            OnSaved(added);
            return result;
        }

        /// <summary>
        /// Asynchronously saves all changes made in this context to the underlying database.
        /// </summary>
        /// <remarks>
        /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
        /// that any asynchronous operations have completed before calling another method on this context.
        /// </remarks>
        /// <returns>
        /// A task that represents the asynchronous save operation.
        /// The task result contains the number of state entries written to the underlying database. This can include
        /// state entries for entities and/or relationships. Relationship state entries are created for
        /// many-to-many relationships and relationships where there is no foreign key property
        /// included in the entity class (often referred to as independent associations).
        /// </returns>
        /// <exception cref="T:System.Data.Entity.Infrastructure.DbUpdateException">An error occurred sending updates to the database.</exception>
        /// <exception cref="T:System.Data.Entity.Infrastructure.DbUpdateConcurrencyException">
        /// A database command did not affect the expected number of rows. This usually indicates an optimistic
        /// concurrency violation; that is, a row has been changed in the database since it was queried.
        /// </exception>
        /// <exception cref="T:System.Data.Entity.Validation.DbEntityValidationException">
        /// The save was aborted because validation of entity property values failed.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        /// An attempt was made to use unsupported behavior such as executing multiple asynchronous commands concurrently
        /// on the same context instance.</exception>
        /// <exception cref="T:System.ObjectDisposedException">The context or connection have been disposed.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        /// Some error occurred attempting to process entities in the context either before or after sending commands
        /// to the database.
        /// </exception>
        public override async Task<int> SaveChangesAsync()
        {
            var added = OnSaving();
            int result = await base.SaveChangesAsync();
            OnSaved(added);
            return result;
        }

        private void OnSaved(List<object> added)
        {
            PublishAddedEntities(added);
        }

        private List<object> OnSaving()
        {
            if (_saving)
            {
                throw new System.ApplicationException(
                    "Recursive SaveChanges call detected. If you are responding to a EntityChanging event, do not call SaveChanges on the associated DbContext instance.");
            }
            _saving = true;
            try
            {
                UpdateSituationMementos();
                UpdateTimestamps();
                ApplySoftDelete();
                var errors = GetValidationErrors().ToList();
                if (errors.Any())
                {
                    throw ConstructValidationException(errors);
                }
                PublishChangingEntities();
                var added = GetAddedEntities();
                return added;
            }
            finally
            {
                _saving = false;
            }
        }

        private void ApplySoftDelete()
        {
            var deletedEntries= ChangeTracker.Entries().Where(t => t.State == EntityState.Deleted && t.Entity is ISoftDelete)
                                              .Select(e => e.Cast<ISoftDelete>())
                                              .ToList();
            foreach (var dbEntityEntry in deletedEntries)
            {
                dbEntityEntry.Entity.IsDeleted = true;
                dbEntityEntry.State = EntityState.Modified;
            }
        }

        private static Exception ConstructValidationException(List<DbEntityValidationResult> validationResults)
        {
            var errors = validationResults.SelectMany(r => r.ValidationErrors.Select(e => $"{r.Entry.State} {System.Data.Entity.Core.Objects.ObjectContext.GetObjectType(r.Entry.Entity.GetType()).Name}.{e.PropertyName}: {e.ErrorMessage}"));
            
            var message = $"Validation errors: {string.Join("; ", errors)}";

            return new DbEntityValidationException(message, validationResults);
        }

        private List<object> GetAddedEntities()
        {
            return ChangeTracker.Entries().Where(e => e.State == EntityState.Added).Select(e => e.Entity).ToList();
        }

        private List<object> GetChangedEntities()
        {
            return ChangeTracker.Entries().Where(e => e.State == EntityState.Added).Select(e => e.Entity).ToList();
        }
        
        private void UpdateTimestamps()
        {
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

        private void UpdateSituationMementos()
        {
            var situationChanges = ChangeTracker.Entries<Situation>().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var situationChange in situationChanges)
            {
                situationChange.Entity.UpdateMemento();
            }
        }

        private void PublishAddedEntities(List<object> addedEntities)
        {
            var methodDefinition = typeof(ApplicationDbContext).GetMethod("PublishAdded", BindingFlags.Instance|BindingFlags.NonPublic).GetGenericMethodDefinition();
            foreach (var addedEntity in addedEntities)
            {
                var method = methodDefinition.MakeGenericMethod(addedEntity.GetType());
                method.Invoke(this, new[] {addedEntity});
            }
        }

        private void PublishChangingEntities()
        {
            var methodDefinition = typeof(ApplicationDbContext).GetMethod("PublishChanging", BindingFlags.Instance | BindingFlags.NonPublic).GetGenericMethodDefinition();
            foreach (var modifiedEntry in ChangeTracker.Entries().Where(e => e.State == EntityState.Modified))
            {
                var method = methodDefinition.MakeGenericMethod(modifiedEntry.Entity.GetType());
                method.Invoke(this, new object[] { modifiedEntry });
            }
        }
        
        [UsedImplicitly]
        private void PublishAdded<T>(T entity) where T : class
        {
            if (_messageBus != null)
            {
                var entityAdded = new EntityAdded<T>(this, entity);
                _messageBus.Publish(entityAdded);
            }
        }

        [UsedImplicitly]
        private void PublishChanging<T>(DbEntityEntry dbEntityEntry) where T : class
        {
            if (_messageBus != null)
            {
                var previous = (T) dbEntityEntry.OriginalValues.ToObject();
                var current = (T)dbEntityEntry.Entity;
                var entityChanging = new EntityUpdating<T>(this, previous, current);
                _messageBus.Publish(entityChanging);
            }
        }

        IDbSet<TEntity> IDbSetAdapter.Set<TEntity>() => Set<TEntity>();
    }

    public class EntityUpdating<T>
    {
        public EntityUpdating(IDbSetAdapter dbSetAdapter, T previous, T current)
        {
            Current = current;
            DbSetAdapter = dbSetAdapter;
            Previous = previous;
        }

        public IDbSetAdapter DbSetAdapter { get; set; }
        public T Previous { get; set; }
        public T Current { get; set; }
    }

    public class EntityAdded<T>
    {
        public EntityAdded(IDbSetAdapter dbSetAdapter, T entity)
        {
            DbSetAdapter = dbSetAdapter;
            Entity = entity;
        }

        public IDbSetAdapter DbSetAdapter { get; set; }
        public T Entity { get; set; }
    }
}
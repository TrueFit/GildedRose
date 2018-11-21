using System;
using GildedRose.Data.Configurations;
using GildedRose.Data.Interfaces;
using GildedRose.Model;
using Microsoft.EntityFrameworkCore;

namespace GildedRose.Data
{
    public class StoreContext : DbContext, IStoreContext
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options) { }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>  Map our POCO data classes to our EF Core in memory datastore.</summary>
        ///-------------------------------------------------------------------------------------------------

        public DbSet<StoreItemDto> StoreItems { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Commits this object. </summary>
        ///-------------------------------------------------------------------------------------------------

        public virtual void Commit()
        {
            base.SaveChanges();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Override this method to further configure the model that was discovered by convention
        ///     from the entity types exposed in <see cref="T:Microsoft.EntityFrameworkCore.DbSet`1" />
        ///     properties on your derived context. The resulting model may be cached and re-used for
        ///     subsequent instances of your derived context.
        /// </summary>
        ///
        /// <remarks>
        ///     If a model is explicitly set on the options for this context (via
        ///     <see cref="M:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.UseModel(Microsoft.EntityFrameworkCore.Metadata.IModel)" />)
        ///     then this method will not be run.
        /// </remarks>
        ///
        /// <param name="modelBuilder"> The builder being used to construct the model for this context.
        ///                             Databases (and other extensions) typically define extension
        ///                             methods on this object that allow you to configure aspects of the
        ///                             model that are specific to a given database. </param>
        ///-------------------------------------------------------------------------------------------------

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new StoreItemConfiguration());
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Overriding this method to configure the database (and other options) to be used for 
        ///           this context. This method is called for each instance of the context that is created. 
        ///           The base implementation does nothing and we need to set the No Tracking since we don’t 
        ///           need to track operations when querying and updating data.
        /// </summary>
        ///
        /// <param name="optionsBuilder">   A builder used to create or modify options for this context.
        ///                                 Databases (and other extensions) typically define extension 
        ///                                 methods on this object that allow us to configure the context. 
        ///                                 </param>
        ///-------------------------------------------------------------------------------------------------

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

    }
}

namespace WorldTour.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class WorldTourContext : DbContext
    {
        public WorldTourContext()
            : base("name=WorldTourContext")
        {
        }

        public virtual DbSet<Booking> Booking { get; set; }
        public virtual DbSet<Flights> Flights { get; set; }
        public virtual DbSet<Tickets> Tickets { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Booking>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<Booking>()
                .Property(e => e.Class)
                .IsUnicode(false);

            modelBuilder.Entity<Booking>()
                .HasMany(e => e.Tickets)
                .WithRequired(e => e.Booking)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Flights>()
                .Property(e => e.Company_Name)
                .IsUnicode(false);

            modelBuilder.Entity<Flights>()
                .Property(e => e.Departure_Airport)
                .IsUnicode(false);

            modelBuilder.Entity<Flights>()
                .Property(e => e.Arrival_Airport)
                .IsUnicode(false);

            modelBuilder.Entity<Flights>()
                .Property(e => e.Starting_Price)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Flights>()
                .HasMany(e => e.Booking)
                .WithRequired(e => e.Flights)
                .WillCascadeOnDelete(false);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using K4os.Compression.LZ4.Internal;
using Microsoft.EntityFrameworkCore;

namespace VetManagement.Data
{
    public class AppDbContext: DbContext 
    {

        public DbSet<T> GetDbSet<T>() where T : class
        {
            return Set<T>();  // Return the DbSet<T> for any entity
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Med> Meds { get; set; }

        public DbSet<Patient> Patients { get; set; }

        public DbSet<Treatment> Treatments { get; set; }

        public DbSet<Owner> Owners { get; set; }

        public DbSet<TreatmentMed> TreatmentMed { get; set; }



        private readonly string _dbConnectionString = "Server=192.168.100.197;Database=inventar;Uid=root;Password=mysqlserver";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //base.OnConfiguring(optionsBuilder);
            try { 
                optionsBuilder.UseMySQL(_dbConnectionString);
            }catch( Exception e) {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Treatment>()
                .HasOne(t => t.Patient)
                .WithMany(p => p.Treatments)
                .HasForeignKey(t => t.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Treatment>()
                .HasOne(t => t.Owner)
                .WithMany(p => p.Treatments)
                .HasForeignKey(t => t.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);

            //treatment - meds many to many 
            modelBuilder.Entity<TreatmentMed>()
                .HasKey(tm => new { tm.TreatmentId, tm.MedId });

            modelBuilder.Entity<TreatmentMed>()
                .HasOne(tm => tm.Treatment)
                .WithMany(t => t.TreatmentMeds)
                .HasForeignKey(tm => tm.TreatmentId);

            modelBuilder.Entity<TreatmentMed>()
                .HasOne(tm => tm.Med)
                .WithMany(m => m.TreatmentMeds)
                .HasForeignKey(tm => tm.MedId);
            
            modelBuilder.Entity<Owner>()
               .HasMany(p => p.Patients)
               .WithOne(t => t.Owner)
               .HasForeignKey(t => t.OwnerId)
               .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Treatment>()
                .Property(t => t.Details)
                .HasColumnType("mediumtext");

            modelBuilder.Entity<Patient>()
                .Property(t => t.Details)
                .HasColumnType("mediumtext");
        }

    }
}

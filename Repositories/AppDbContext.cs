using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using K4os.Compression.LZ4.Internal;
using Microsoft.EntityFrameworkCore;
using VetManagement.Data;
using VetManagement.Services;

namespace VetManagement.Repositories
{
    public class AppDbContext : DbContext
    {

        public DbSet<T> GetDbSet<T>() where T : class
        {
            return Set<T>();
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Med> Meds { get; set; }

        public DbSet<Patient> Patients { get; set; }

        public DbSet<Treatment> Treatments { get; set; }

        public DbSet<Owner> Owners { get; set; }

        public DbSet<TreatmentMed> TreatmentMed { get; set; }

        public DbSet<TreatmentImportedMed> TreatmentImportedMed { get; set; }

        public DbSet<RegistryRecord> RegistryRecords { get; set; }

        public DbSet<Invoice> Invoices { get; set; }

        public DbSet<Recipe> Recipes { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<ImportedMed> ImportedMeds { get; set; }

        public DbSet<MedHistory> MedHistory { get; set; }
        
        public DbSet<OwnerHistory> OwnerHistory { get; set; }

        public DbSet<PatientHistory> PatientHistory { get; set; }

        public DbSet<TreatmentHistory> TreatmentHistory{ get; set; }
        
        public DbSet<RegistryRecordHistory> RegistryRecordHistory{ get; set; }
        
        public DbSet<RecipeHistory> RecipeHistory{ get; set; }

        public DbSet<TreatmentMedHistory> TreatmentMedHistory{ get; set; }



        //private readonly string _dbConnectionString = "Server=192.168.100.197;Database=inventar;Uid=root;Password=mysqlserver";
        private string _dbConnectionString;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            CreateConnectionString();
            //base.OnConfiguring(optionsBuilder);
            try
            {
                optionsBuilder.UseMySQL(_dbConnectionString);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async Task<bool> CheckConnection()
        {
            bool response = false;
            await Task.Run(() =>
            {
                try
                {
                    response = Database.CanConnect();

                    if (!response)
                    {
                        Boxes.ErrorBox("Eroare la conectare la baza de date!");
                        return false;
                    }
                    return true;
                }

                catch (Exception e)
                {
                    Logger.LogError("Error", e.ToString());
                    Boxes.ErrorBox("Eroare la conectare la baza de date!\n" + e.Message);
                    return false;
                }
            });
            return response;
        }

        private void CreateConnectionString()
        {
            try
            {
                Dictionary<string, string> settings = AppSettings.GetSettings();
                _dbConnectionString = $"Server={settings["Server"]};Database={settings["Database"]};Uid={settings["DatabaseUser"]};Password={settings["DatabasePassword"]};Connection Timeout=3;";
            }
            catch (Exception e)
            {
                Boxes.ErrorBox("Could not connect to database!\n" + e.Message);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<TreatmentMedHistory>()
                .HasOne(tm => tm.TreatmentHistory)
                .WithMany(th => th.TreatmentMedHistory)
                .HasForeignKey(tm => tm.TreatmentHistoryId)
                .OnDelete(DeleteBehavior.Cascade);

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

            //treatment - imported meds many to many 
            modelBuilder.Entity<TreatmentImportedMed>()
                .HasKey(tmm => new { tmm.TreatmentId, tmm.ImportedMedId });

            modelBuilder.Entity<TreatmentImportedMed>()
                .HasOne(tmm => tmm.Treatment)
                .WithMany(t => t.TreatmentImportedMeds)
                .HasForeignKey(tmm => tmm.TreatmentId);

            modelBuilder.Entity<TreatmentImportedMed>()
                .HasOne(tmm => tmm.ImportedMed)
                .WithMany(m => m.TreatmentImportedMeds)
                .HasForeignKey(tmm => tmm.ImportedMedId);

            modelBuilder.Entity<Owner>()
               .HasMany(p => p.Patients)
               .WithOne(t => t.Owner)
               .HasForeignKey(t => t.OwnerId)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Owner>()
                .HasIndex(o => new { o.Name, o.Phone })
                .IsUnique();


            modelBuilder.Entity<Med>()
             .HasOne(m => m.Invoice)
             .WithMany(i => i.Meds)
             .HasForeignKey(m => m.InvoiceNumber);

            modelBuilder.Entity<Recipe>()
            .HasOne(r => r.RegistryRecord)
            .WithOne(rr => rr.Recipe)
            .HasForeignKey<RegistryRecord>(rr => rr.RecipeNumber);

            modelBuilder.Entity<Treatment>()
                .Property(t => t.Details)
                .HasColumnType("mediumtext");

            modelBuilder.Entity<Patient>()
                .Property(t => t.Details)
                .HasColumnType("mediumtext");

            //modelBuilder.Entity<Patient>()
            //    .Property(p => p.Type)
            //    .HasColumnType("ENUM('pet','livestock')");

            //modelBuilder.Entity<Patient>()
            //    .Property(p => p.Sex)
            //    .HasColumnType("ENUM('Mascul','Femela')");

            //modelBuilder.Entity<Med>()
            //    .Property(m => m.Type)
            //    .HasColumnType("ENUM('medicament','vaccin')");


            //modelBuilder.Entity<User>()
            //    .Property(u => u.Role)
            //    .HasColumnType("ENUM('manager','farmacist','asistent')");

        }

    }
}

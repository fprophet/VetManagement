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
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }



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

    }
}

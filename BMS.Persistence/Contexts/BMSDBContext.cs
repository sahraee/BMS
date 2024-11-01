using BMS.Domain.Models.Book;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BMS.Persistence.Contexts
{
    public class BMSDBContext: DbContext
    {

        public BMSDBContext(DbContextOptions<BMSDBContext> options)
            : base(options)
        {

        }

        public DbSet<BookInfo> Books { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "MyDb.db" };
            var connectionString = connectionStringBuilder.ToString();
            var connection = new SqliteConnection(connectionString);

            optionsBuilder.UseSqlite(connection);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BookInfo>().HasQueryFilter(p => !p.IsRemoved);

        }

    
}
}

using Microsoft.EntityFrameworkCore;
using SigarettaBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SigarettaBot.DB_Models
{
    public class SgContext : DbContext
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Player> Players { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var serverVersion = MySqlServerVersion.AutoDetect(Settings.DbConnectionString);
            optionsBuilder.UseMySql(Settings.DbConnectionString, serverVersion);
        }
    }
}

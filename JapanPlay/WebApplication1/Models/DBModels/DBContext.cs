using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
namespace WebApplication.Models.DBModels
{
    public class DBContext : DbContext
    {
        

        public DBContext()
            : base("DefaultConnection")
        { }


        public DbSet<WebApplication.Models.DBModels.Account> Account { get; set; }
        public DbSet<WebApplication.Models.DBModels.Cost> Cost { get; set; }
        public DbSet<WebApplication.Models.DBModels.Scene> Scene { get; set; }
        public DbSet<WebApplication.Models.DBModels.Scenerank> Scenerank { get; set; }
        public DbSet<WebApplication.Models.DBModels.Job> Job { get; set; }
    }
}
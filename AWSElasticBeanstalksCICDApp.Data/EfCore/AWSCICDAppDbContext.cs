using AWSElasticBeanstalksCICDApp.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AWSElasticBeanstalksCICDApp.Data.EfCore
{
    public class AWSCICDAppDbContext:DbContext
    {
        public DbSet<UserRegister> UserRegister { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(@"server=free-verison.cw4mjrlp3wjx.us-west-2.rds.amazonaws.com;port=3306;database=AWSCICDAppDb;user=admin;password=123456789;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRegister>().HasKey(p => p.UserId);
        }
    }
}

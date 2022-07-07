using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Swift.BBS.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swift.BBS.EntityFramework.EfContext
{
    public class SwiftBbsContext : DbContext
    {
        /**
         * 此空构造函数必须要加，在进行数据库迁移时，会出现如下报错
         * Unable to create an object of type 'SwiftBbsContext'. For the different patterns supported at design time
         */
        public SwiftBbsContext() { }

        public SwiftBbsContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Article> Articles { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Article>().Property(p => p.Title).HasMaxLength(128);
            modelBuilder.Entity<Article>().Property(p => p.Submitter).HasMaxLength(64);
            modelBuilder.Entity<Article>().Property(p => p.Category).HasMaxLength(256);
            //modelBuilder.Entity<Article>().Property(p => p.Content).HasMaxLength(128);
            modelBuilder.Entity<Article>().Property(p => p.Remark).HasMaxLength(1024);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(@"Server=.;Database=SwiftCodeBbs;Trusted_Connection=True;Connection Timeout=600;MultipleActiveResultSets=true;")
                .LogTo(Console.WriteLine, LogLevel.Information);
        }
    }
}

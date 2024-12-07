using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FirstQnAAPI;

namespace FirstQnAAPI.Data
{
    public class FirstQnAAPIContext : DbContext
    {
        public FirstQnAAPIContext (DbContextOptions<FirstQnAAPIContext> options)
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<QnA>()
                .HasNoKey();
            //.HasOne(p => p.Blog)
            //.WithMany(b => b.Posts)
            //.HasForeignKey(p => p.BlogUrl)
            //.HasPrincipalKey(b => b.Url);
        }



        public DbSet<FirstQnAAPI.Question> Question { get; set; }
        public DbSet<FirstQnAAPI.QnA> QnA { get; set; } 
        public DbSet<FirstQnAAPI.Group> Group { get; set; }


    }
}

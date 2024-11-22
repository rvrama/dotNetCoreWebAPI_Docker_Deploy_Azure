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

        public DbSet<FirstQnAAPI.Question> Question { get; set; } = default!;
    }
}

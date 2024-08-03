using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using test001.Models;

namespace test001.Data
{
    public class test001Context : DbContext
    {
        public test001Context (DbContextOptions<test001Context> options)
            : base(options)
        {
        }

        public DbSet<test001.Models.Movie> Movie { get; set; } = default!;

        public DbSet<test001.Models.Staff> Staff { get; set; } = default!;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using _0611_2.Models;

namespace _0611_2.Data
{
    public class _0611_2Context : DbContext
    {
        public _0611_2Context (DbContextOptions<_0611_2Context> options)
            : base(options)
        {
        }

        public DbSet<_0611_2.Models.Schedule> Schedule { get; set; } = default!;
        public DbSet<_0611_2.Models.Doctor> Doctor { get; set; } = default!;

        public DbSet<_0611_2.Models.Customer> Customer { get; set; } = default!;
        public DbSet<_0611_2.Models.Appointment> Appointment { get; set; } = default!;


    }
}

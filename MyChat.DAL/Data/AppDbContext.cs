using Microsoft.EntityFrameworkCore;
using MyChat.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyChat.DAL.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        // Define DbSet properties for your entities here

        public DbSet<MessageModel> Messages { get; set; }
    }
}

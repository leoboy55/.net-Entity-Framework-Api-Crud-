using ApiCase.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System;

namespace ApiCase.Data
{
    public class DataContext : DbContext
    {  
        public DataContext(DbContextOptions<DataContext> options) : base(options) {}
        public DbSet<Addres> Addresses => Set<Addres>();
        
    }
}


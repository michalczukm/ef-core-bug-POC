using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

public class MyDbContext : DbContext
{
    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = @"Server=(localdb)\mssqllocaldb;Database=EFCore-Poc;Trusted_Connection=True;";
        optionsBuilder.UseSqlServer(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>().HasOne(s => s.Course).WithMany().IsRequired();
    }

    public class Student
    {
        public int Id { get; set; }
        public Course Course {get; set;}
    }

    public class Course
    {
        public int Id { get; set; }
    }
}
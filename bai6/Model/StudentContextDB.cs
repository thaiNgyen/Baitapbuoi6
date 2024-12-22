using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace bai6.Model
{
    public partial class StudentContextDB : DbContext
    {
        public StudentContextDB()
            : base("name=StudentContextDB")
        {
        }

        public virtual DbSet<Faculty> Faculties { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Faculty>()
                .HasMany(e => e.Student)
                .WithRequired(e => e.Faculty)
                .WillCascadeOnDelete(false);
        }
    }
}

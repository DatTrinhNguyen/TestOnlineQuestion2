using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace TestOnlineQuestion.Models
{
    public partial class WebDbContext : DbContext
    {
        public WebDbContext()
            : base("name=WebDbContext")
        {
        }

        public virtual DbSet<Contest> Contests { get; set; }
        public virtual DbSet<ContestQuestion> ContestQuestions { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Topic> Topics { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contest>()
                .HasMany(e => e.ContestQuestions)
                .WithRequired(e => e.Contest)
                .HasForeignKey(e => e.Idcontest)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Question>()
                .HasMany(e => e.ContestQuestions)
                .WithRequired(e => e.Question)
                .HasForeignKey(e => e.IdQuestion)
                .WillCascadeOnDelete(false);
        }
    }
}

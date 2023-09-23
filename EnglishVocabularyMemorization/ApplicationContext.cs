using EnglishVocabularyMemorization.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace EnglishVocabularyMemorization
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Word> Words { get; set; }
        public DbSet<Sentence> Sentences { get; set; }
        public DbSet<Config> Configs { get; set; }
        public DbSet<Exam> Exams { get; set; }

        public ApplicationContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

    }
}

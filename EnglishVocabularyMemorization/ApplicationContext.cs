using System.Data.Entity;
using System.Reflection;

namespace EnglishVocabularyMemorization
{
    public class ApplicationContext : DbContext
    {

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

    }
}

using EnglishVocabularyMemorization.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnglishVocabularyMemorization.DbEntitiesConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {

        }

        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Email);

            builder.HasMany(x => x.Words).WithOne(x => x.User);
        }
    }
}

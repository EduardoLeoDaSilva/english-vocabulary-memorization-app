using EnglishVocabularyMemorization.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace EnglishVocabularyMemorization.DbEntitiesConfigurations
{
    public class ConfigConfiguration : IEntityTypeConfiguration<Config>
    {
        public ConfigConfiguration()
        {

        }

        public void Configure(EntityTypeBuilder<Config> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Key);
            builder.HasIndex(x => x.Key).IsUnique(true);
            builder.Property(x => x.Value);

        }
    }
}

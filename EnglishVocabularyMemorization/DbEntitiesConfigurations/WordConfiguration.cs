using EnglishVocabularyMemorization.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnglishVocabularyMemorization.DbEntitiesConfigurations
{
    public class WordConfiguration : IEntityTypeConfiguration<Word>
    {
        public void Configure(EntityTypeBuilder<Word> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.WordUpId).IsUnique();
            builder.Property(x => x.Name);
            builder.Property(x => x.Definition);
            builder.Property(x => x.TimesReviewed);
            builder.Property(x => x.LastTimeReviewed);
            builder.Property(x => x.LastExam);
            builder.HasOne(x => x.User).WithMany(x => x.Words);
            builder.HasMany(x => x.SavedSentences).WithOne(x => x.Word);

        }
    }
}

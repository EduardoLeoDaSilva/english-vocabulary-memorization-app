using EnglishVocabularyMemorization.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace EnglishVocabularyMemorization.DbEntitiesConfigurations
{
    public class SentenceConfiguration : IEntityTypeConfiguration<Sentence>
    {
        public SentenceConfiguration()
        {

        }

        public void Configure(EntityTypeBuilder<Sentence> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Text).HasMaxLength(190);
            builder.HasIndex(x => x.Text).IsUnique(true);
            builder.Property(x => x.OriginalSentence);
            builder.Property(x => x.LastAnswers).HasConversion(x => JsonConvert.SerializeObject(x), x => JsonConvert.DeserializeObject<List<string>>(x));

            builder.HasOne(x => x.Word).WithMany(x => x.SavedSentences);
        }
    }
}

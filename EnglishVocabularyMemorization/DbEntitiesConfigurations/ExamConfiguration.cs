using EnglishVocabularyMemorization.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace EnglishVocabularyMemorization.DbEntitiesConfigurations
{
    public class ExamConfiguration : IEntityTypeConfiguration<Exam>
    {
        public ExamConfiguration()
        {

        }

        public void Configure(EntityTypeBuilder<Exam> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasMany(x => x.Sentences).WithMany(x => x.Exams);
            builder.Property(x => x.IsFinished);
            builder.Property(x => x.Email);
            builder.Property(x => x.LastExam);

        }
    }
}

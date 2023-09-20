﻿// <auto-generated />
using System;
using EnglishVocabularyMemorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EnglishVocabularyMemorization.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20230915135947_Sentence-Increased-size")]
    partial class SentenceIncreasedsize
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("EnglishVocabularyMemorization.Entities.Sentence", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("LastAnswers")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasMaxLength(190)
                        .HasColumnType("varchar(190)");

                    b.Property<Guid>("WordId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("Text")
                        .IsUnique();

                    b.HasIndex("WordId");

                    b.ToTable("Sentences");
                });

            modelBuilder.Entity("EnglishVocabularyMemorization.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Pin")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("EnglishVocabularyMemorization.Entities.Word", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Definition")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("LastTimeReviewed")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("TimesReviewed")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.Property<string>("WordUpId")
                        .IsRequired()
                        .HasColumnType("varchar(95)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("WordUpId")
                        .IsUnique();

                    b.ToTable("Words");
                });

            modelBuilder.Entity("EnglishVocabularyMemorization.Entities.Sentence", b =>
                {
                    b.HasOne("EnglishVocabularyMemorization.Entities.Word", "Word")
                        .WithMany("SavedSentences")
                        .HasForeignKey("WordId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Word");
                });

            modelBuilder.Entity("EnglishVocabularyMemorization.Entities.Word", b =>
                {
                    b.HasOne("EnglishVocabularyMemorization.Entities.User", "User")
                        .WithMany("Words")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("EnglishVocabularyMemorization.Entities.User", b =>
                {
                    b.Navigation("Words");
                });

            modelBuilder.Entity("EnglishVocabularyMemorization.Entities.Word", b =>
                {
                    b.Navigation("SavedSentences");
                });
#pragma warning restore 612, 618
        }
    }
}
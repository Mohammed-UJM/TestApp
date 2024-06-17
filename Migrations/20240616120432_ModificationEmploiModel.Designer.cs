﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TestApp.Models;

#nullable disable

namespace TestApp.Migrations
{
    [DbContext(typeof(PersonnesAppContext))]
    [Migration("20240616120432_ModificationEmploiModel")]
    partial class ModificationEmploiModel
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TestApp.Models.Emploi", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("DateDebut")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateFin")
                        .HasColumnType("datetime2");

                    b.Property<bool>("EstActuel")
                        .HasColumnType("bit");

                    b.Property<string>("NomEntreprise")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("PersonneId")
                        .HasColumnType("bigint");

                    b.Property<string>("Poste")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("PersonneId");

                    b.ToTable("Emplois");
                });

            modelBuilder.Entity("TestApp.Models.Personne", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("DateNaissance")
                        .HasColumnType("datetime2");

                    b.Property<string>("Nom")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Prenom")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Personnes");
                });

            modelBuilder.Entity("TestApp.Models.Emploi", b =>
                {
                    b.HasOne("TestApp.Models.Personne", "Personne")
                        .WithMany("Emplois")
                        .HasForeignKey("PersonneId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Personne");
                });

            modelBuilder.Entity("TestApp.Models.Personne", b =>
                {
                    b.Navigation("Emplois");
                });
#pragma warning restore 612, 618
        }
    }
}
